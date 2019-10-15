using System;
using System.Diagnostics;
using System.Text;
using IronPdf;

namespace CVApp.Common.IronPdfConverter
{
    public class IronHtmlToPdfConverter : IIronHtmlToPdfConverter
    {
        public void GetHTMLPageAsPDF(string basePath, string htmlCode)
        {
            var Renderer = new HtmlToPdf();
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Screen;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Portrait;
            Renderer.PrintOptions.InputEncoding = Encoding.UTF8;
            Renderer.PrintOptions.EnableJavaScript = true;
            Renderer.PrintOptions.RenderDelay = 500;
            Renderer.PrintOptions.CustomCssUrl = new Uri("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css").ToString();
            Renderer.PrintOptions.CustomCssUrl = "~/css/print.css";
            //Set the width of the resposive virtual browser window in pixels
            Renderer.PrintOptions.ViewPortWidth = 1280;

            var PDF = Renderer.RenderHtmlAsPdf(htmlCode);
            var OutputPath = $"{basePath}\\IronPdfHtml.pdf";
            PDF.SaveAs(OutputPath);
            // This neat trick opens our PDF file so we can see the result in our default PDF viewer
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = OutputPath,
                UseShellExecute = true
            };
            Process.Start(psi);

            ////return a  pdf document from a view
            //var contentLength = PDF.BinaryData.Length;
            //Response.AppendHeader("Content-Length", contentLength.ToString());
            //Response.AppendHeader("Content-Disposition", "inline; filename=Document_" + id + ".pdf");
            //return File(PDF.BinaryData, "application/pdf;");
        }

    }
}
