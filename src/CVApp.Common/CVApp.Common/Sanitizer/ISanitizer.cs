using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Sanitizer
{
    public interface ISanitizer
    {
        string Sanitize(string text);
    }
}
