namespace NoReinstallHub;

public partial class MainForm
{
    private TableLayoutPanel layout = null!;
    private TableLayoutPanel body = null!;
    private Panel scrollHost = null!;
    private TableLayoutPanel welcome = null!;
    private TextBox txtOrigen = null!;
    private TextBox txtDestino = null!;
    private TextBox txtLog = null!;
    private ThemePicker themeChoice = null!;
    private ThemePicker languageChoice = null!;
    private ModernButton btnCrearJunction = null!;
    private ModernButton btnValidate = null!;
    private ModernButton btnOrigen = null!;
    private ModernButton btnDestino = null!;
    private ModernButton btnOpen = null!;
    private ModernButton btnCopy = null!;
    private ModernButton btnClear = null!;
    private ModernButton btnActivity = null!;
    private ModernButton btnHelp = null!;
    private ModernButton btnStart = null!;
    private Label status = null!;
    private Label preferenceWarning = null!;
    private ProgressBar progress = null!;
    private FlowLayoutPanel resultActions = null!;
    private readonly List<Font> ownedFonts = [];

    private Font UiFont(float size, FontStyle style = FontStyle.Regular)
    {
        var font = new Font("Segoe UI", size, style);
        ownedFonts.Add(font);
        return font;
    }

    private Label Caption(string key, float size = 10, FontStyle style = FontStyle.Regular)
        => new() { Tag = key, AutoSize = true, Dock = DockStyle.Top, Font = UiFont(size, style),
            Margin = new Padding(0, 0, 0, 6), UseMnemonic = false };

    private ModernButton ActionButton(string key, EventHandler action, bool primary = false)
    {
        var button = new ModernButton { Tag = key, Primary = primary, Margin = new Padding(4), Font = UiFont(10) };
        button.Click += action;
        return button;
    }

