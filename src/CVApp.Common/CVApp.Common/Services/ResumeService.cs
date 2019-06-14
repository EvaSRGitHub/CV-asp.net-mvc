using CVApp.Common.Repository;
using CVApp.Models;
using CVApp.ViewModels.Education;
using CVApp.ViewModels.PersonalInfo;
using CVApp.ViewModels.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

                try
                {
                    await this.resumeRepo.SaveChangesAsync();

                    var user = await this.userRepo.GetByIdAsync(userId);

                    user.ResumeId = resume.Id;

                    this.userRepo.Update(user);

                    await this.userRepo.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
        }

        public ResumeDisplayViewModel DisplayResume(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidOperationException("Something went wrong.Please try again later.");
            }

            var resume = this.resumeRepo.All().Include("User").Include("Education").AsNoTracking().SingleOrDefault(u => u.User.UserName == userName);

            if (resume == null)
            {
                throw new NullReferenceException("Something went wrong.Please try again later.");
            }

            var model = new ResumeDisplayViewModel();

            var personalInfo = new PersonalInfoOutViewModel();

            if (resume.User != null)
            {
                personalInfo = CreatePersonalInvoDisplayVM(resume);
            }

            model.PersonalInfo = personalInfo;

            var educationCollection = new List<EducationOutViewModel>();

            if (resume.Education.Count() > 0)
            {
                educationCollection = CreateEducationDisplayVM(resume);
            }

            model.Educations = educationCollection;

            return model;
        }

        private List<EducationOutViewModel> CreateEducationDisplayVM(Resume resume)
        {
            return resume.Education.Select(e => new EducationOutViewModel
            {
                Institution = HttpUtility.HtmlDecode(e.Institution),
                FromYear = e.FromYear.ToString("MM/yyyy"),
                ToYear = e.ToYear.ToString("MM/yyyy"),
                GPA = e.GPA,
                MainSubjects = HttpUtility.HtmlDecode(e.MainSubjects),
                Diploma = HttpUtility.HtmlDecode(e.Diploma),
                City = e.City,
                Country = e.Country,
                EducationId = e.Id
            }).OrderByDescending(x => x.FromYear).ThenByDescending(x => x.ToYear).ToList();
        }

        private PersonalInfoOutViewModel CreatePersonalInvoDisplayVM(Resume resume)
        {
            return new PersonalInfoOutViewModel()
            {
                Address = resume.User.Address,
                Picture = resume.User.Picture,
                FirstName = resume.User.FirstName,
                LastName = resume.User.LastName,
                Email = resume.User.Email,
                PhoneNumber = resume.User.PhoneNumber,
                DateOfBirth = resume.User.DateOfBirth.Value.ToShortDateString(),
                RepoProfile = resume.User.RepoProfile,
                Summary = HttpUtility.HtmlDecode(resume.User.Summary),
            };
        }
    }
}
