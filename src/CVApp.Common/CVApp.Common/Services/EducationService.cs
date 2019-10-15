using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Common.Services.Contracts;
using CVApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CVApp.ViewModels.Education.EducationViewModels;

namespace CVApp.Common.Services
{
    public class EducationService : IEducationService
    {
        private readonly IRepository<Education> educationRepo;
        private readonly IRepository<CVAppUser> userRepo;
        private readonly IHttpContextAccessor accessor;
        //The Sanitizer remove the whole text between the script tags, together with the tags;
        private readonly ISanitizer sanitizer;
        private readonly int? resumeId;

        public EducationService(IRepository<Education> educationRepo, IRepository<CVAppUser> userRepo, ISanitizer sanitizer, IHttpContextAccessor accessor)
        {
            this.educationRepo = educationRepo;
            this.userRepo = userRepo;
            this.sanitizer = sanitizer;
            this.accessor = accessor;
            this.resumeId = this.userRepo.All().SingleOrDefault(u => u.UserName == this.accessor.HttpContext.User.Identity.Name).ResumeId.GetValueOrDefault();
        }

        public async Task Delete(EducationEditViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var educ = await this.educationRepo.GetByIdAsync(model.Id);

            try
            {
                this.educationRepo.Delete(educ);
                await this.educationRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to delete education model {model.Id} id.", e);
            }
        }

        public async Task<EducationEditViewModel> EditDeleteForm(int id, string userName)
        {
            var educ = await this.educationRepo.All().SingleOrDefaultAsync(e => e.Id == id);
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == educ.ResumeId);
            var currentUser = user.UserName;

            if (userName != currentUser)
            {
                return null;
            }

            EducationEditViewModel model = null;

            if (educ != null)
            {
                model = new EducationEditViewModel
                {
                    Id = educ.Id,
                    Institution = educ.Institution,
                    Diploma = educ.Diploma,
                    Country = educ.Country,
                    Region = educ.Region,
                    City = educ.City,
                    StartDate = educ.StartDate,
                    EndDate = educ.EndDate.HasValue ? educ.EndDate.Value : (DateTime?)null,
                    GPA = educ.GPA,
                    MainSubjects = educ.MainSubjects,
                    ResumeId = educ.ResumeId
                };
            }

            return model;
        }

        public async Task<int> SaveFormData(EducationInputViewModel model, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new NullReferenceException($"{userName} is null or empty");
            }

            if(model == null)
            {
                throw new NullReferenceException("Education model is null");
            }

            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null for user {userName}.");
            }

            var education = new Education
            {
                ResumeId = this.resumeId.Value,
                Institution = this.sanitizer.Sanitize(model.Institution),
                StartDate = model.StartDate,
                EndDate = model.EndDate.HasValue ? model.EndDate.Value : (DateTime?)null,
                GPA = model.GPA,
                MainSubjects = this.sanitizer.Sanitize(model.MainSubjects),
                Diploma = this.sanitizer.Sanitize(model.Diploma),
                City = model.City,
                Country = model.Country,
                Region = model.Region
            };

            await this.educationRepo.AddAsync(education);
            int id = default(int);
            try
            {
               await this.educationRepo.SaveChangesAsync();
                id = education.Id;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            return id;
        }

        public async Task Update(EducationEditViewModel model)
        {
            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null.");
            }

            var education = new Education
            {
                ResumeId = this.resumeId.Value,
                Institution = this.sanitizer.Sanitize(model.Institution),
                StartDate = model.StartDate,
                EndDate = model.EndDate.HasValue ? model.EndDate.Value : (DateTime?) null,
                GPA = model.GPA,
                MainSubjects = this.sanitizer.Sanitize(model.MainSubjects),
                Diploma = this.sanitizer.Sanitize(model.Diploma),
                City = model.City,
                Country = model.Country,
                Region = model.Region,
                Id = model.Id
            };

            this.educationRepo.Update(education);

            try
            {
                await this.educationRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
