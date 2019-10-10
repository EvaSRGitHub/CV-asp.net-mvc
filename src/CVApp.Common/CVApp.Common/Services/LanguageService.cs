using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Models;
using CVApp.ViewModels.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CVApp.ViewModels.Language.LanguageViewModels;

namespace CVApp.Common.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> languageRepo;
        private readonly IRepository<CVAppUser> userRepo;
        private readonly IHttpContextAccessor accessor;
        private readonly ISanitizer sanitizer;
        private readonly int? resumeId;

        public LanguageService(IRepository<Language> languageRepo, IRepository<CVAppUser> userRepo, ISanitizer sanitizer, IHttpContextAccessor accessor)
        {
            this.languageRepo = languageRepo;
            this.userRepo = userRepo;
            this.sanitizer = sanitizer;
            this.accessor = accessor;
            this.resumeId = this.userRepo.All().SingleOrDefault(u => u.UserName == this.accessor.HttpContext.User.Identity.Name).ResumeId.GetValueOrDefault();
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
                throw new InvalidOperationException($"Unable to delete language {model.Id} id.", e);
            }
        }

        public async Task<LanguageEditViewModel> EditDeleteForm(int id, string userName)
        {
            var language = await this.languageRepo.All().SingleOrDefaultAsync(e => e.Id == id);
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == language.ResumeId);
            var currentUser = user.UserName;

            if (userName != currentUser)
            {
                return null;
            }

            LanguageEditViewModel model = null;

            if (language != null)
            {
                model = new LanguageEditViewModel
                {
                    Id = language.Id,
                    Name = language.Name,
                    Level = language.Level,
                    ResumeId = language.ResumeId
                };
            }

            return model;
        }

        public async Task SaveFormData(LanguageInputViewModel model, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new NullReferenceException($"{userName} is null or empty");
            }

            if (model == null)
            {
                throw new NullReferenceException("Language model is null");
            }

            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null for user {userName}.");
            }

            var language = new Language
            {
                Name = this.sanitizer.Sanitize(model.Name),
                Level = model.Level,
                ResumeId = this.resumeId.Value
            };

            await this.languageRepo.AddAsync(language);

            try
            {
                await this.languageRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task Update(LanguageEditViewModel model)
        {
            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null.");
            }

            var language = new Language
            {
                Id = model.Id,
                Name = this.sanitizer.Sanitize(model.Name),
                Level = model.Level,
                ResumeId = this.resumeId.Value
            };

            try
            {
                this.languageRepo.Update(language);
                await this.languageRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
