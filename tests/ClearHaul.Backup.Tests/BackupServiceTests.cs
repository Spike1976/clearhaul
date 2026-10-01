using ClearHaul.Backup;

namespace ClearHaul.Backup.Tests;

public sealed class BackupServiceTests
{
    [Fact]
    public void Round_trip_restores_file_bytes()
    {
        using var source = new TempDirectory();
        using var backups = new TempDirectory();
        using var restored = new TempDirectory();
        File.WriteAllText(Path.Combine(source.Path, "notes.txt"), "foundation");
        Directory.CreateDirectory(Path.Combine(source.Path, "bin"));
        File.WriteAllText(Path.Combine(source.Path, "bin", "ignored.txt"), "skip");
        var key = Enumerable.Range(1, 32).Select(value => (byte)value).ToArray();

        var created = new BackupService().Create(new BackupCreateRequest(
            source.Path,
            backups.Path,
            "worktree-1",
            DateTimeOffset.Parse("2026-09-30T12:00:00Z"),
            "milestone/m0-foundation",
            "abc123",
            "0001_foundation.sql",
            "worktree",
            key));

        new BackupService().Restore(created.ManifestPath, restored.Path, key);

        Assert.Equal("foundation", File.ReadAllText(Path.Combine(restored.Path, "notes.txt")));
        Assert.False(File.Exists(Path.Combine(restored.Path, "bin", "ignored.txt")));
        Assert.Equal("verified", BackupService.ReadManifest(created.ManifestPath).VerificationResult);
    }

    [Fact]
    public void Tampered_payload_fails_restore()
    {
        var (manifest, _, key) = CreateSample();
        var payloadPath = Path.Combine(Path.GetDirectoryName(manifest)!, "payload.bin");
        var payload = File.ReadAllBytes(payloadPath);
        payload[^1] ^= 0x01;
        File.WriteAllBytes(payloadPath, payload);

        var exception = Assert.Throws<BackupVerificationException>(() =>
            new BackupService().Restore(manifest, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")), key));
        Assert.Contains("payload", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Tampered_manifest_hash_fails_restore()
    {
        var (manifest, restoreRoot, key) = CreateSample();
        var document = BackupService.ReadManifest(manifest);
        document.Files[0] = document.Files[0] with { Sha256 = new string('a', 64) };
        File.WriteAllText(manifest, System.Text.Json.JsonSerializer.Serialize(document, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web) { WriteIndented = true }));

        Assert.Throws<BackupVerificationException>(() => new BackupService().Restore(manifest, restoreRoot, key));
    }

    [Fact]
    public void Retention_keeps_the_newest_worktree_backups_and_every_milestone()
    {
        var created = DateTimeOffset.Parse("2026-09-30T00:00:00Z");
        var candidates = Enumerable.Range(0, 50)
            .Select(index => new RetentionCandidate($"work-{index:00}", created.AddMinutes(index), "worktree"))
            .Append(new RetentionCandidate("milestone-old", created.AddMinutes(-10), "milestone"))
            .ToArray();

        var expired = BackupService.SelectExpired(candidates);

        Assert.Equal(2, expired.Count);
        Assert.DoesNotContain("milestone-old", expired);
        Assert.Contains("work-00", expired);
        Assert.Contains("work-01", expired);
        Assert.DoesNotContain("work-02", expired);
    }

    private static (string Manifest, string RestoreRoot, byte[] Key) CreateSample()
    {
        var source = new TempDirectory();
        var backups = new TempDirectory();
        File.WriteAllText(Path.Combine(source.Path, "notes.txt"), "foundation");
        var key = Enumerable.Repeat((byte)7, 32).ToArray();
        var created = new BackupService().Create(new BackupCreateRequest(
            source.Path,
            backups.Path,
            "sample",
            DateTimeOffset.Parse("2026-09-30T12:00:00Z"),
            "develop",
            "def456",
            "none",
            "worktree",
            key));
        return (created.ManifestPath, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")), key);
    }

    private sealed class TempDirectory : IDisposable
    {
        public TempDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "clearhaul-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
