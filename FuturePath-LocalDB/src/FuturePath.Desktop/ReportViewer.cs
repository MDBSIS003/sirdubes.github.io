using System.Drawing.Printing;
using FuturePath.Business;

namespace FuturePath.Desktop;
internal sealed class ReportViewer : AppPage
{
    private readonly ReportDocument report;
    private int printPosition;
    private int pageNumber;
    private readonly Font printFont = new("Consolas", 9);
    public ReportViewer(AppServices services, ReportDocument report, Action back, Action? finish = null)
    {
        this.report = report;
        Dock = DockStyle.Fill;
        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(16),
            BackColor = Color.White
        };
        toolbar.Controls.Add(Ui.Button("Save report…", () =>
        {
            using var save = new SaveFileDialog
            {
                Title = "Export " + report.Title,
                Filter = "Printable HTML (*.html)|*.html|Text document (*.txt)|*.txt",
                FileName = report.SessionId.HasValue ? $"FuturePath-Advisory-{report.SessionId}" : $"FuturePath-Report-{DateTime.Now:yyyyMMdd}",
                OverwritePrompt = true
            };
            if (save.ShowDialog(this) != DialogResult.OK)
                return;
            services.Reports.Export(report, save.FileName);
            MessageBox.Show("Report saved. Share it only with intended recipients.", "Future Path");
        }, true));
        toolbar.Controls.Add(Ui.Button("Print / PDF…", Print));
        toolbar.Controls.Add(Ui.Button("← Back", back));
        if (finish != null)
            toolbar.Controls.Add(Ui.Button("Finish — overview", finish));
        var text = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            WordWrap = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None,
            Text = report.Content,
            AccessibleName = report.Title
        };
        var body = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24)
        };
        body.Controls.Add(text);
        Controls.Add(body);
        Controls.Add(toolbar);
        FinishLayout();
    }

    private void Print()
    {
        using var document = new PrintDocument
        {
            DocumentName = report.Title
        };
        document.BeginPrint += (_, _) =>
        {
            printPosition = 0;
            pageNumber = 0;
        };
        document.PrintPage += (_, e) =>
        {
            if (e.Graphics == null)
                return;
            pageNumber++;
            var area = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height - 32);
            var remaining = report.Content[printPosition..];
            using var format = new StringFormat(StringFormatFlags.LineLimit);
            e.Graphics.MeasureString(remaining, printFont, area.Size, format, out var characters, out var linesFilled);
            if (characters == 0)
                throw new ValidationException("The selected printer has no usable page area.");
            e.Graphics.DrawString(remaining[..characters], printFont, Brushes.Black, area, format);
            e.Graphics.DrawString($"Future Path | Page {pageNumber} | {report.GeneratedUtc:u}", printFont, Brushes.Gray, area.Left, e.MarginBounds.Bottom - 20);
            printPosition += characters;
            e.HasMorePages = printPosition < report.Content.Length;
        };
        using var dialog = new PrintDialog
        {
            Document = document,
            UseEXDialog = true,
            AllowSomePages = false
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            document.Print();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            printFont.Dispose();
        base.Dispose(disposing);
    }
}
