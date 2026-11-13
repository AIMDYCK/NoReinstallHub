using Microsoft.Win32;
using System.Drawing.Drawing2D;

namespace NoReinstallHub;

internal sealed record UiTheme(Color Background, Color Surface, Color Text, Color Muted, Color Border, Color Accent, Color AccentText)
{
    internal static UiTheme Resolve(string mode)
    {
        if (SystemInformation.HighContrast)
            return new(SystemColors.Window, SystemColors.Window, SystemColors.WindowText,
                SystemColors.WindowText, SystemColors.WindowText, SystemColors.Highlight, SystemColors.HighlightText);
        var dark = mode == "dark";
        if (mode == "system")
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                dark = key?.GetValue("AppsUseLightTheme") is int value && value == 0;
            }
            catch (Exception ex) when (ex is System.Security.SecurityException or UnauthorizedAccessException or IOException) { }
        }
        return dark
            ? new(Color.FromArgb(15, 23, 35), Color.FromArgb(24, 35, 50), Color.FromArgb(241, 245, 249),
                Color.FromArgb(174, 189, 207), Color.FromArgb(61, 78, 98), Color.FromArgb(103, 221, 204), Color.FromArgb(8, 37, 37))
            : new(Color.FromArgb(244, 247, 250), Color.White, Color.FromArgb(24, 39, 57),
                Color.FromArgb(77, 96, 117), Color.FromArgb(198, 210, 222), Color.FromArgb(0, 105, 99), Color.White);
    }
}

internal class ModernButton : Button
{
    internal UiTheme Palette { get; set; } = UiTheme.Resolve("light");
    internal bool Primary { get; set; }
    private float hover;
    private float target;
    private readonly System.Windows.Forms.Timer animator = new() { Interval = 16 };

    internal ModernButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(12, 4, 12, 4);
        MinimumSize = new Size(32, 32);
        UseVisualStyleBackColor = false;
        DoubleBuffered = true;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        Cursor = Cursors.Hand;
        animator.Tick += (_, _) =>
        {
            hover += (target - hover) * .35f;
            if (Math.Abs(target - hover) < .02f) { hover = target; animator.Stop(); }
            Invalidate();
        };
    }

    private void Animate(float value)
    {
        target = value;
        if (SystemInformation.IsMenuAnimationEnabled && !SystemInformation.HighContrast)
            animator.Start();
        else { hover = value; Invalidate(); }
    }

    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); Animate(1); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); Animate(0); }

    protected virtual Rectangle CaptionBounds => ClientRectangle;
    protected virtual TextFormatFlags CaptionFlags => TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        if (Width < 4 || Height < 4) return;
        using var shape = Shape(new Rectangle(0, 0, Width, Height), Math.Max(4, DeviceDpi / 16));
        var previous = Region;
        Region = new Region(shape);
        previous?.Dispose();
    }

    protected override void OnPaintBackground(PaintEventArgs e) => e.Graphics.Clear(Parent?.BackColor ?? Palette.Background);

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var background = Primary ? Palette.Accent : Palette.Surface;
        var foreground = Primary ? Palette.AccentText : Palette.Text;
        var tint = Primary ? Palette.AccentText : Palette.Accent;
        var amount = hover * .10f;
        background = Color.FromArgb((int)(background.R * (1 - amount) + tint.R * amount),
            (int)(background.G * (1 - amount) + tint.G * amount), (int)(background.B * (1 - amount) + tint.B * amount));
        if (!Enabled) { background = Palette.Border; foreground = Palette.Muted; }
        using var fill = new SolidBrush(background);
        using var border = new Pen(Primary ? background : Palette.Border);
        using var shape = Shape(new Rectangle(1, 1, Width - 3, Height - 3), Math.Max(4, DeviceDpi / 16));
        e.Graphics.FillPath(fill, shape);
        e.Graphics.DrawPath(border, shape);
        TextRenderer.DrawText(e.Graphics, Text, Font, CaptionBounds, foreground,
            CaptionFlags | (ShowKeyboardCues ? 0 : TextFormatFlags.HidePrefix));
        if (Focused && ShowFocusCues)
            ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(ClientRectangle, -6, -6), foreground, background);
    }

    internal static GraphicsPath Shape(Rectangle box, int radius)
    {
        var result = new GraphicsPath();
        var diameter = Math.Min(radius * 2, Math.Min(box.Width, box.Height));
        result.AddArc(box.Left, box.Top, diameter, diameter, 180, 90);
        result.AddArc(box.Right - diameter, box.Top, diameter, diameter, 270, 90);
        result.AddArc(box.Right - diameter, box.Bottom - diameter, diameter, diameter, 0, 90);
        result.AddArc(box.Left, box.Bottom - diameter, diameter, diameter, 90, 90);
        result.CloseFigure();
        return result;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) animator.Dispose();
        base.Dispose(disposing);
    }
}

