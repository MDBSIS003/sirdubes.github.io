using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{

    internal class ReportFiles
    {
        public static void Save(IWin32Window owner, ReportController controller, ReportDocument report)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save " + report.Title;
                dialog.Filter = "Printable HTML (*.html)|*.html|Text document (*.txt)|*.txt";
                dialog.FileName = "FuturePath-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                if (dialog.ShowDialog(owner) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    controller.Export(report, dialog.FileName);
                    MessageBox.Show(owner, "Report saved. Share only with intended recipients.", "Future Path");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(owner, ex.Message, "Save report");
                }
            }
        }

        public static void Print(IWin32Window owner, ReportDocument report)
        {
            using (ReportPrinter printer = new ReportPrinter(report))
            {
                using (PrintDialog dialog = new PrintDialog())
                {
                    dialog.Document = printer;
                    dialog.UseEXDialog = true;
                    if (dialog.ShowDialog(owner) != DialogResult.OK)
                    {
                        return;
                    }

                    try
                    {
                        printer.Print();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(owner, ex.Message, "Print report");
                    }
                }
            }
        }
    }


    internal class ReportPrinter : PrintDocument
    {
        private ReportDocument report;
        private int position;
        private int pageNumber;
        public ReportPrinter(ReportDocument report)
        {
            this.report = report;
            DocumentName = report.Title;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            position = 0;
            pageNumber = 0;
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);
            if (e.Graphics == null)
            {
                return;
            }

            pageNumber++;
            RectangleF area = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height - 32);
            string remaining = report.Content.Substring(position);
            using (Font font = new Font("Consolas", 9))
            {
                using (StringFormat format = new StringFormat(StringFormatFlags.LineLimit))
                {
                    int characters;
                    int lines;
                    e.Graphics.MeasureString(remaining, font, area.Size, format, out characters, out lines);
                    if (characters == 0)
                    {
                        throw new ValidationException("The printer has no usable page area.");
                    }

                    e.Graphics.DrawString(remaining.Substring(0, characters), font, Brushes.Black, area, format);
                    e.Graphics.DrawString("Future Path | Page " + pageNumber, font, Brushes.Gray, area.Left, e.MarginBounds.Bottom - 20);
                    position += characters;
                    e.HasMorePages = position < report.Content.Length;
                }
            }
        }
    }
}
