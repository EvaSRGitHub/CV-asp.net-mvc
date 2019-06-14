using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Models;
using CVApp.ViewModels.Education;
using Microsoft.EntityFrameworkCore;

namespace CVApp.Common.Services
{
    public class EducationService : IEducationService
    {
        private readonly IRepository<Education> educationRepo;
        private readonly IRepository<Resume> resumeRepo;
        private readonly ISanitizer sanitizer;

        public EducationService(IRepository<Education> educationRepo, IRepository<Resume> resumeRepo, ISanitizer sanitizer)
        {
            this.educationRepo = educationRepo;
            this.resumeRepo = resumeRepo;
            this.sanitizer = sanitizer;
        }

        public async Task Delete(EducationEditViewModel model)
        {
            var education = new Education
            {
                ResumeId = model.InputVM.ResumeId,
                Institution = this.sanitizer.Sanitize(model.InputVM.Institution),
                FromYear = model.InputVM.FromYear,
                ToYear = model.InputVM.ToYear,
                GPA = model.InputVM.GPA,
                MainSubjects = this.sanitizer.Sanitize(model.InputVM.MainSubjects),
                Diploma = this.sanitizer.Sanitize(model.InputVM.Diploma),
                City = model.InputVM.City,
                Country = model.InputVM.Country,
                Region = model.InputVM.Region,
                Id = model.Id
            };

            this.educationRepo.Delete(education);

            try
            {
                await this.educationRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task<EducationEditViewModel> DeleteForm(int id)
        {
            var educ = await this.educationRepo.GetByIdAsync(id);

            EducationEditViewModel model = new EducationEditViewModel();

            if (educ != null)
            {
                model.Id = educ.Id;
                model.InputVM = new EducationInputViewModel
                {
                    Institution = educ.Institution,
                    Diploma = educ.Diploma,
                    Country = educ.Country,
                    Region = educ.Region,
                    City = educ.City,
                    FromYear = educ.FromYear,
                    ToYear = educ.ToYear,
                    GPA = educ.GPA,
                    MainSubjects = educ.MainSubjects,
                    ResumeId = educ.ResumeId
                };
            }

            return model;
        }

        public async Task<EducationEditViewModel> EditForm(int id)
        {
            var educ = await this.educationRepo.GetByIdAsync(id);

            EducationEditViewModel model = new EducationEditViewModel();

            if (educ != null)
            {
                model.Id = educ.Id;
                model.InputVM = new EducationInputViewModel
                {
                    Institution = educ.Institution,
                    Diploma = educ.Diploma,
                    Country = educ.Country,
                    Region = educ.Region,
                    City = educ.City,
                    FromYear = educ.FromYear,
                    ToYear = educ.ToYear,
                    GPA = educ.GPA,
                    MainSubjects = educ.MainSubjects,
                    ResumeId = educ.ResumeId
                };
            }

            return model;
        }

        public async Task SaveFormData(EducationInputViewModel model, string userName)
        {
            var resumeId = this.resumeRepo.All().Include("User").SingleOrDefault(r => r.User.UserName == userName).Id;

            var education = new Education
            {
                ResumeId = resumeId,
                Institution = this.sanitizer.Sanitize(model.Institution),
                FromYear = model.FromYear,
                ToYear = model.ToYear,
                GPA = model.GPA,
                MainSubjects = this.sanitizer.Sanitize(model.MainSubjects),
                Diploma = this.sanitizer.Sanitize(model.Diploma),
                City = model.City,
                Country = model.Country,
                Region = model.Region
            };


            await this.educationRepo.AddAsync(education);

            try
            {
                await this.educationRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task Update(EducationEditViewModel model)
        {
            var education = new Education
            {
                ResumeId = model.InputVM.ResumeId,
                Institution = this.sanitizer.Sanitize(model.InputVM.Institution),
                FromYear = model.InputVM.FromYear,
                ToYear = model.InputVM.ToYear,
                GPA = model.InputVM.GPA,
                MainSubjects = this.sanitizer.Sanitize(model.InputVM.MainSubjects),
                Diploma = this.sanitizer.Sanitize(model.InputVM.Diploma),
                City = model.InputVM.City,
                Country = model.InputVM.Country,
                Region = model.InputVM.Region,
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
