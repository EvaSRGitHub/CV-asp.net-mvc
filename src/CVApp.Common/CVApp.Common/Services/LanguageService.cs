using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Models;
using CVApp.ViewModels.Language;
using Microsoft.EntityFrameworkCore;

namespace CVApp.Common.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> languageRepo;
        private readonly IRepository<Resume> resumeRepo;
        private readonly ISanitizer sanitizer;

        public LanguageService(IRepository<Language> languageRepo, IRepository<Resume> resumeRepo, ISanitizer sanitizer)
        {
            this.languageRepo = languageRepo;
            this.resumeRepo = resumeRepo;
            this.sanitizer = sanitizer;
        }

        public async Task Delete(LanguageEditViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var language = await this.languageRepo.GetByIdAsync(model.Id);

            try
            {
                this.languageRepo.Delete(language);
                await this.languageRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //Log exeption
                throw new InvalidOperationException("Unable to delete your data. Try again, and if the problem persists contact site administrator.");
            }
        }

        public async Task<LanguageEditViewModel> DeleteForm(int id, string userName)
        {
            var language = await this.languageRepo.All().Include(l => l.Resume).ThenInclude(r => r.User).SingleOrDefaultAsync(l => l.Id == id);

            var user = language?.Resume.User.UserName;

            if (userName != user)
            {
                return null;
            }

            LanguageEditViewModel model = null;

            if (language != null)
            {
                model = new LanguageEditViewModel
                {
                    Id = language.Id,
                    InputVM = new LanguageInputViewModel
                    {
                        Name = language.Name,
                        Level = language.Level,
                        ResumeId = language.ResumeId
                    }
                };
            }
            return model;
        }

        public async Task<LanguageEditViewModel> EditForm(int id, string userName)
        {
            var language = await this.languageRepo.All().Include(l => l.Resume).ThenInclude(r => r.User).SingleOrDefaultAsync(l => l.Id == id);
            var user = language?.Resume.User.UserName;

            if (userName != user)
            {
                return null;
            }

            LanguageEditViewModel model = null;

            if (language != null)
            {
                model = new LanguageEditViewModel
                {
                    Id = language.Id,
                    InputVM = new LanguageInputViewModel
                    {
                        Name = language.Name,
                        Level = language.Level,
                        ResumeId = language.ResumeId
                    }
                };
            }
            return model;
        }

        public async Task SaveFormData(LanguageInputViewModel model, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new NullReferenceException("Invalid data");
            }

            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var resume = await this.resumeRepo.All().Include("User").FirstOrDefaultAsync(u => u.User.UserName == userName);

            if (resume == null)
            {
                throw new InvalidOperationException("Something went wrong. Please contact site administrator for details.");
            }

            var language = new Language
            {
                Name = this.sanitizer.Sanitize(model.Name),
                Level = model.Level,
                ResumeId = resume.Id
            };

            await this.languageRepo.AddAsync(language);

            try
            {
                await this.languageRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //Log exeption
                throw new InvalidOperationException("Unable to save your data. Try again, and if the problem persists contact site administrator.");
            }
        }

        public async Task Update(LanguageEditViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var language = new Language
            {
                Id = model.Id,
                Name = this.sanitizer.Sanitize(model.InputVM.Name),
                Level = model.InputVM.Level,
                ResumeId = model.InputVM.ResumeId
            };

            try
            {
                this.languageRepo.Update(language);
                await this.languageRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //Logg e
                throw new InvalidOperationException("Unable to change your data. Try again, and if the problem persists contact site administrator.");
            }
        }
    }
}
