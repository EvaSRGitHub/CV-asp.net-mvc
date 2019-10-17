using CVApp.Common.Services;
using CVApp.Common.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CVApp.ViewModels.Education.EducationViewModels;

namespace CVApp.Web.Controllers
{
    [Authorize]
    public class Education : Controller
    {
        private readonly IEducationService educationService;
        private readonly ILogger<Education> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly string userName;

        public Education(IEducationService educationService, ILogger<Education> logger, IHttpContextAccessor accessor)
        {
            this.educationService = educationService;
            this.logger = logger;
            this.accessor = accessor;
            this.userName = this.accessor.HttpContext.User.Identity.Name; 
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EducationInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            int id = default(int);
            try
            {
                id = await this.educationService.SaveFormData(model, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + $"#{id}");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                this.logger.LogDebug($"User {this.userName} tries to edit education info with negative {id} id.");
                return this.NotFound();
            }

            EducationEditViewModel model;

            try
            {
                model = await this.educationService.EditDeleteForm(id, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"User {this.userName} tries to edit someone else education info.");
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EducationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.educationService.Update(model);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + $"#{model.Id}");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                this.logger.LogDebug($"User {this.userName} tries to delete education info with negative {id} id.");
                return this.NotFound();
            }

            EducationEditViewModel model;

            try
            {
               model  = await this.educationService.EditDeleteForm(id, this.userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"User {this.userName} tries to delete null education model.");
                return this.NotFound();
            }
            
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EducationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.educationService.Delete(model);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + "#education");
        }

        public async Task<IActionResult> Display(int id)
        {
            IEnumerable<EducationOutViewModel> educInfo;

            try
            {
                educInfo = await this.educationService.GetEducationInfo(id);

                if (educInfo == null)
                {
                    this.logger.LogDebug($"Can't display education info for user {this.userName}.");
                    return this.NotFound();
                }
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.View(educInfo);
        }
    }
}
