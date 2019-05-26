using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVApp.Common.Repository;
using CVApp.Models;
using CVApp.ViewModels.Start;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public StartOutViewModel GetStartInfoByUserName(string userName)
        {
            var resume = this.resumeRepo.All().Include("User").SingleOrDefault(u => u.User.UserName == userName);
            
            if (resume == null)
            {
                this.logger.LogError("Current user resume is null.");
                //ErrorPage
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
