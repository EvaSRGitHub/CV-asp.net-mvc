using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.IronPdfConverter
{
    public interface IIronHtmlToPdfConverter
    {
        void GetHTMLPageAsPDF(string basePath, string htmlCode);
    }
}
