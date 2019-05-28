using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Services
{
    public interface ICloudinaryService
    {
        string PublicId { get; }
        string Upload(string filePath);

        string DeleteCloudinaryImg(string publicId);
    }
}
