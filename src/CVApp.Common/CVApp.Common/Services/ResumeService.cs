using CVApp.Common.Repository;
using CVApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    }
}
