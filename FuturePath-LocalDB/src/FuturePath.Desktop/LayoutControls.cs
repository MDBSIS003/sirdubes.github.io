namespace FuturePath.Desktop;

/// <summary>Code-built pages use the same 96-DPI design baseline as their containing form.</summary>
internal class AppPage : UserControl
{
    public AppPage()
    {
        SuspendLayout();
        AutoScaleDimensions = new SizeF(96, 96);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = Ui.Body();
    }

    // Finish after constructing the whole page, including pages added after the form is visible.
    internal void FinishLayout() => ResumeLayout(true);
}

/// <summary>Fit page content to the viewport, rather than retaining a fixed minimum card width.</summary>
internal sealed class PageStack : FlowLayoutPanel
{
    private bool arranging;

    public PageStack()
    {
        Dock = DockStyle.Fill;
        FlowDirection = FlowDirection.TopDown;
        WrapContents = false;
        AutoScroll = true;
        Padding = new Padding(24);
        BackColor = Ui.Background;
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        if (!arranging)
        {
            arranging = true;
            try
            {
                int available = Math.Max(1, ClientSize.Width - Padding.Horizontal);
                foreach (Control child in Controls)
                {
                    int width = Math.Max(1, available - child.Margin.Horizontal);
                    if (child is ContentCard card)
                        card.FitWidth(width);
                    else if (child is Label { AutoSize: true } or FlowLayoutPanel { AutoSize: true })
                        child.MaximumSize = new Size(width, 0);
                }
            }
            finally
            {
                arranging = false;
            }
        }
        base.OnLayout(e);
    }
}

internal sealed class ContentCard : FlowLayoutPanel
{
    private readonly int designWidth;

    public ContentCard(int designWidth)
    {
        this.designWidth = designWidth;
        FlowDirection = FlowDirection.TopDown;
        WrapContents = false;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(22);
        BackColor = Color.White;
        Margin = new Padding(0, 0, 0, 18);
        FitWidth(designWidth);
    }

    internal void FitWidth(int available)
    {
        int width = Math.Min(available, LogicalToDeviceUnits(designWidth));
        if (MinimumSize.Width == width && MaximumSize.Width == width)
            return;
        // Clear both limits first so shrinking and growing cannot clamp against the previous width.
        MinimumSize = Size.Empty;
        MaximumSize = new Size(width, 0);
        MinimumSize = new Size(width, 0);
        Width = width;
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        int available = Math.Max(1, ClientSize.Width - Padding.Horizontal);
        foreach (Control child in Controls)
        {
            if (child is Label { AutoSize: true } or FlowLayoutPanel { AutoSize: true })
                child.MaximumSize = new Size(Math.Max(1, available - child.Margin.Horizontal), 0);
        }
        base.OnLayout(e);
    }
}
