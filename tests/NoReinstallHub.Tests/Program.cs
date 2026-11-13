using NoReinstallHub;

internal static class Program
{
    private static int passed;

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            RunAsync().GetAwaiter().GetResult();
            Console.WriteLine($"OK: {passed} comprobaciones.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception("FALLO: " + name);
        passed++;
        Console.WriteLine("OK: " + name);
    }

    private static void Reject(Action action, string name)
    {
        try { action(); }
        catch (Exception ex) when (ex is ArgumentException or IOException or UnauthorizedAccessException)
        {
            Check(true, name);
            return;
        }
        throw new Exception("FALLO: se aceptó " + name);
    }

    private static Control? Find(Control parent, string name) => parent.Controls.Find(name, true).SingleOrDefault();

    private static async Task<bool> AttemptCreate(string source, string destination)
    {
        try { await JunctionService.CreateAsync(source, destination); return true; }
        catch (IOException) { return false; }
    }

    private static async Task RunAsync()
    {
        foreach (var input in new[] { "", " ", "relative", @"C:folder", @"\folder", @"\\server\share", @"\\?\C:\folder", @"C:\", @"C:\a\..\b", @"C:\folder.", @"C:\folder ", @"C:\CON.txt", @"C:\COM1", @"C:\NUL", @"C:\a:stream", "C:\\a\nname", "C:\\a\"name" })
            Reject(() => JunctionService.NormalizePath(input), "Rechazo de ruta: " + input.Replace("\n", "[LF]"));
        foreach (var character in "|<>*?")
            Reject(() => JunctionService.NormalizePath(@"C:\folder" + character + "name"), "Carácter: " + character);
        Check(JunctionService.NormalizePath(@"C:\Juegos con espacios\Árbol (2026)\") == @"C:\Juegos con espacios\Árbol (2026)", "Espacios, Unicode y barra final");

        var workspace = Path.GetFullPath(Environment.CurrentDirectory);
        var scratch = Path.Combine(workspace, "artifacts", "tests-" + Guid.NewGuid().ToString("N"));
        if (!scratch.StartsWith(workspace + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new Exception("La carpeta temporal debe estar dentro del workspace.");
        var source = Path.Combine(scratch, "Origen con espacios á");
        var destination = Path.Combine(scratch, "Destino");
        Directory.CreateDirectory(source);
        await File.WriteAllTextAsync(Path.Combine(source, "datos.txt"), "Conservar");
        try
        {
            Reject(() => JunctionService.Validate(source, source.ToUpperInvariant()), "Origen y destino iguales");
            Reject(() => JunctionService.Validate(source, Path.Combine(source, "child")), "Destino dentro del origen");
            Reject(() => JunctionService.Validate(source, scratch), "Destino ancestro del origen");
            Reject(() => JunctionService.Validate(Path.Combine(scratch, "missing"), destination), "Origen ausente");
            Directory.CreateDirectory(destination);
            Reject(() => JunctionService.Validate(source, destination), "Destino vacío existente");
            Check(Directory.Exists(destination), "Se conserva el destino vacío");
            var marker = Path.Combine(destination, "no-borrar.txt");
            await File.WriteAllTextAsync(marker, "Datos del usuario");
            Reject(() => JunctionService.CreateAsync(source, destination).GetAwaiter().GetResult(), "Destino con datos");
            Check(await File.ReadAllTextAsync(marker) == "Datos del usuario", "Se conservan los datos del destino");
            var fileDestination = Path.Combine(scratch, "archivo.txt");
            await File.WriteAllTextAsync(fileDestination, "No reemplazar");
            Reject(() => JunctionService.Validate(source, fileDestination), "Destino es archivo");
            var sibling = source + "-sibling";
            Check(JunctionService.Validate(source, sibling).Destination == sibling, "Prefijo común no implica anidación");


        // Native backend supports literal shell characters: they are no longer commands.
        var specialSource = Path.Combine(scratch, "Origen & %literal% ! ^ á");
        var specialLink = Path.Combine(scratch, "Enlace & %literal% ! ^ á");
        Directory.CreateDirectory(specialSource);
        await File.WriteAllTextAsync(Path.Combine(specialSource, "dato.txt"), "Native");
        await JunctionService.CreateAsync(specialSource, specialLink);
        try
        {
            Check(await File.ReadAllTextAsync(Path.Combine(specialLink, "dato.txt")) == "Native", "Caracteres literales sin shell");
            var raceTasks = new[] { AttemptCreate(specialSource, Path.Combine(scratch, "race")),
                AttemptCreate(specialSource, Path.Combine(scratch, "race")) };
            var winners = await Task.WhenAll(raceTasks);
            Check(winners.Count(x => x) == 1, "Creación exclusiva: un ganador con dos operaciones");
            Directory.Delete(Path.Combine(scratch, "race"), false);
        }
        finally { Directory.Delete(specialLink, false); }

            var link = Path.Combine(scratch, "parent", "Nuevo enlace á (prueba)");
            await JunctionService.CreateAsync(source, link);
            try
            {
                Check((File.GetAttributes(link) & FileAttributes.ReparsePoint) != 0, "Junction real creada");
                Check(await File.ReadAllTextAsync(Path.Combine(link, "datos.txt")) == "Conservar", "El enlace lee los archivos reales");
                Reject(() => JunctionService.Validate(source, link), "No reemplazar junction existente");
                Reject(() => JunctionService.Validate(link, sibling), "Origen es junction");
                Reject(() => JunctionService.Validate(source, Path.Combine(link, "nested")), "Ancestro es junction");
            }
            finally
            {
                // Remove only the test link, never traverse into its target.
                Directory.Delete(link, recursive: false);
            }
            Check(await File.ReadAllTextAsync(Path.Combine(source, "datos.txt")) == "Conservar", "Origen intacto después de quitar el enlace temporal");
        }
        finally
        {
            // Verified workspace-local GUID directory, created exclusively by this test.
            Directory.Delete(scratch, recursive: true);
        }
    }
}
