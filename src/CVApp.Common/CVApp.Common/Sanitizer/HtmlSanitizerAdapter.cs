using Ganss.XSS;

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
