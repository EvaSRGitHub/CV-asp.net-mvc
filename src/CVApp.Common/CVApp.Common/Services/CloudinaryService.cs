using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Services
{
    public class CloudinaryService:ICloudinaryService
    {
        private IServiceProvider serviceProvider;
        public CloudinaryService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Upload(string filePath)
        { 
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var cloudName = configuration["Cloudinary:cloud_name"];
            var apiKey = configuration["Cloudinary:api_key"];
            var apiSecret = configuration["Cloudinary:api_secret"];

            Account account = new Account(cloudName, apiKey, apiSecret);

            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            var uploadStatus = uploadResult.StatusCode;

            //TODO -see what is received if not OK and deside how to handle it.

            if(!uploadStatus.ToString().Equals ("OK", StringComparison.OrdinalIgnoreCase))
            {
               return($"Couldn't load picture - {uploadStatus.ToString()}");
            }

            var uploadLink = uploadResult.SecureUri.ToString();

            return uploadLink;
        }

    }
}
