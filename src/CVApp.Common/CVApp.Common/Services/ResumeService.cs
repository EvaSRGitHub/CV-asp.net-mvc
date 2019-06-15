using CVApp.Common.Repository;
using CVApp.Models;
using CVApp.ViewModels.Education;
using CVApp.ViewModels.Language;
using CVApp.ViewModels.PersonalInfo;
using CVApp.ViewModels.Resume;
using CVApp.ViewModels.Work;
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

        public async Task<ResumeDisplayViewModel> DisplayResume(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidOperationException("Something went wrong.Please try again later.");
            }

            var resume = await this.resumeRepo.All()
                .Include("User")
                .Include("Education")
                .Include("Works")
                .Include("Languages")
                .AsNoTracking().SingleOrDefaultAsync(u => u.User.UserName == userName);

            if (resume == null)
            {
                throw new NullReferenceException("Something went wrong.Please try again later.");
            }

            ResumeDisplayViewModel model = new ResumeDisplayViewModel();

            PersonalInfoOutViewModel personalInfo  = CreatePersonalInvoDisplayVM(resume);

            model.PersonalInfo = personalInfo;

            var educationCollection = new List<EducationOutViewModel>();

            if (resume.Education.Count() > 0)
            {
                educationCollection = CreateEducationDisplayVM(resume);
            }

            model.Educations = educationCollection;

            var workCollection = new List<WorkOutViewModel>();

            if (resume.Works.Count() > 0)
            {
                workCollection = CreateWorkDisplayVM(resume);
            }

            model.Employments = workCollection;

            var languageCollection = new List<LanguageOutViewModel>();

            if(resume.Languages.Count() > 0)
            {
                languageCollection = CreateLanguageDisplayVM(resume);
            }

            model.Languages = languageCollection;

            return model;
        }

        private List<LanguageOutViewModel> CreateLanguageDisplayVM(Resume resume)
        {
            return resume.Languages.Select(l => new LanguageOutViewModel
            {
                Name = l.Name,
                Level = l.Level,
                Id = l.Id
            }).ToList();
        }

        private List<WorkOutViewModel> CreateWorkDisplayVM(Resume resume)
        {
            return resume.Works.Select(w => new WorkOutViewModel
            {
                Company = HttpUtility.HtmlDecode(w.Company),
                Title = HttpUtility.HtmlDecode(w.Title),
                FromYear = w.FromYear.ToString("MM/yyyy"),
                ToYear = w.ToYear.HasValue? w.ToYear.Value.ToString("MM/yyyy") : "",
                Description = HttpUtility.HtmlDecode(w.Description),
                Country = w.Country,
                City = w.City,
                Id = w.Id
            }).OrderByDescending(x => x.FromYear).ThenByDescending(x => x.ToYear).ToList();
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
                DateOfBirth = resume.User.DateOfBirth.HasValue ? resume.User.DateOfBirth.Value.ToShortDateString() : null,
                RepoProfile = resume.User.RepoProfile,
                Summary = HttpUtility.HtmlDecode(resume.User.Summary),
            };
        }
    }
}