    private static TableLayoutPanel Stack() => new()
    {
        AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Top,
        ColumnCount = 1, Margin = Padding.Empty, Padding = Padding.Empty
    };

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "NoReinstallHub";
        Name = "MainForm";
        Font = UiFont(10);
        AutoScaleDimensions = new SizeF(96, 96);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(900, 650);
        MinimumSize = new Size(720, 560);
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = Padding.Empty };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(layout);

        var header = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, ColumnCount = 3,
            Padding = new Padding(24, 12, 24, 10), Margin = Padding.Empty };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        var brand = Stack();
        var brandName = new Label { Text = "NoReinstallHub", AutoSize = true, Font = UiFont(14, FontStyle.Bold), Margin = Padding.Empty };
        brand.Controls.Add(brandName);
        brand.Controls.Add(Caption("Tag", 9));
        header.Controls.Add(brand, 0, 0);

        var themeBox = Stack();
        themeBox.Margin = new Padding(18, 0, 8, 0);
        themeBox.Controls.Add(Caption("Theme", 9));
        themeChoice = new ThemePicker { Name = "themeChoice", 
            Width = 100, TabIndex = 0, Margin = Padding.Empty };
        themeBox.Controls.Add(themeChoice);
        header.Controls.Add(themeBox, 1, 0);
        var languageBox = Stack();
        languageBox.Margin = new Padding(8, 0, 0, 0);
        languageBox.Controls.Add(Caption("Language", 9));
        languageChoice = new ThemePicker { Name = "languageChoice", 
            Width = 112, TabIndex = 1, Margin = Padding.Empty };
        languageChoice.Items.AddRange(Texts.LanguageNames);
        languageBox.Controls.Add(languageChoice);
        header.Controls.Add(languageBox, 2, 0);
        layout.Controls.Add(header, 0, 0);

        scrollHost = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Margin = Padding.Empty,
            Padding = new Padding(24, 8, 24, 8), TabIndex = 2 };
        layout.Controls.Add(scrollHost, 0, 1);
        body = Stack();
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        scrollHost.Controls.Add(body);
        var hero = Stack();
        hero.Margin = new Padding(0, 0, 0, 12);
        hero.Controls.Add(Caption("Title", 20, FontStyle.Bold));
        hero.Controls.Add(Caption("Subtitle", 11));
        body.Controls.Add(hero);

        welcome = Stack();
        welcome.Padding = new Padding(20);
        welcome.Margin = new Padding(0, 0, 0, 16);
        welcome.Controls.Add(Caption("Welcome", 13, FontStyle.Bold));
        welcome.Controls.Add(Caption("WelcomeHint"));
        btnStart = ActionButton("Start", (_, _) => CompleteWelcome(), true);
        welcome.Controls.Add(btnStart);
        body.Controls.Add(welcome);

        txtOrigen = new TextBox { Name = "txtOrigen", BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill,
            Margin = new Padding(0, 7, 8, 7), TabIndex = 0, Font = UiFont(10) };
        btnOrigen = ActionButton("Browse", BtnOrigen_Click);
        btnOrigen.TabIndex = 1;
        body.Controls.Add(PathCard("01", "SourceTitle", "SourceHint", txtOrigen, btnOrigen));
        txtDestino = new TextBox { Name = "txtDestino", BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill,
            Margin = new Padding(0, 7, 8, 7), TabIndex = 0, Font = UiFont(10) };
        btnDestino = ActionButton("Parent", BtnDestino_Click);
        btnDestino.TabIndex = 1;
        body.Controls.Add(PathCard("02", "DestinationTitle", "DestinationHint", txtDestino, btnDestino));

        var outcome = Stack();
        outcome.Margin = new Padding(0, 10, 0, 4);
        status = new Label { Name = "operationStatus", AutoSize = true, Dock = DockStyle.Top,
            Font = UiFont(11, FontStyle.Bold), Margin = new Padding(0, 0, 0, 8), AccessibleRole = AccessibleRole.Alert };
        outcome.Controls.Add(status);
        outcome.Controls.Add(Caption("Safe", 9));
        progress = new ProgressBar { Dock = DockStyle.Top, Height = 5, Visible = false, Style = ProgressBarStyle.Marquee,
            MarqueeAnimationSpeed = 25, AccessibleName = "Progress" };
        outcome.Controls.Add(progress);
        resultActions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, WrapContents = true, Visible = false };
        btnOpen = ActionButton("Open", (_, _) => OpenDestination());
        btnCopy = ActionButton("Copy", (_, _) => CopyDestination());
        resultActions.Controls.AddRange([btnOpen, btnCopy]);
        outcome.Controls.Add(resultActions);
        preferenceWarning = Caption("PreferencesError", 9);
        preferenceWarning.Visible = false;
        outcome.Controls.Add(preferenceWarning);
        body.Controls.Add(outcome);

        btnActivity = ActionButton("Activity", (_, _) => ToggleActivity());
        body.Controls.Add(btnActivity);
        txtLog = new TextBox { Name = "txtLog", Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
            Dock = DockStyle.Top, Height = 110, Visible = false, BorderStyle = BorderStyle.FixedSingle, WordWrap = true };
        body.Controls.Add(txtLog);

        var footer = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true, Padding = new Padding(18, 8, 18, 8), Margin = Padding.Empty, TabIndex = 3 };
        btnCrearJunction = ActionButton("Create", BtnCrearJunction_Click, true);
        btnCrearJunction.Name = "btnCrearJunction";
        btnCrearJunction.TabIndex = 0;
        btnValidate = ActionButton("Validate", (_, _) => CheckPaths());
        btnValidate.TabIndex = 1;
        btnClear = ActionButton("Clear", (_, _) => ResetPaths());
        btnClear.TabIndex = 2;
        btnHelp = ActionButton("Help", (_, _) => ShowHelp());
        btnHelp.TabIndex = 3;
        footer.Controls.AddRange([btnCrearJunction, btnValidate, btnClear, btnHelp]);
        layout.Controls.Add(footer, 0, 2);
        AcceptButton = btnCrearJunction;
        txtOrigen.TextChanged += (_, _) => PathsChanged();
        txtDestino.TextChanged += (_, _) => PathsChanged();
        themeChoice.SelectedIndexChanged += (_, _) => PreferencesChanged();
        languageChoice.SelectedIndexChanged += (_, _) => PreferencesChanged();
        Resize += (_, _) => WrapLabels(this);
        ResumeLayout(true);
    }

    private TableLayoutPanel PathCard(string number, string title, string hint, TextBox editor, ModernButton browse)
    {
        var card = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 2,
            Padding = new Padding(18, 14, 18, 12), Margin = new Padding(0, 0, 0, 10), Tag = "surface" };
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58));
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        var marker = new Label { Text = number, Tag = "accent", AutoSize = true, Font = UiFont(10, FontStyle.Bold),
            Margin = new Padding(0, 0, 12, 0), AccessibleRole = AccessibleRole.StaticText };
        var badge = Stack();
        badge.Controls.Add(marker);
        badge.Controls.Add(new FolderGlyph { Tag = "accent" });
        card.Controls.Add(badge, 0, 0);
        var content = Stack();
        content.Controls.Add(Caption(title, 12, FontStyle.Bold));
        content.Controls.Add(Caption(hint));
        var fields = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 2, Margin = Padding.Empty };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        fields.Controls.Add(new RoundedInput(editor), 0, 0);
        fields.Controls.Add(browse, 1, 0);
        content.Controls.Add(fields);
        card.Controls.Add(content, 1, 0);
        return card;
    }

    private static void WrapLabels(Control container)
    {
        foreach (Control control in container.Controls)
        {
            if (control is Label label && label.Dock == DockStyle.Top)
                label.MaximumSize = new Size(Math.Max(100, container.ClientSize.Width - container.Padding.Horizontal - label.Margin.Horizontal), 0);
            if (control.HasChildren) WrapLabels(control);
        }
    }
}
