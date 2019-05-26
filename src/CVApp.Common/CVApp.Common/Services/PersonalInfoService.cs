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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       // private readonly IMapper mapper;

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

        public Task EditForm(PersonalInfoViewModel model)
        {
            throw new NotImplementedException();
        }

        public PersonalInfoOutViewModel GetFormToEditOrDelete(string userIdentifier)
        {
            var user = this.userRepo.All().SingleOrDefault(u => u.UserName == userIdentifier);

            var model = new PersonalInfoOutViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth.Value.ToString("MM/dd/yyyy"),
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Picture = user.Picture,
                Email = user.Email,
                RepoProfile = user.RepoProfile,
                Summary = this.sanitizer.Sanitize(user.Summary)
            };

            return model;
        }

        public Task MarkFormAsDeleted(PersonalInfoViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task SaveFormData(PersonalInfoViewModel model, string userName)
        {
            //check for vlidity???

            var name = model.FirstName + "_" + model.LastName + ".jpg";
            var filePath = this.environment.WebRootPath + "/tempImgs/" + name;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Picture.CopyToAsync(fileStream);
            }

            var pictrueUrl = this.cloudinaryService.Upload(filePath);

            if (!Uri.IsWellFormedUriString(pictrueUrl, UriKind.Absolute))
            {
                //TODO - Error View
            }

            var resume = this.resumeRepo.All().Include("User").SingleOrDefault(u => u.User.UserName == userName);
            var user = this.userRepo.All().SingleOrDefault(u => u.UserName == userName);

            user.ResumeId = resume.Id;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.Address = model.Address;
            user.Summary = model.Summary;
            user.Picture = pictrueUrl;
            user.RepoProfile = model.RepoProfile;

            File.Delete(filePath);

            this.userRepo.Update(user);

            await this.userRepo.SaveChangesAsync();
        }
    }
}
