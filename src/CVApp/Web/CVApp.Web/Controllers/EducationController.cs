using CVApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

            try
            {
                await this.educationService.SaveFormData(model, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + "#education");
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

            return RedirectToAction("Display", "Resume");
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

            return RedirectToAction("Display", "Resume");
        }
    }
}
