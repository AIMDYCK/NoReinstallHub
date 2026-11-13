using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32;

namespace NoReinstallHub;

public partial class MainForm : Form
{
    private readonly SettingsStore settings;
    private AppPreferences preferences;
    private Texts texts;
    private UiTheme palette = UiTheme.Resolve("system");
    private bool updating;
    private bool operationInProgress;
    private bool statusIsError;
    private string statusKey = "Ready";
    private JunctionPaths? lastCreated;
    private Icon? applicationIcon;

    public MainForm() : this(new SettingsStore(), false) { }

    internal MainForm(SettingsStore store, bool skipWelcome, AppPreferences? preview = null)
    {
        settings = store;
        preferences = (preview ?? settings.Load()).Sanitize();
        if (skipWelcome) preferences = preferences with { FirstRunCompleted = true };
        texts = new Texts(preferences.Language);
        updating = true;
        InitializeComponent();
        themeChoice.Items.AddRange([texts["System"], texts["Light"], texts["Dark"]]);
        themeChoice.SelectedIndex = Array.IndexOf(new[] { "system", "light", "dark" }, preferences.Theme);
        languageChoice.SelectedIndex = Array.IndexOf(Texts.Languages, preferences.Language);
        updating = false;
        ApplyPresentation();
        welcome.Visible = !preferences.FirstRunCompleted;
        preferenceWarning.Visible = settings.LoadFailed;
        try
        {
            applicationIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Icon = applicationIcon;
        }
        catch (Exception ex) when (ex is ArgumentException or IOException or Win32Exception) { /* Default window icon remains usable. */ }
        FormClosing += (_, e) =>
        {
            if (operationInProgress) { e.Cancel = true; SetStatus("CloseBusy"); }
        };
        SystemEvents.UserPreferenceChanged += OnSystemPreferencesChanged;
        UpdateActions();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        WindowChrome.Apply(this, palette);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Small screens at 200% stay usable through the scrollable content region.
        var available = Screen.FromControl(this).WorkingArea;
        MinimumSize = new Size(Math.Min(MinimumSize.Width, available.Width), Math.Min(MinimumSize.Height, available.Height));
        Size = new Size(Math.Min(Width, available.Width), Math.Min(Height, available.Height));
        WrapLabels(this);
    }

