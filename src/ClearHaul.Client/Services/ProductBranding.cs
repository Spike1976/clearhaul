using System.Text.Json;

namespace ClearHaul.Client.Services;

public static class ProductBranding
{
    public static string LoadName()
    {
        var configured = Environment.GetEnvironmentVariable("CLEARHAUL_PRODUCT_NAME");
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured.Trim();
        }

        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
        {
            return "ClearHaul";
        }

        using var document = JsonDocument.Parse(File.ReadAllText(path));
        if (document.RootElement.TryGetProperty("productName", out var name)
            && name.ValueKind == JsonValueKind.String
            && !string.IsNullOrWhiteSpace(name.GetString()))
        {
            return name.GetString()!.Trim();
        }

        return "ClearHaul";
    }
}
