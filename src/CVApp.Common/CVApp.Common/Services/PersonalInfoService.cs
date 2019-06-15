using AutoMapper;
using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Models;
using CVApp.ViewModels;
using CVApp.ViewModels.PersonalInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CVApp.Common.Services
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly ILogger<PersonalInfoService> logger;
        private readonly IRepository<CVAppUser> userRepo;
        private readonly IRepository<Resume> resumeRepo;
        private readonly ISanitizer sanitizer;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IHostingEnvironment environment;

        public PersonalInfoService(ILogger<PersonalInfoService> logger, IRepository<CVAppUser> userRepo, IRepository<Resume> resumeRepo, ISanitizer sanitizer, ICloudinaryService cloudinaryService, IHostingEnvironment environment)
        {
            this.userRepo = userRepo;
            this.resumeRepo = resumeRepo;
            this.sanitizer = sanitizer;
            this.cloudinaryService = cloudinaryService;
            this.environment = environment;
           // this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<PersonalInfoViewModel> EditForm(string userName)
        {
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.UserName == userName);

            if(user == null)
            {
                return null;
            }

            PersonalInfoViewModel model = null;

            try
            {
                model = new PersonalInfoViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth.Value,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Picture = null,
                    CurrentPicture = user.Picture,
                    Address = user.Address,
                    RepoProfile = user.RepoProfile,
                    Summary = user.Summary
                };
            }
            catch (Exception e)
            {
                //Log Error
                return model;
            }
            
            return model;
        }

        public async Task SaveFormData(PersonalInfoViewModel model, string userName)
        {
            var resume = this.resumeRepo.All().Include("User").SingleOrDefault(u => u.User.UserName == userName);

            if (model.Picture == null && resume.User.Picture == null)
            {
                throw new NullReferenceException("Please provide your recent photo.");
            }

            string filePath = string.Empty;
            string pictureUrl = string.Empty;
            string publicId = string.Empty;

            if(model.Picture != null)
            {
                var name = model.FirstName + "_" + model.LastName + ".jpg";
                filePath = this.environment.WebRootPath + "/tempImgs/" + name;

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Picture.CopyToAsync(fileStream);
                }

                pictureUrl = this.cloudinaryService.Upload(filePath);

                publicId = this.cloudinaryService.PublicId;

                if (!Uri.IsWellFormedUriString(pictureUrl, UriKind.Absolute))
                {
                    throw new InvalidOperationException("Something went wrong. Please try again later.");
                }

                if(resume.User.Picture != null)
                {
                    this.cloudinaryService.DeleteCloudinaryImg(resume.User.CloudinaryPublicId);
                }

                resume.User.Picture = pictureUrl;
                resume.User.CloudinaryPublicId = publicId;

                File.Delete(filePath);
            }

            resume.User.ResumeId = resume.Id;
            resume.User.FirstName = this.sanitizer.Sanitize(model.FirstName);
            resume.User.LastName = this.sanitizer.Sanitize(model.LastName);
            resume.User.DateOfBirth = model.DateOfBirth;
            resume.User.PhoneNumber = this.sanitizer.Sanitize(model.PhoneNumber);
            resume.User.Email = this.sanitizer.Sanitize(model.Email);
            resume.User.Address = this.sanitizer.Sanitize(model.Address);
            resume.User.Summary = this.sanitizer.Sanitize(model.Summary);
            resume.User.RepoProfile = this.sanitizer.Sanitize(model.RepoProfile);

            this.userRepo.Update(resume.User);

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {

                //Log exeption
                throw new InvalidOperationException("Unable to save your data. Try again, and if the problem persists contact site administrator.");
            }
        }

        public async Task DeletePicture(string userName)
        {
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.UserName == userName);

            if (user.Picture != null)
            {
                this.cloudinaryService.DeleteCloudinaryImg(user.CloudinaryPublicId);
            }

            user.Picture = null;
            user.CloudinaryPublicId = null;

            this.userRepo.Update(user);

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                //Log exeption
                throw new InvalidOperationException("Unable to delete your picture. Try again, and if the problem persists contact site administrator.");
            }
        }
    }
}