    private void OnSystemPreferencesChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        if (!IsDisposed && IsHandleCreated)
        {
            try { BeginInvoke(ApplyPresentation); }
            catch (InvalidOperationException) { /* Window closed during the system event. */ }
        }
    }

    private void ApplyPresentation()
    {
        texts = new Texts(preferences.Language);
        palette = UiTheme.Resolve(preferences.Theme);
        updating = true;
        var selectedTheme = themeChoice.SelectedIndex;
        themeChoice.Items.Clear();
        themeChoice.Items.AddRange([texts["System"], texts["Light"], texts["Dark"]]);
        themeChoice.SelectedIndex = Math.Max(0, selectedTheme);
        themeChoice.AccessibleName = texts["Theme"] + ": " + themeChoice.Text;
        languageChoice.AccessibleName = texts["Language"] + ": " + languageChoice.Text;
        ApplyControls(this, false);
        txtOrigen.PlaceholderText = texts["SourcePlaceholder"];
        txtDestino.PlaceholderText = texts["DestinationPlaceholder"];
        txtOrigen.AccessibleName = texts["SourceTitle"];
        txtDestino.AccessibleName = texts["DestinationTitle"];
        txtLog.AccessibleName = texts["Log"];
        progress.AccessibleName = texts["Busy"];
        btnActivity.Text = texts[txtLog.Visible ? "HideActivity" : "Activity"];
        progress.MarqueeAnimationSpeed = SystemInformation.IsMenuAnimationEnabled ? 25 : 0;
        status.Text = texts[statusKey];
        status.ForeColor = statusIsError && !SystemInformation.HighContrast
            ? (palette.Background.GetBrightness() < .3 ? Color.FromArgb(255, 180, 171) : Color.FromArgb(180, 35, 24)) : palette.Text;
        updating = false;
        WindowChrome.Apply(this, palette);
        WrapLabels(this);
        Invalidate(true);
    }

    private void ApplyControls(Control parent, bool surface)
    {
        parent.BackColor = surface ? palette.Surface : palette.Background;
        parent.ForeColor = palette.Text;
        foreach (Control control in parent.Controls)
        {
            var isSurface = surface || Equals(control.Tag, "surface") || control == welcome;
            ApplyControls(control, isSurface);
            if (control.Tag is string key && Texts.All.ContainsKey(key))
            {
                control.Text = texts[key];
                control.AccessibleName = control.Text.Replace("&", "");
            }
            if (control is Label && (Equals(control.Tag, "Subtitle") || Equals(control.Tag, "Tag") ||
                Equals(control.Tag, "SourceHint") || Equals(control.Tag, "DestinationHint") || Equals(control.Tag, "Safe")))
                control.ForeColor = palette.Muted;
            if (Equals(control.Tag, "accent")) control.ForeColor = palette.Accent;
            if (control is TextBox or ComboBox) control.BackColor = palette.Surface;
            if (control is RoundedInput input)
            {
                input.Palette = palette;
                input.Editor.BackColor = palette.Background;
                input.Invalidate();
            }
            if (control is ModernButton button) { button.Palette = palette; button.Invalidate(); }
        }
    }

    private void PreferencesChanged()
    {
        if (updating || themeChoice.SelectedIndex < 0 || languageChoice.SelectedIndex < 0) return;
        preferences = preferences with { Theme = new[] { "system", "light", "dark" }[themeChoice.SelectedIndex],
            Language = Texts.Languages[languageChoice.SelectedIndex] };
        ApplyPresentation();
        preferenceWarning.Visible = !settings.TrySave(preferences);
    }

    private void CompleteWelcome()
    {
        preferences = preferences with { FirstRunCompleted = true };
        welcome.Visible = false;
        preferenceWarning.Visible = !settings.TrySave(preferences);
        UpdateActions();
        txtOrigen.Focus();
    }

    private void PathsChanged()
    {
        if (updating || operationInProgress) return;
        lastCreated = null;
        resultActions.Visible = false;
        SetStatus("Ready");
        UpdateActions();
    }

    private void UpdateActions()
    {
        var ready = !string.IsNullOrWhiteSpace(txtOrigen.Text) && !string.IsNullOrWhiteSpace(txtDestino.Text) && preferences.FirstRunCompleted;
        btnCrearJunction.Enabled = btnValidate.Enabled = ready && !operationInProgress;
        btnOrigen.Enabled = btnDestino.Enabled = btnClear.Enabled = !operationInProgress;
        txtOrigen.ReadOnly = txtDestino.ReadOnly = operationInProgress;
        themeChoice.Enabled = languageChoice.Enabled = !operationInProgress;
        btnCrearJunction.Text = texts[operationInProgress ? "Busy" : "Create"];
        progress.Visible = operationInProgress;
        resultActions.Enabled = !operationInProgress;
    }

    private void Log(string message)
    {
        if (txtLog.TextLength > 32000) txtLog.Text = txtLog.Text[^16000..];
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
    }

    private void SetStatus(string key, bool error = false)
    {
        statusKey = key;
        statusIsError = error;
        status.Text = texts[key];
        status.AccessibleName = status.Text;
        status.ForeColor = error && !SystemInformation.HighContrast
            ? (palette.Background.GetBrightness() < .3 ? Color.FromArgb(255, 180, 171) : Color.FromArgb(180, 35, 24))
            : palette.Text;
        WrapLabels(body);
    }

    private void Report(Exception exception)
    {
        var message = texts.Error(exception);
        status.Text = message;
        status.AccessibleName = message;
        statusKey = exception is JunctionException junction ? junction.Code :
            exception is UnauthorizedAccessException ? "Permission" : exception is PathTooLongException ? "PathTooLong" : "Unexpected";
        SetStatus(statusKey, true);
        Log(message);
    }

    private async void CheckPaths()
    {
        if (operationInProgress) return;
        var source = txtOrigen.Text;
        var destination = txtDestino.Text;
        operationInProgress = true;
        UpdateActions();
        SetStatus("Checking");
        try
        {
            await Task.Run(() => JunctionService.Validate(source, destination));
            SetStatus("Valid");
            Log(texts["Valid"]);
        }
        catch (Exception ex) { Report(ex); }
        finally { operationInProgress = false; UpdateActions(); }
    }

    private async void BtnCrearJunction_Click(object? sender, EventArgs e)
    {
        if (operationInProgress || !btnCrearJunction.Enabled) return;
        var source = txtOrigen.Text;
        var destination = txtDestino.Text;
        operationInProgress = true;
        lastCreated = null;
        resultActions.Visible = false;
        SetStatus("Busy");
        UpdateActions();
        try
        {
            lastCreated = await JunctionService.CreateAsync(source, destination);
            SetStatus("Success");
            Log(texts["Success"]);
            resultActions.Visible = true;
        }
        catch (Exception ex) { Report(ex); }
        finally { operationInProgress = false; UpdateActions(); }
    }

    private void BtnOrigen_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog { Description = texts["SourceHint"],
            UseDescriptionForTitle = true, ShowNewFolderButton = false };
        if (dialog.ShowDialog(this) == DialogResult.OK) txtOrigen.Text = dialog.SelectedPath;
    }

    private void BtnDestino_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog { Description = texts["DestinationHint"],
            UseDescriptionForTitle = true, ShowNewFolderButton = false };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        var name = texts["NewLinkName"];
        txtDestino.Text = Path.Combine(dialog.SelectedPath, name);
        txtDestino.Focus();
        txtDestino.Select(txtDestino.TextLength - name.Length, name.Length);
    }

    private void ResetPaths()
    {
        txtOrigen.Clear();
        txtDestino.Clear();
        txtOrigen.Focus();
    }

    private void ToggleActivity()
    {
        txtLog.Visible = !txtLog.Visible;
        btnActivity.Text = texts[txtLog.Visible ? "HideActivity" : "Activity"];
    }

    private void CopyDestination()
    {
        if (lastCreated == null) return;
        try { Clipboard.SetText(lastCreated.Destination); SetStatus("Copied"); }
        catch (System.Runtime.InteropServices.ExternalException) { SetStatus("ClipboardError", true); }
    }

    private void OpenDestination()
    {
        if (lastCreated == null) return;
        try
        {
            if (!Directory.Exists(lastCreated.Destination)) { SetStatus("OpenError", true); return; }
            using var process = Process.Start(new ProcessStartInfo { FileName = lastCreated.Destination, UseShellExecute = true });
        }
        catch (Exception ex) when (ex is Win32Exception or IOException or InvalidOperationException)
        { SetStatus("OpenError", true); }
    }

    private void ShowHelp()
    {
        using var help = new InstructionsForm(texts, palette);
        help.ShowDialog(this);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F1) { ShowHelp(); return true; }
        if (!operationInProgress)
        {
            if (keyData == (Keys.Alt | Keys.D1)) { txtOrigen.Focus(); return true; }
            if (keyData == (Keys.Alt | Keys.D2)) { txtDestino.Focus(); return true; }
            if (keyData == (Keys.Control | Keys.Enter)) { btnCrearJunction.PerformClick(); return true; }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            SystemEvents.UserPreferenceChanged -= OnSystemPreferencesChanged;
            applicationIcon?.Dispose();
        }
        base.Dispose(disposing);
        if (disposing) foreach (var font in ownedFonts) font.Dispose();
    }
}