internal sealed class FolderGlyph : Control
{
    internal FolderGlyph()
    {
        Size = new Size(30, 30);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        AccessibleRole = AccessibleRole.Graphic;
        TabStop = false;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.ScaleTransform(Width / 32f, Height / 32f);
        using var pen = new Pen(ForeColor, 1.7f) { LineJoin = LineJoin.Round };
        e.Graphics.DrawLines(pen, [new PointF(3, 10), new PointF(3, 6), new PointF(12, 6),
            new PointF(15, 10), new PointF(29, 10), new PointF(29, 26), new PointF(3, 26), new PointF(3, 10)]);
        e.Graphics.DrawLine(pen, 3, 12, 29, 12);
    }
}

internal sealed class ThemePicker : ModernButton
{
    internal List<string> Items { get; } = [];
    private int selectedIndex = -1;
    private ContextMenuStrip? menu;
    internal event EventHandler? SelectedIndexChanged;

    internal int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            if (value < -1 || value >= Items.Count) throw new ArgumentOutOfRangeException(nameof(value));
            var changed = selectedIndex != value;
            selectedIndex = value;
            Text = value < 0 ? "" : Items[value];
            Invalidate();
            if (changed) SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    internal ThemePicker()
    {
        AccessibleRole = AccessibleRole.ButtonMenu;
        Padding = new Padding(10, 3, 10, 3);
        MinimumSize = new Size(32, 30);
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        var size = base.GetPreferredSize(proposedSize);
        if (Items == null || Items.Count == 0) return size;
        var textWidth = Items.Max(x => TextRenderer.MeasureText(x, Font).Width);
        return new Size(Math.Max(size.Width, textWidth + Padding.Horizontal + (int)(18 * DeviceDpi / 96f)), size.Height);
    }

    protected override Rectangle CaptionBounds => new(Padding.Left, 0,
        Math.Max(1, Width - Padding.Horizontal - (int)(18 * DeviceDpi / 96f)), Height);
    protected override TextFormatFlags CaptionFlags => TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var unit = DeviceDpi / 96f;
        var x = Width - 14 * unit;
        var y = Height / 2f;
        using var pen = new Pen(Palette.Muted, 1.3f * unit) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        e.Graphics.DrawLines(pen, [new PointF(x - 3 * unit, y - unit), new PointF(x, y + 2 * unit), new PointF(x + 3 * unit, y - unit)]);
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (!Enabled || Items.Count == 0) return;
        menu?.Dispose();
        menu = new ContextMenuStrip
        {
            ShowImageMargin = false, ShowCheckMargin = true, Font = Font,
            BackColor = Palette.Surface, ForeColor = Palette.Text,
            Renderer = new ToolStripProfessionalRenderer(new PickerMenuColors(Palette)),
            AccessibleName = AccessibleName
        };
        ContextMenuStrip = menu;
        for (var i = 0; i < Items.Count; i++)
        {
            var index = i;
            var item = new ToolStripMenuItem(Items[i])
            {
                Checked = i == SelectedIndex, ForeColor = Palette.Text, BackColor = Palette.Surface,
                Padding = new Padding(6, 4, 6, 4)
            };
            item.Click += (_, _) => { SelectedIndex = index; menu.Close(); };
            menu.Items.Add(item);
        }
        menu.Show(this, new Point(0, Height + 3));
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter || keyData == (Keys.Alt | Keys.Down))
        {
            PerformClick();
            return true;
        }
        if (Items.Count > 0 && keyData is Keys.Up or Keys.Down or Keys.Home or Keys.End)
        {
            SelectedIndex = keyData switch
            {
                Keys.Home => 0,
                Keys.End => Items.Count - 1,
                Keys.Up => Math.Max(0, SelectedIndex - 1),
                _ => Math.Min(Items.Count - 1, SelectedIndex + 1)
            };
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) menu?.Dispose();
        base.Dispose(disposing);
    }

    private sealed class PickerMenuColors(UiTheme theme) : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => theme.Surface;
        public override Color MenuBorder => theme.Border;
        public override Color MenuItemBorder => theme.Border;
        public override Color MenuItemSelected => theme.Border;
        public override Color MenuItemSelectedGradientBegin => theme.Border;
        public override Color MenuItemSelectedGradientEnd => theme.Border;
        public override Color CheckBackground => theme.Border;
        public override Color CheckSelectedBackground => theme.Border;
        public override Color CheckPressedBackground => theme.Border;
        public override Color ImageMarginGradientBegin => theme.Surface;
        public override Color ImageMarginGradientMiddle => theme.Surface;
        public override Color ImageMarginGradientEnd => theme.Surface;
    }
}

