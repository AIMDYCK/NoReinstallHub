using System.Text.Json;

namespace NoReinstallHub;

internal sealed record AppPreferences(string Theme = "system", string Language = "es", bool FirstRunCompleted = false)
{
    [System.Text.Json.Serialization.JsonExtensionData]
    public Dictionary<string, JsonElement>? Extra { get; init; }

    internal AppPreferences Sanitize() => this with
    {
        Theme = new[] { "system", "light", "dark" }.Contains(Theme) ? Theme : "system",
        Language = Texts.Languages.Contains(Language) ? Language : "es"
    };
}

internal sealed class SettingsStore
{
    private readonly string file;
    internal bool LoadFailed { get; private set; }
    internal SettingsStore(string? path = null) => file = path ?? Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NoReinstallHub", "preferences.json");

    internal AppPreferences Load()
    {
        try
        {
            if (!File.Exists(file)) return new AppPreferences();
            if (new FileInfo(file).Length > 65536) throw new JsonException();
            var value = JsonSerializer.Deserialize<AppPreferences>(File.ReadAllText(file), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (value == null) throw new JsonException();
            return value.Sanitize();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException or NotSupportedException)
        {
            LoadFailed = true;
            return new AppPreferences();
        }
    }

    internal bool TrySave(AppPreferences value)
    {
        // Never overwrite a file that could not be read. UI choices still work for this session.
        if (LoadFailed) return false;
        var temporary = file + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file)!);
            File.WriteAllText(temporary, JsonSerializer.Serialize(value.Sanitize(), new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temporary, file, overwrite: true);
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException)
        {
            return false;
        }
        finally
        {
            try { if (File.Exists(temporary)) File.Delete(temporary); }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { /* Only an app-owned temporary file. */ }
        }
    }
}
