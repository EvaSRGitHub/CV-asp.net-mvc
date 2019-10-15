using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Common.Services.Contracts;
using CVApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CVApp.ViewModels.PersonalInfo.PersonalInfoViewModels;

namespace CVApp.Common.Services
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly IRepository<CVAppUser> userRepo;
        private readonly IRepository<Resume> resumeRepo;
        //The Sanitizer remove the whole text between the script tags, together with the tags;
        private readonly ISanitizer sanitizer;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IHostingEnvironment environment;
        private readonly IHttpContextAccessor accessor;

        public PersonalInfoService(IRepository<CVAppUser> userRepo, IRepository<Resume> resumeRepo, ISanitizer sanitizer, ICloudinaryService cloudinaryService, IHostingEnvironment environment, IHttpContextAccessor accessor)
        {
            this.userRepo = userRepo;
            this.resumeRepo = resumeRepo;
            this.sanitizer = sanitizer;
            this.cloudinaryService = cloudinaryService;
            this.environment = environment;
            this.accessor = accessor;
        }

        public async Task<PersonalInfoEditViewModel> EditForm(string userName)
        {
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.UserName == userName);
            var currentUser = this.accessor.HttpContext.User.Identity.Name;

            if (user == null || currentUser != userName)
            {
                return null;
            }

            PersonalInfoEditViewModel model = new PersonalInfoEditViewModel
            {
                Id = user.Id,
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
            
            return model;
        }

        public async Task SaveFormData(PersonalInfoBaseViewModel model, string userName)
        {
            var personalInfo = this.userRepo.All().SingleOrDefault(u => u.UserName == userName);

            string filePath = string.Empty;
            string pictureUrl = string.Empty;
            string publicId = string.Empty;

            if (model.Picture != null)
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

                if (personalInfo.Picture != null)
                {
                    this.cloudinaryService.DeleteCloudinaryImg(personalInfo.CloudinaryPublicId);
                }

                personalInfo.Picture = pictureUrl;
                personalInfo.CloudinaryPublicId = publicId;

                File.Delete(filePath);
            }

            personalInfo.FirstName = this.sanitizer.Sanitize(model.FirstName);
            personalInfo.LastName = this.sanitizer.Sanitize(model.LastName);
            personalInfo.DateOfBirth = model.DateOfBirth;
            personalInfo.PhoneNumber = this.sanitizer.Sanitize(model.PhoneNumber);
            personalInfo.Email = this.sanitizer.Sanitize(model.Email);
            personalInfo.Address = this.sanitizer.Sanitize(model.Address);
            personalInfo.Summary = this.sanitizer.Sanitize(model.Summary);
            personalInfo.RepoProfile = this.sanitizer.Sanitize(model.RepoProfile);

            this.userRepo.Update(personalInfo);

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
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
                throw new InvalidOperationException("Unable to delete your picture. Try again, and if the problem persists contact site administrator.");
            }
        }

        public async Task<bool> HasPersonalInfoFormFilled(string userName)
        {
            return await this.userRepo.All().SingleOrDefaultAsync(u => u.UserName == userName) == null ? false : true;
        }
    }
}
