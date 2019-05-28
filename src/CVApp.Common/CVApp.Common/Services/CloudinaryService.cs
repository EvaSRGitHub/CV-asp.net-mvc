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
        private IConfiguration configuration;

        private readonly string cloudName;
        private readonly string apiKey; 
        private readonly string apiSecret;

        private Account account;
        private Cloudinary cloudinary; 

        public CloudinaryService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = serviceProvider.GetRequiredService<IConfiguration>();
            this.cloudName = this.configuration["Cloudinary:cloud_name"];
            this.apiKey = this.configuration["Cloudinary:api_key"];
            this.apiSecret = this.configuration["Cloudinary:api_secret"];
            this.account = new Account(cloudName, apiKey, apiSecret);
            this.cloudinary = new Cloudinary(account);
        }

        public string PublicId { get; private set; }
        public string Upload(string filePath)
        { 
           
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

            this.PublicId = uploadResult.PublicId;

            return uploadLink;
        }

        public string DeleteCloudinaryImg(string publicId)
        {
            //public DelResResult DeleteResources(DelResParams parameters);

            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { publicId },
                Invalidate = true
            };

            var delResult = cloudinary.DeleteResources(delParams);

            return null;
        }

    }
}
