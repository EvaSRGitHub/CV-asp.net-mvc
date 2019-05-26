using Ganss.XSS;
using AngleSharp.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Sanitizer
{
    public class HtmlSanitizerAdapter : ISanitizer
    {
        public string Sanitize(string text)
        {
            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(text);
            return sanitizedContent;
        }
    }
}
