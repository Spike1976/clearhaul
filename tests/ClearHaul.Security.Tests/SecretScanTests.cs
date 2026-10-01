namespace ClearHaul.Security.Tests;

public sealed class SecretScanTests
{
    [Fact]
    public void Repository_does_not_contain_private_keys_or_cloud_key_ids()
    {
        var root = FindRepositoryRoot();
        var failures = new List<string>();
        foreach (var file in Enumerate(new DirectoryInfo(root)))
        {
            var name = file.Name;
            if (name.EndsWith(".pfx", StringComparison.OrdinalIgnoreCase)
                || name.EndsWith(".key", StringComparison.OrdinalIgnoreCase)
                || name.EndsWith(".dpapi", StringComparison.OrdinalIgnoreCase))
            {
                failures.Add(Relative(root, file.FullName));
                continue;
            }

            if (file.Length > 2_000_000)
            {
                continue;
            }

            string text;
            try
            {
                var bytes = File.ReadAllBytes(file.FullName);
                if (bytes.Contains((byte)0))
                {
                    continue;
                }

                text = System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch (IOException)
            {
                continue;
            }

            var privateKey = "BEGIN " + "PRIVATE KEY";
            var rsaKey = "BEGIN " + "RSA PRIVATE KEY";
            var openSshKey = "BEGIN " + "OPENSSH PRIVATE KEY";
            var accessKeyPrefix = "AK" + "IA";
            if (text.Contains(privateKey, StringComparison.Ordinal)
                || text.Contains(rsaKey, StringComparison.Ordinal)
                || text.Contains(openSshKey, StringComparison.Ordinal)
                || text.Contains(accessKeyPrefix, StringComparison.Ordinal))
            {
                failures.Add(Relative(root, file.FullName));
            }
        }

        Assert.True(failures.Count == 0, "Secret scan found: " + string.Join(", ", failures));
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "QUESTIONS_FOR_MICHAEL.md")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("The repository root was not found.");
    }

    private static IEnumerable<FileInfo> Enumerate(DirectoryInfo root)
    {
        var pending = new Stack<DirectoryInfo>();
        pending.Push(root);
        while (pending.Count > 0)
        {
            var directory = pending.Pop();
            if (directory.Name is "bin" or "obj" or ".git")
            {
                continue;
            }

            foreach (var child in directory.EnumerateDirectories())
            {
                pending.Push(child);
            }

            foreach (var file in directory.EnumerateFiles())
            {
                yield return file;
            }
        }
    }

    private static string Relative(string root, string path)
    {
        return Path.GetRelativePath(root, path);
    }
}
