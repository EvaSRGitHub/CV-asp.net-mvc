using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Services
{
    public interface ICloudinaryService
    {
        string Upload(string filePath);
    }
}
