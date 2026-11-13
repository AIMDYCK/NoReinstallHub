using NoReinstallHub;

internal static class UiChecks
{
    internal static IEnumerable<Control> All(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (var nested in All(child)) yield return nested;
        }
    }

    internal static void Run(Action<bool, string> check)
    {
        var scratch = Path.Combine(Environment.CurrentDirectory, "artifacts", "preferences-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(scratch);
        try
        {
            var file = Path.Combine(scratch, "preferences.json");
            var store = new SettingsStore(file);
            check(!store.Load().FirstRunCompleted, "Primer inicio conserva configuración inicial");
            check(store.TrySave(new AppPreferences("dark", "fr", true)), "Preferencias guardadas");
            check(new SettingsStore(file).Load() == new AppPreferences("dark", "fr", true), "Preferencias recuperadas");
            check(!File.ReadAllText(file).Contains("Source") && !File.ReadAllText(file).Contains("Destination"), "No persistir rutas");
            File.WriteAllText(file, "{malformed");
            var broken = new SettingsStore(file);
            check(broken.Load() == new AppPreferences() && broken.LoadFailed, "JSON inválido tiene valores seguros");
            check(!broken.TrySave(new AppPreferences("light")), "No sobrescribir configuración ilegible");
            check(File.ReadAllText(file) == "{malformed", "Configuración original inválida conservada");
            check(new AppPreferences("unknown", "xx").Sanitize() == new AppPreferences(), "Preferencias desconocidas saneadas");

            File.WriteAllText(file, "{\"Theme\":\"dark\",\"Language\":\"en\",\"FutureSetting\":42}");
            var futureStore = new SettingsStore(file);
            check(futureStore.TrySave(futureStore.Load() with { Language = "fr" }) && File.ReadAllText(file).Contains("FutureSetting"), "Préférences inconnues conservées");
            File.WriteAllText(file, new string('x', 65537));
            var oversized = new SettingsStore(file);
            oversized.Load();
            check(oversized.LoadFailed && !oversized.TrySave(new AppPreferences()) && new FileInfo(file).Length == 65537, "Préférences trop grandes conservées");
            var freshFile = Path.Combine(scratch, "fresh.json");
            using (var fresh = new MainForm(new SettingsStore(freshFile), false))
            {
                fresh.Show();
                Application.DoEvents();
                var start = All(fresh).OfType<ModernButton>().Single(x => Equals(x.Tag, "Start"));
                check(start.Visible, "Bienvenida visible en primer inicio");
                check(!All(fresh).OfType<ModernButton>().Single(x => Equals(x.Tag, "Create")).Enabled, "Creación desactivada sin rutas");
                start.PerformClick();
                check(new SettingsStore(freshFile).Load().FirstRunCompleted, "Configuración inicial recordada");
                fresh.Close();
            }


            using (var choiceForm = new MainForm(new SettingsStore(Path.Combine(scratch, "choices.json")), true))
            {
                choiceForm.Show();
                Application.DoEvents();
                var themeChoice = (ThemePicker)All(choiceForm).Single(x => x.Name == "themeChoice");
                var languageChoice = (ThemePicker)All(choiceForm).Single(x => x.Name == "languageChoice");
                themeChoice.PerformClick();
                Application.DoEvents();
                check(themeChoice.ContextMenuStrip?.Visible == true, "Menú de tema accesible y desplegable");
                ((ToolStripMenuItem)themeChoice.ContextMenuStrip!.Items[2]).PerformClick();
                languageChoice.SelectedIndex = 1;
                var saved = new SettingsStore(Path.Combine(scratch, "choices.json")).Load();
                check(saved.Theme == "dark" && saved.Language == "en", "Selectores conservan las preferencias");
                var sourceInput = (TextBox)All(choiceForm).Single(x => x.Name == "txtOrigen");
                sourceInput.Text = @"D:\KeepThisPath";
                languageChoice.SelectedIndex = 2;
                check(sourceInput.Text == @"D:\KeepThisPath", "Cambiar idioma conserva la ruta escrita");
                choiceForm.Close();
            }

            foreach (var (key, values) in Texts.All)
                check(values.Length == 4 && values.All(x => !string.IsNullOrWhiteSpace(x)), "Traducción completa: " + key);
            foreach (var theme in new[] { "light", "dark" })
            {
                var palette = UiTheme.Resolve(theme);
                check(Contrast(palette.Text, palette.Background) >= 4.5, "Contraste texto: " + theme);
                check(Contrast(palette.Muted, palette.Surface) >= 4.5, "Contraste secundario: " + theme);
                check(Contrast(palette.AccentText, palette.Accent) >= 4.5, "Contraste acción: " + theme);
            }

            foreach (var language in Texts.Languages)
            foreach (var theme in new[] { "light", "dark" })
            foreach (var factor in new[] { 1f, 1.25f, 1.5f, 2f })
            {
                using var form = new MainForm(new SettingsStore(Path.Combine(scratch, "preview.json")), true,
                    new AppPreferences(theme, language, true));
                form.Show();
                Application.DoEvents();
                var temporaryFonts = new List<Font>();
                try
                {
                    if (factor != 1)
                    {
                        var originalFonts = All(form).Prepend(form).Select(x => (Control: x, Font: x.Font)).ToArray();
                        form.AutoScaleMode = AutoScaleMode.None;
                        form.Scale(new SizeF(factor, factor));
                        foreach (var item in originalFonts)
                        {
                            var control = item.Control;
                            var scaledFont = new Font(item.Font.FontFamily, item.Font.Size * factor, item.Font.Style);
                            temporaryFonts.Add(scaledFont);
                            control.Font = scaledFont;
                        }
                        var available = Screen.FromControl(form).WorkingArea;
                        form.MinimumSize = new Size(Math.Min(form.MinimumSize.Width, available.Width), Math.Min(form.MinimumSize.Height, available.Height));
                        form.Size = new Size(Math.Min((int)(916 * factor), available.Width), Math.Min((int)(689 * factor), available.Height));
                    }
                    form.PerformLayout();
                    Application.DoEvents();
                    var context = $"{language}/{theme}/{factor:0.##}";
                    if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000) && !SystemInformation.HighContrast &&
                        DwmGetWindowAttribute(form.Handle, 35, out var captionColor, sizeof(int)) == 0)
                        check(captionColor == WindowChrome.ColorRef(UiTheme.Resolve(theme).Background), "Barra nativa con el tema: " + context);
                    check(form.AcceptButton != null, "Acción por teclado: " + context);
                    var controls = All(form).ToArray();
                    foreach (var button in controls.OfType<ModernButton>().Where(x => x.Visible))
                    {
                        var textSize = TextRenderer.MeasureText(button.Text.Replace("&", ""), button.Font);
                        check(button.Region != null && !button.Region.IsVisible(0, 0) && button.Region.IsVisible(button.Width / 2f, button.Height / 2f), "Esquinas recortadas: " + context);
                        check(button.Height <= 38 * factor, "Botón compacto: " + context + "/" + button.Tag);
                        check(button.Width >= textSize.Width && button.Height >= textSize.Height, "Botón sin recorte: " + context + "/" + button.Tag);
                    }
                    foreach (var label in controls.OfType<Label>().Where(x => x.Visible && !string.IsNullOrWhiteSpace(x.Text)))
                        check(label.Height >= label.GetPreferredSize(new Size(label.Width, 0)).Height, "Texte sans découpe: " + context + "/" + label.Tag);
                    foreach (var choice in controls.OfType<ThemePicker>())
                        check(choice.Height >= choice.Font.Height + 4, "Selector legible: " + context);
                    foreach (var input in controls.OfType<RoundedInput>())
                        check(input.Editor.BorderStyle == BorderStyle.None && input.Padding.Horizontal > 0, "Ruta con marco redondeado: " + context);
                    foreach (var editor in controls.OfType<TextBox>().Where(x => x.Visible && !x.Multiline))
                        check(editor.Width > 100 * factor && editor.Height >= editor.Font.Height, "Campo usable: " + context);
                    using var bitmap = new Bitmap(form.Width, form.Height);
                    form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                    bitmap.Save($"artifacts/visual-{language}-{theme}-{(int)(factor * 100)}.png");
                }
                finally
                {
                    form.Close();
                    foreach (var font in temporaryFonts) font.Dispose();
                }
            }
        }
        finally { Directory.Delete(scratch, true); }
    }

    [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
    private static extern int DwmGetWindowAttribute(IntPtr window, uint attribute, out int value, int size);

    internal static void CaptureDocs()
    {
        Directory.CreateDirectory("docs");
        foreach (var theme in new[] { "light", "dark" })
        {
            using var form = new MainForm(new SettingsStore(Path.Combine(Environment.CurrentDirectory, "artifacts", "docs-preview.json")), true,
                new AppPreferences(theme, "es", true));
            form.Show();
            Application.DoEvents();
            ((TextBox)All(form).Single(x => x.Name == "txtOrigen")).Text = @"D:\Juegos\MiJuego";
            ((TextBox)All(form).Single(x => x.Name == "txtDestino")).Text = @"C:\Juegos\MiJuego";
            Application.DoEvents();
            // Capture the actual application client area, not the sandbox's unthemed window frame.
            var client = form.Controls[0];
            using var bitmap = new Bitmap(client.Width, client.Height);
            client.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
            bitmap.Save(theme == "light" ? "docs/interfaz-clara.png" : "docs/interfaz-oscura.png");
            form.Close();
        }
        File.Copy("docs/interfaz-oscura.png", "docs/screenshot.png", true);
        Console.WriteLine("Capturas 1.1.2 del área cliente generadas con rutas de ejemplo; no se crearon enlaces.");
    }

    internal static void CheckCaptions(Action<bool, string> check)
    {
        foreach (var theme in new[] { "light", "dark" })
        {
            using var form = new MainForm(new SettingsStore(Path.Combine(Environment.CurrentDirectory, "artifacts", "caption-preview.json")), true,
                new AppPreferences(theme, "es", true));
            form.Show();
            Application.DoEvents();
            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000) && !SystemInformation.HighContrast &&
                DwmGetWindowAttribute(form.Handle, 35, out var captionColor, sizeof(int)) == 0)
                check(captionColor == WindowChrome.ColorRef(UiTheme.Resolve(theme).Background), "Color nativo de barra: " + theme);
            else
                Console.WriteLine("DWM no disponible para comprobar: " + theme);
            form.Close();
        }
    }

    private static double Contrast(Color a, Color b)
    {
        static double Linear(byte channel)
        {
            var value = channel / 255d;
            return value <= .04045 ? value / 12.92 : Math.Pow((value + .055) / 1.055, 2.4);
        }
        static double Luminance(Color color) => .2126 * Linear(color.R) + .7152 * Linear(color.G) + .0722 * Linear(color.B);
        var x = Luminance(a);
        var y = Luminance(b);
        return (Math.Max(x, y) + .05) / (Math.Min(x, y) + .05);
    }
}
