namespace NoReinstallHub;

internal sealed record JunctionPaths(string Source, string Destination);
internal sealed class JunctionException(string code, Exception? inner = null) : IOException(code, inner)
{
    internal string Code { get; } = code;
}

internal static class JunctionService
{
    internal static string NormalizePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new JunctionException("MissingPaths");
        if (value.Any(char.IsControl) || value.IndexOfAny(['"', '|', '<', '>', '*', '?']) >= 0)
            throw new JunctionException("InvalidPath");
        if (value.Length < 3 || !char.IsAsciiLetter(value[0]) || value[1] != ':' || value[2] != '\\' || value[2..].Contains(':'))
            throw new JunctionException("LocalPath");
        var full = Path.TrimEndingDirectorySeparator(Path.GetFullPath(value));
        if (full.Equals(Path.GetPathRoot(full), StringComparison.OrdinalIgnoreCase))
            throw new JunctionException("RootPath");
        foreach (var part in value[3..].Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries))
        {
            if (part is "." or ".." || part.EndsWith(' ') || part.EndsWith('.'))
                throw new JunctionException("InvalidPath");
            var stem = part.Split('.')[0];
            if (new[] { "CON", "PRN", "AUX", "NUL", "CONIN$", "CONOUT$" }.Contains(stem, StringComparer.OrdinalIgnoreCase) ||
                System.Text.RegularExpressions.Regex.IsMatch(stem, @"^(COM|LPT)[1-9¹²³]$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                throw new JunctionException("ReservedName");
        }
        if (full.Length > 4000) throw new JunctionException("PathTooLong");
        return full;
    }

    internal static JunctionPaths Validate(string source, string destination)
    {
        source = NormalizePath(source);
        destination = NormalizePath(destination);
        if (source.Equals(destination, StringComparison.OrdinalIgnoreCase) ||
            source.StartsWith(destination + "\\", StringComparison.OrdinalIgnoreCase) ||
            destination.StartsWith(source + "\\", StringComparison.OrdinalIgnoreCase))
            throw new JunctionException("OverlappingPaths");
        RejectReparseAncestors(source);
        RejectReparseAncestors(destination);
        if (!Directory.Exists(source))
            throw new JunctionException("SourceMissing");
        if (Path.Exists(destination))
            throw new JunctionException("DestinationExists");
        var drive = new DriveInfo(Path.GetPathRoot(destination)!);
        if (drive.DriveType == DriveType.Network || !drive.DriveFormat.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
            throw new JunctionException("NtfsRequired");
        return new JunctionPaths(source, destination);
    }

    private static void RejectReparseAncestors(string path)
    {
        for (string? current = path; current != null; current = Path.GetDirectoryName(current))
        {
            try
            {
                if ((File.GetAttributes(current) & FileAttributes.ReparsePoint) != 0)
                    throw new JunctionException("ExistingLink");
            }
            catch (FileNotFoundException) { /* A new destination is expected. */ }
            catch (DirectoryNotFoundException) { /* Its parent may also be new. */ }
        }
    }

    internal static Task<JunctionPaths> CreateAsync(string source, string destination) => Task.Run(() =>
    {
        var paths = Validate(source, destination);
        Directory.CreateDirectory(Path.GetDirectoryName(paths.Destination)!);
        paths = Validate(paths.Source, paths.Destination);
        NativeJunction.Create(paths.Source, paths.Destination);
        var target = new DirectoryInfo(paths.Destination).ResolveLinkTarget(false);
        if (target == null || !Path.TrimEndingDirectorySeparator(target.FullName).Equals(paths.Source, StringComparison.OrdinalIgnoreCase))
            throw new JunctionException("Unconfirmed");
        return paths;
    });
}