internal sealed class RoundedInput : Panel
{
    internal TextBox Editor { get; }
    internal UiTheme Palette { get; set; } = UiTheme.Resolve("light");

    internal RoundedInput(TextBox editor)
    {
        Editor = editor;
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw, true);
        TabStop = false;
        Margin = new Padding(0, 4, 8, 4);
        Dock = DockStyle.Fill;
        editor.BorderStyle = BorderStyle.None;
        editor.Dock = DockStyle.Fill;
        editor.Margin = Padding.Empty;
        Controls.Add(editor);
        editor.Enter += (_, _) => Invalidate();
        editor.Leave += (_, _) => Invalidate();
        editor.FontChanged += (_, _) => UpdateMetrics();
        UpdateMetrics();
    }

    private void UpdateMetrics()
    {
        var scale = DeviceDpi / 96f;
        Padding = new Padding((int)(10 * scale), (int)(7 * scale), (int)(10 * scale), (int)(7 * scale));
        Height = Math.Max((int)(32 * scale), Editor.PreferredHeight + Padding.Vertical);
        Invalidate();
    }

    public override Size GetPreferredSize(Size proposedSize) => new(
        Math.Max(100, proposedSize.Width),
        Math.Max((int)(32 * DeviceDpi / 96f), Editor.PreferredHeight + Padding.Vertical));

    protected override void OnDpiChangedAfterParent(EventArgs e)
    {
        base.OnDpiChangedAfterParent(e);
        UpdateMetrics();
    }

    protected override void OnPaintBackground(PaintEventArgs e) => e.Graphics.Clear(Parent?.BackColor ?? Palette.Surface);

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var shape = ModernButton.Shape(new Rectangle(1, 1, Width - 3, Height - 3), Math.Max(5, DeviceDpi / 16));
        using var fill = new SolidBrush(Palette.Background);
        using var pen = new Pen(Editor.Focused ? Palette.Accent : Palette.Border, Editor.Focused ? 1.5f : 1f);
        e.Graphics.FillPath(fill, shape);
        e.Graphics.DrawPath(pen, shape);
    }
}

internal static class WindowChrome
{
    internal static int ColorRef(Color value) => value.R | (value.G << 8) | (value.B << 16);

    internal static void Apply(Form form, UiTheme theme)
    {
        if (!form.IsHandleCreated || !OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000)) return;
        // Keep the native caption, resizing and system buttons. Change only this window.
        var dark = !SystemInformation.HighContrast && theme.Background.GetBrightness() < .3 ? 1 : 0;
        var background = SystemInformation.HighContrast ? -1 : ColorRef(theme.Background);
        var foreground = SystemInformation.HighContrast ? -1 : ColorRef(theme.Text);
        _ = DwmSetWindowAttribute(form.Handle, 20, ref dark, sizeof(int));
        _ = DwmSetWindowAttribute(form.Handle, 35, ref background, sizeof(int));
        _ = DwmSetWindowAttribute(form.Handle, 36, ref foreground, sizeof(int));
    }

    [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr window, uint attribute, ref int value, int size);
}
