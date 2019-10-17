using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Common.Services.Contracts;
using CVApp.Models;
using CVApp.ViewModels.Work;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static CVApp.ViewModels.Work.WorkViewModels;

namespace CVApp.Common.Services
{
    public class WorkService : IWorkService
    {
        private readonly IRepository<WorkExperience> workRepo;
        private readonly IRepository<CVAppUser> userRepo;
        private readonly IHttpContextAccessor accessor;
        //The Sanitizer remove the whole text between the script tags, together with the tags;
        private readonly ISanitizer sanitizer;
        private readonly int? resumeId;

        public WorkService(IRepository<WorkExperience> workRepo, IRepository<CVAppUser> userRepo, ISanitizer sanitizer, IHttpContextAccessor accessor)
        {
            this.workRepo = workRepo;
            this.userRepo = userRepo;
            this.sanitizer = sanitizer;
            this.accessor = accessor;
            this.resumeId = this.userRepo.All().SingleOrDefault(u => u.UserName == this.accessor.HttpContext.User.Identity.Name).ResumeId.GetValueOrDefault();
        }

        public async Task Delete(WorkEditViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var work = await this.workRepo.GetByIdAsync(model.Id);

            try
            {
                this.workRepo.Delete(work);
                await this.workRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to delete work model {model.Id} id.", e);
            }
        }

        public async Task<WorkEditViewModel> EditDeleteForm(int id, string userName)
        {
            var work = await this.workRepo.All().SingleOrDefaultAsync(w => w.Id == id);
            var user = await this.userRepo.All().SingleOrDefaultAsync(u => u.ResumeId == work.ResumeId);
            var currentUser = user.UserName;

            if (userName != currentUser)
            {
                return null;
            }

            WorkEditViewModel model = null;

            if (work != null)
            {
                model = new WorkEditViewModel
                {
                    Id = work.Id,
                    Company = work.Company,
                    Title = work.Title,
                    Description = work.Description,
                    StartDate = work.StartDate,
                    EndDate = work.EndDate.HasValue ? work.EndDate.Value : (DateTime?)null,
                    Country = work.Country,
                    Region = work.Region,
                    City = work.City,
                    ResumeId = work.ResumeId
                };
            }

            return model;
        }

        public async Task<IEnumerable<WorkOutViewModel>> GetWorkInfo(int resumeId)
        {
            if (this.resumeId != resumeId)
            {
                return null;
            }

            var workInfo = await this.workRepo.All().Where(e => e.ResumeId == this.resumeId).OrderByDescending(e => e.StartDate).ThenByDescending(e => e.EndDate).ToListAsync();

            if (workInfo.Count == 0)
            {
                return null;
            }

            var model = workInfo.Select(w => new WorkOutViewModel
            {
                Company = HttpUtility.HtmlDecode(w.Company),
                Title = HttpUtility.HtmlDecode(w.Title),
                StartDate = w.StartDate.ToString("MM/yyyy"),
                EndDate = w.EndDate.HasValue ? w.EndDate.Value.ToString("MM/yyyy") : "",
                Description = HttpUtility.HtmlDecode(w.Description),
                Country = w.Country,
                City = w.City,
                Id = w.Id
            }).ToList();

            return model;
        }

        public async Task<int> SaveFormData(WorkInputViewModel model, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new NullReferenceException($"{userName} is null or empty");
            }

            if (model == null)
            {
                throw new NullReferenceException("Work model is null");
            }

            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null for user {userName}.");
            }

            var work = new WorkExperience
            {
                Company = this.sanitizer.Sanitize(model.Company),
                Title = this.sanitizer.Sanitize(model.Title),
                Description = this.sanitizer.Sanitize(model.Description),
                StartDate = model.StartDate,
                EndDate = model.EndDate.HasValue ? model.EndDate.Value : (DateTime?)null,
                Country = model.Country,
                Region = model.Region,
                City = model.City,
                ResumeId = this.resumeId.Value
            };

            await this.workRepo.AddAsync(work);
            int id = default(int);

            try
            {
                await this.workRepo.SaveChangesAsync();
                id = work.Id;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            return id;
        }

        public async Task Update(WorkEditViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            if (this.resumeId == null)
            {
                throw new InvalidOperationException($"Resume id is null.");
            }

            var work = new WorkExperience
            {
                Id = model.Id,
                Company = this.sanitizer.Sanitize(model.Company),
                Title = this.sanitizer.Sanitize(model.Title),
                Description = this.sanitizer.Sanitize(model.Description),
                StartDate = model.StartDate,
                EndDate = model.EndDate.HasValue ? model.EndDate.Value : (DateTime?)null,
                Country = model.Country,
                Region = model.Region,
                City = model.City,
                ResumeId = this.resumeId.Value
            };

            this.workRepo.Update(work);

            try
            {
                await this.workRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
