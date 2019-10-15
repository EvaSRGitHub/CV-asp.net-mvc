using CVApp.Common.Repository;
using CVApp.Common.Services.Contracts;
using CVApp.Models;
using CVApp.ViewModels.Start;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public class StartService : IStartService
    {
        private readonly ILogger<StartService> logger;
        private readonly IRepository<Resume> resumeRepo;

        public StartService(ILogger<StartService> logger, IRepository<Resume> resumeRepo)
        {
            this.logger = logger;
            this.resumeRepo = resumeRepo;
        }

        public async Task<StartOutViewModel> GetStartInfoByUserName(string userName)
        {
            var resume = await this.resumeRepo.All().Include("User").SingleOrDefaultAsync(u => u.User.UserName == userName);
            
            if (resume == null)
            {
                this.logger.LogError("Current user resume is null.");
            }

            var model = new StartOutViewModel
            {
                IsPersonalInfoFilled = !string.IsNullOrEmpty(resume.User.FirstName),
                EducationRecords = resume.Education.Count(),
                WorkRecords = resume.Works.Count(),
                SkillRecords = resume.Skills.Count(),
                CertificateRecords = resume.Certificates.Count(),
                LanguageRecords = resume.Languages.Count()
            };

            return model;
        }
    }
}
