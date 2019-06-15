using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Models;
using CVApp.ViewModels.Work;
using Microsoft.EntityFrameworkCore;

namespace CVApp.Common.Services
{
    public class WorkService : IWorkService
    {
        private readonly IRepository<WorkExperience> workRepo;
        private readonly IRepository<CVAppUser> userRepo;
        private readonly ISanitizer sanitizer;

        public WorkService(IRepository<WorkExperience> workRepo, IRepository<CVAppUser> userRepo, ISanitizer sanitizer)
        {
            this.workRepo = workRepo;
            this.userRepo = userRepo;
            this.sanitizer = sanitizer;
        }

        public async Task Delete(WorkEditViewModel model)
        {
            if(model == null)
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
                //Log exeption
                throw new InvalidOperationException("Unable to delete your data. Try again, and if the problem persists contact site administrator.");
            }
        }

        public async Task<WorkEditViewModel> DeleteForm(int id, string userName)
        {
            var work = await this.workRepo.All().Include(w => w.Resume).ThenInclude(r => r.User).SingleOrDefaultAsync(w => w.Id == id);

            var user = work?.Resume.User.UserName;

            if(userName != user)
            {
                return null;
            }

            WorkEditViewModel model = null;

            if(work != null)
            {
                model = new WorkEditViewModel();
                model.Id = work.Id;
                model.InputVM = new WorkInputViewModel
                {
                    Company = work.Company,
                    Title = work.Title,
                    Description = work.Description,
                    FromYear = work.FromYear,
                    ToYear = work.ToYear.HasValue? work.ToYear.Value : (DateTime?)null,
                    Country = work.Country,
                    Region = work.Region,
                    City = work.City,
                    ResumeId = work.ResumeId
                };
            }

            return model;
        }

        public async Task<WorkEditViewModel> EditForm(int id, string userName)
        {
            var work = await this.workRepo.All().Include(w => w.Resume).ThenInclude(r => r.User).SingleOrDefaultAsync(w => w.Id == id);
            var user = work?.Resume.User.UserName;

            if (userName != user)
            {
                return null;
            }

            WorkEditViewModel model = null;

            if(work != null)
            {
                model = new WorkEditViewModel
                {
                    Id = work.Id,
                    InputVM = new WorkInputViewModel
                    {
                        Company = work.Company,
                        Title = work.Title,
                        Description = work.Description,
                        FromYear = work.FromYear,
                        ToYear = work.ToYear.HasValue? work.ToYear.Value : (DateTime?)null,
                        Country = work.Country,
                        Region = work.Region,
                        City = work.City,
                        ResumeId = work.ResumeId
                    }
                };
            }
            return model;
        }

        public async Task SaveFormData(WorkInputViewModel model, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new NullReferenceException("Invalid data");
            }

            if(model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var resumeId = this.userRepo.All().SingleOrDefault(u => u.UserName == userName)?.ResumeId;

            if(resumeId == null)
            {
                throw new InvalidOperationException("Something went wrong. Please contact site administrator for details.");
            }

            var work = new WorkExperience
            {
                Company = this.sanitizer.Sanitize(model.Company),
                Title = this.sanitizer.Sanitize(model.Title),
                Description = this.sanitizer.Sanitize(model.Description),
                FromYear = model.FromYear,
                ToYear = model.ToYear.HasValue? model.ToYear.Value : (DateTime?)null,
                Country = model.Country,
                Region = model.Region,
                City = model.City,
                ResumeId = resumeId.Value
            };

            await this.workRepo.AddAsync(work);

            try
            {
                await this.workRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //Log exeption
                throw new InvalidOperationException("Unable to save your data. Try again, and if the problem persists contact site administrator.");
            }
        }

        public async Task Update(WorkEditViewModel model)
        {
            if(model == null)
            {
                throw new NullReferenceException("Invalid data");
            }

            var work = new WorkExperience
            {
                Id = model.Id,
                Company = this.sanitizer.Sanitize(model.InputVM.Company),
                Title = this.sanitizer.Sanitize(model.InputVM.Title),
                Description = this.sanitizer.Sanitize(model.InputVM.Description),
                FromYear = model.InputVM.FromYear,
                ToYear = model.InputVM.ToYear,
                Country = model.InputVM.Country,
                Region = model.InputVM.Region,
                City = model.InputVM.City,
                ResumeId = model.InputVM.ResumeId
            };

            this.workRepo.Update(work);

            try
            {
                await this.workRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //Log exeption
                throw new InvalidOperationException("Unable to change your data. Try again, and if the problem persists contact site administrator.");
            }
        }
    }
}
