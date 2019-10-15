using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Common.Services.Contracts;
using CVApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CVApp.ViewModels.Skill.SkillViewModels;

namespace CVApp.Common.Services
{
    public class SkillService : ISkillService
    {
        private readonly IRepository<Skill> skillRepo;
        private readonly IRepository<CVAppUser> userRepo;
        private readonly IHttpContextAccessor accessor;
        //The Sanitizer remove the whole text between the script tags, together with the tags;
        private readonly ISanitizer sanitizer;
        private readonly int? resumeId;

        public SkillService(IRepository<Skill> skillRepo, IRepository<CVAppUser> userRepo, IHttpContextAccessor accessor, ISanitizer sanitizer)
        {
            this.skillRepo = skillRepo;
            this.userRepo = userRepo;
            this.accessor = accessor;
            this.sanitizer = sanitizer;
            this.resumeId = this.userRepo.All().SingleOrDefault(u => u.UserName == this.accessor.HttpContext.User.Identity.Name).ResumeId.GetValueOrDefault();
        }

        public async Task Delete(SkillEditViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var skill = await this.skillRepo.GetByIdAsync(model.Id);

            try
            {
                this.skillRepo.Delete(skill);
                await this.skillRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to delete skill {model.Id} id.", e);
            }
        }

        public async Task<SkillEditViewModel> EditDeleteForm(int id, string userName)
        {
            var skill = await this.skillRepo.All().SingleOrDefaultAsync(s => s.Id == id);
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == skill.ResumeId);
            var currentUser = user.UserName;

            if (userName != currentUser)
            {
                return null;
            }

            SkillEditViewModel model = null;

            if (skill != null)
            {
                model = new SkillEditViewModel
                {
                    Id = skill.Id,
                    Name = skill.Name,
                    ResumeId = skill.ResumeId
                };
            }

            return model;
        }

        public async Task<int> SaveFormData(SkillInputViewModel model, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new NullReferenceException($"{userName} is null or empty");
            }

            if (model == null)
            {
                throw new NullReferenceException("Skill model is null");
            }

            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null for user {userName}.");
            }

            var skill = new Skill
            {
                ResumeId = this.resumeId.Value,
                Name = this.sanitizer.Sanitize(model.Name),
            };

            await this.skillRepo.AddAsync(skill);
            int id = default(int);
            try
            {
                await this.skillRepo.SaveChangesAsync();
                id = skill.Id;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            return id;
        }

        public async Task Update(SkillEditViewModel model)
        {
            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null.");
            }

            var skill = new Skill
            {
                Id = model.Id,
                ResumeId = this.resumeId.Value,
                Name = this.sanitizer.Sanitize(model.Name),
            };

            this.skillRepo.Update(skill);

            try
            {
                await this.skillRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
