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
    }
}
