namespace NoReinstallHub;

public sealed class InstructionsForm : Form
{
    private readonly Font bodyFont;
    internal InstructionsForm(Texts texts, UiTheme theme)
    {
        Text = "NoReinstallHub — " + texts["HelpTitle"];
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(640, 540);
        MinimumSize = new Size(420, 300);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = theme.Background;
        HandleCreated += (_, _) => WindowChrome.Apply(this, theme);
        bodyFont = new Font("Segoe UI", 11);
        var content = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
            Dock = DockStyle.Fill, Font = bodyFont, BorderStyle = BorderStyle.None, BackColor = theme.Surface,
            ForeColor = theme.Text, Text = texts["HelpBody"].Replace("\n", Environment.NewLine),
            WordWrap = true, AccessibleName = texts["HelpTitle"] };
        Padding = new Padding(24);
        Controls.Add(content);
        KeyPreview = true;
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) bodyFont.Dispose();
    }
}
