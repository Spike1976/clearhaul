using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ClearHaul.Backup;

public sealed class BackupVerificationException : Exception
{
    public BackupVerificationException(string message)
        : base(message)
    {
    }
}

public sealed record BackupFileEntry(string RelativePath, string Sha256, long Length);

public sealed class BackupManifest
{
    public string BackupId { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }

    public string ProjectCommit { get; set; } = "";

    public string Branch { get; set; } = "";

    public string[] IncludedComponents { get; set; } = [];

    public string[] ExcludedComponents { get; set; } = [];

    public string DatabaseMigrationVersion { get; set; } = "none";

    public string EncryptionAlgorithm { get; set; } = "AES-256-GCM";

    public string EncryptionStatus { get; set; } = "encrypted";

    public string ToolVersion { get; set; } = BackupService.ToolVersion;

    public string RestoreInstructions { get; set; } = "Decrypt payload.bin with the DPAPI-wrapped key and verify every file hash before use.";

    public string VerificationResult { get; set; } = "not-verified";

    public BackupFileEntry[] Files { get; set; } = [];

    public string PayloadFile { get; set; } = "payload.bin";

    public string PayloadSha256 { get; set; } = "";

    public string RetentionClass { get; set; } = "worktree";
}

public sealed record RetentionCandidate(string BackupId, DateTimeOffset CreatedAt, string RetentionClass);

public sealed record BackupCreateRequest(
    string SourceDirectory,
    string DestinationRoot,
    string BackupId,
    DateTimeOffset CreatedAt,
    string Branch,
    string Commit,
    string DatabaseMigrationVersion,
    string RetentionClass,
    byte[] ContentKey);

public sealed record BackupCreateResult(string ManifestPath, string PayloadPath);

public sealed class BackupService
{
    public const string ToolVersion = "0.1.0-foundation";

    public static readonly string[] ExcludedDirectoryNames =
    [
        "bin",
        "obj",
        ".git",
        "node_modules",
        "TestResults",
        ".vs"
    ];

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public BackupCreateResult Create(BackupCreateRequest request)
    {
        if (request.ContentKey.Length != 32)
        {
            throw new ArgumentException("The backup key must be 32 bytes.", nameof(request));
        }

        var source = Path.GetFullPath(request.SourceDirectory);
        var destination = Path.Combine(Path.GetFullPath(request.DestinationRoot), request.BackupId);
        Directory.CreateDirectory(destination);

        var files = new List<BackupFileEntry>();
        using var plaintext = new MemoryStream();
        using (var archive = new ZipArchive(plaintext, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var file in EnumerateFiles(new DirectoryInfo(source)))
            {
                var relative = Path.GetRelativePath(source, file.FullName).Replace('\\', '/');
                var bytes = File.ReadAllBytes(file.FullName);
                files.Add(new BackupFileEntry(relative, Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant(), bytes.Length));
                var entry = archive.CreateEntry(relative, CompressionLevel.SmallestSize);
                using var entryStream = entry.Open();
                entryStream.Write(bytes);
            }
        }

        var payload = Encrypt(plaintext.ToArray(), request.ContentKey);
        var payloadPath = Path.Combine(destination, "payload.bin");
        File.WriteAllBytes(payloadPath, payload);

        var manifest = new BackupManifest
        {
            BackupId = request.BackupId,
            CreatedAt = request.CreatedAt,
            ProjectCommit = request.Commit,
            Branch = request.Branch,
            IncludedComponents = ["worktree"],
            ExcludedComponents = ExcludedDirectoryNames,
            DatabaseMigrationVersion = request.DatabaseMigrationVersion,
            ToolVersion = ToolVersion,
            Files = files.OrderBy(file => file.RelativePath, StringComparer.Ordinal).ToArray(),
            PayloadFile = "payload.bin",
            PayloadSha256 = Convert.ToHexString(SHA256.HashData(payload)).ToLowerInvariant(),
            RetentionClass = request.RetentionClass
        };

        var manifestPath = Path.Combine(destination, "manifest.json");
        File.WriteAllText(manifestPath, JsonSerializer.Serialize(manifest, JsonOptions));
        return new BackupCreateResult(manifestPath, payloadPath);
    }

