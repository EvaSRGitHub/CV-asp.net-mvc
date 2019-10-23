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
using System.Web;
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
        private readonly int? resumeId;

        public PersonalInfoService(IRepository<CVAppUser> userRepo, IRepository<Resume> resumeRepo, ISanitizer sanitizer, ICloudinaryService cloudinaryService, IHostingEnvironment environment, IHttpContextAccessor accessor)
        {
            this.userRepo = userRepo;
            this.resumeRepo = resumeRepo;
            this.sanitizer = sanitizer;
            this.cloudinaryService = cloudinaryService;
            this.environment = environment;
            this.accessor = accessor;
            this.resumeId = this.userRepo.All().SingleOrDefault(u => u.UserName == this.accessor.HttpContext.User.Identity.Name).ResumeId.GetValueOrDefault();
        }

        public async Task<PersonalInfoEditViewModel> EditForm(int resumeId)
        {
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == this.resumeId);

            if (user == null || this.resumeId != resumeId)
            {
                return null;
            }

            PersonalInfoEditViewModel model = new PersonalInfoEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Picture = null,
                CurrentPicture = user.Picture,
                RepoProfile = user.RepoProfile,
                Summary = user.Summary,
                ResumeId = this.resumeId.Value,
            };
            
            return model;
        }

        public async Task SaveFormData(PersonalInfoBaseViewModel model)
        {
            var personalInfo = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == this.resumeId);

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
            personalInfo.PhoneNumber = this.sanitizer.Sanitize(model.PhoneNumber);
            personalInfo.Email = this.sanitizer.Sanitize(model.Email);
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

        public async Task DeletePicture()
        {
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == this.resumeId);

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

        public async Task<bool> HasPersonalInfoFormFilled()
        {
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == this.resumeId);
            return user.FirstName == null ? false : true;
        }

        public async Task<PersonalInfoOutViewModel> GetPersonalInfo(int resumeId)
        {
            if (this.resumeId != resumeId)
            {
                return null;
            }

            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == this.resumeId);

            if (user == null)
            {
                return null;
            }

            var model = new PersonalInfoOutViewModel()
            {
                Id = user.Id,
                Picture = user.Picture,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RepoProfile = user.RepoProfile,
                Summary = HttpUtility.HtmlDecode(user.Summary),
                ResumeId = this.resumeId.Value
            };

            return model;
        }
    }
}
