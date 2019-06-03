using CVApp.Common.Repository;
using CVApp.Models;
using CVApp.ViewModels.PersonalInfo;
using CVApp.ViewModels.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public class ResumeService : IResumeService
    {
        private readonly ILogger<ResumeService> logger;
        private readonly IRepository<Resume> resumeRepo;
        private readonly IRepository<CVAppUser> userRepo;

        public ResumeService(ILogger<ResumeService> logger, IRepository<Resume> resumeRepo, IRepository<CVAppUser> userRepo)
        {
            this.logger = logger;
            this.resumeRepo = resumeRepo;
            this.userRepo = userRepo;
        }

        public async Task CreateResume(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var resume = new Resume
                {
                    UserId = userId
                };

                await this.resumeRepo.AddAsync(resume);

                await this.resumeRepo.SaveChangesAsync();

                var user = await this.userRepo.GetByIdAsync(userId);

                user.ResumeId = resume.Id;

                this.userRepo.Update(user);

                await this.userRepo.SaveChangesAsync();
            }
        }

        public ResumeDisplayViewModel DisplayResume(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {   
                throw new InvalidOperationException("Something went wrong.Please try again later.");
            }

            var resume = this.resumeRepo.All().Include("User").AsNoTracking().SingleOrDefault(u => u.User.UserName == userName);

            if(resume == null)
            {
                throw new NullReferenceException("Something went wrong.Please try again later.");
            }

            var model = new ResumeDisplayViewModel();

            var personalInfo = new PersonalInfoOutViewModel();

            if(resume.User != null)
            {
                personalInfo.Address = resume.User.Address;
                personalInfo.Picture = resume.User.Picture;
                personalInfo.FirstName = resume.User.FirstName;
                personalInfo.LastName = resume.User.LastName;
                personalInfo.Email = resume.User.Email;
                personalInfo.PhoneNumber = resume.User.PhoneNumber;
                personalInfo.DateOfBirth = resume.User.DateOfBirth.Value.ToShortDateString();
                personalInfo.RepoProfile = resume.User.RepoProfile;
                personalInfo.Summary = resume.User.Summary;
            }

            
            model.PersonalInfo = personalInfo;

            return model;
        }
    }
}