    public void Restore(string manifestPath, string destinationDirectory, byte[] contentKey)
    {
        var manifest = ReadManifest(manifestPath);
        var payloadPath = Path.Combine(Path.GetDirectoryName(manifestPath)!, manifest.PayloadFile);
        var payload = File.ReadAllBytes(payloadPath);
        var actualPayloadHash = Convert.ToHexString(SHA256.HashData(payload)).ToLowerInvariant();
        if (!string.Equals(actualPayloadHash, manifest.PayloadSha256, StringComparison.Ordinal))
        {
            throw new BackupVerificationException("The payload hash does not match the manifest.");
        }

        var plaintext = Decrypt(payload, contentKey);
        Directory.CreateDirectory(destinationDirectory);
        var destinationRoot = Path.GetFullPath(destinationDirectory);
        using var archive = new ZipArchive(new MemoryStream(plaintext), ZipArchiveMode.Read);
        foreach (var expected in manifest.Files)
        {
            if (expected.RelativePath.Contains("..", StringComparison.Ordinal)
                || Path.IsPathRooted(expected.RelativePath))
            {
                throw new BackupVerificationException("The manifest contains an unsafe file path.");
            }

            var entry = archive.GetEntry(expected.RelativePath)
                ?? throw new BackupVerificationException("A manifest file is missing from the payload.");
            using var entryStream = entry.Open();
            using var memory = new MemoryStream();
            entryStream.CopyTo(memory);
            var bytes = memory.ToArray();
            var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
            if (!string.Equals(hash, expected.Sha256, StringComparison.Ordinal) || bytes.Length != expected.Length)
            {
                throw new BackupVerificationException("A restored file hash does not match the manifest.");
            }

            var outputPath = Path.GetFullPath(Path.Combine(destinationDirectory, expected.RelativePath.Replace('/', Path.DirectorySeparatorChar)));
            if (!outputPath.StartsWith(destinationRoot, StringComparison.OrdinalIgnoreCase))
            {
                throw new BackupVerificationException("The manifest contains an unsafe file path.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            File.WriteAllBytes(outputPath, bytes);
        }

        manifest.VerificationResult = "verified";
        File.WriteAllText(manifestPath, JsonSerializer.Serialize(manifest, JsonOptions));
    }

    public static IReadOnlyList<string> SelectExpired(IEnumerable<RetentionCandidate> candidates, int keepNewestWorktree = 48)
    {
        return candidates
            .Where(candidate => string.Equals(candidate.RetentionClass, "worktree", StringComparison.Ordinal))
            .OrderByDescending(candidate => candidate.CreatedAt)
            .ThenBy(candidate => candidate.BackupId, StringComparer.Ordinal)
            .Skip(keepNewestWorktree)
            .Select(candidate => candidate.BackupId)
            .ToArray();
    }

    public static BackupManifest ReadManifest(string manifestPath)
    {
        return JsonSerializer.Deserialize<BackupManifest>(File.ReadAllText(manifestPath), JsonOptions)
            ?? throw new BackupVerificationException("The manifest could not be read.");
    }

    private static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo root)
    {
        var pending = new Stack<DirectoryInfo>();
        pending.Push(root);
        while (pending.Count > 0)
        {
            var directory = pending.Pop();
            IEnumerable<DirectoryInfo> children;
            try
            {
                children = directory.EnumerateDirectories();
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

            foreach (var child in children)
            {
                if (ExcludedDirectoryNames.Contains(child.Name, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                pending.Push(child);
            }

            foreach (var file in directory.EnumerateFiles())
            {
                yield return file;
            }
        }
    }

    private static byte[] Encrypt(byte[] plaintext, byte[] key)
    {
        var nonce = RandomNumberGenerator.GetBytes(12);
        var tag = new byte[16];
        var cipher = new byte[plaintext.Length];
        using var aes = new AesGcm(key, tag.Length);
        aes.Encrypt(nonce, plaintext, cipher, tag);
        var payload = new byte[5 + nonce.Length + tag.Length + cipher.Length];
        Encoding.ASCII.GetBytes("CHBK1").CopyTo(payload, 0);
        nonce.CopyTo(payload, 5);
        tag.CopyTo(payload, 17);
        cipher.CopyTo(payload, 33);
        return payload;
    }

    private static byte[] Decrypt(byte[] payload, byte[] key)
    {
        if (payload.Length < 33 || Encoding.ASCII.GetString(payload, 0, 5) != "CHBK1")
        {
            throw new BackupVerificationException("The payload header is not a ClearHaul backup.");
        }

        var nonce = payload[5..17];
        var tag = payload[17..33];
        var cipher = payload[33..];
        var plaintext = new byte[cipher.Length];
        try
        {
            using var aes = new AesGcm(key, tag.Length);
            aes.Decrypt(nonce, cipher, tag, plaintext);
        }
        catch (AuthenticationTagMismatchException)
        {
            throw new BackupVerificationException("The payload could not be authenticated.");
        }

        return plaintext;
    }
}
