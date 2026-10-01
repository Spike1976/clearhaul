using System.Security.Cryptography;
using System.Text.Json;
using ClearHaul.Backup;

if (args.Length == 0)
{
    Console.Error.WriteLine("Use create or restore.");
    return 1;
}

try
{
    return args[0] switch
    {
        "create" => Create(args),
        "restore" => Restore(args),
        _ => Fail("Use create or restore.")
    };
}
catch (Exception exception) when (exception is ArgumentException or BackupVerificationException or InvalidOperationException or IOException or JsonException or CryptographicException)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}

static int Create(string[] args)
{
    var source = Required(args, "--source");
    var destination = Required(args, "--dest");
    var backupId = Required(args, "--id");
    var branch = Required(args, "--branch");
    var commit = Required(args, "--commit");
    var keyOut = Required(args, "--key-out");
    var migration = Optional(args, "--migration") ?? "none";
    var retention = Optional(args, "--retention") ?? "worktree";
    var key = RandomNumberGenerator.GetBytes(32);
    try
    {
        new BackupService().Create(new BackupCreateRequest(
            source,
            destination,
            backupId,
            DateTimeOffset.Now,
            branch,
            commit,
            migration,
            retention,
            key));
        var wrapped = ProtectedData.Protect(key, EncodingEntropy(), DataProtectionScope.CurrentUser);
        File.WriteAllText(keyOut, JsonSerializer.Serialize(new
        {
            protection = "dpapi",
            wrappedKey = Convert.ToBase64String(wrapped)
        }));
    }
    finally
    {
        CryptographicOperations.ZeroMemory(key);
    }

    return 0;
}

static int Restore(string[] args)
{
    var manifest = Required(args, "--manifest");
    var destination = Required(args, "--dest");
    var keyFile = Required(args, "--key");
    using var document = JsonDocument.Parse(File.ReadAllText(keyFile));
    var protection = document.RootElement.GetProperty("protection").GetString();
    if (!string.Equals(protection, "dpapi", StringComparison.Ordinal))
    {
        throw new InvalidOperationException("The key file is not a DPAPI backup key.");
    }

    var wrapped = Convert.FromBase64String(document.RootElement.GetProperty("wrappedKey").GetString()!);
    var key = ProtectedData.Unprotect(wrapped, EncodingEntropy(), DataProtectionScope.CurrentUser);
    try
    {
        new BackupService().Restore(manifest, destination, key);
    }
    finally
    {
        CryptographicOperations.ZeroMemory(key);
    }

    return 0;
}

static byte[] EncodingEntropy()
{
    return System.Text.Encoding.UTF8.GetBytes("ClearHaul.Backup.v1");
}

static string Required(string[] args, string name)
{
    return Optional(args, name) ?? throw new ArgumentException($"Missing {name}.");
}

static string? Optional(string[] args, string name)
{
    for (var index = 0; index < args.Length - 1; index++)
    {
        if (string.Equals(args[index], name, StringComparison.Ordinal))
        {
            return args[index + 1];
        }
    }

    return null;
}

static int Fail(string message)
{
    Console.Error.WriteLine(message);
    return 1;
}
