using CVApp.Common.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static CVApp.ViewModels.Skill.SkillViewModels;

namespace CVApp.Web.Controllers
{
    public class SkillController : Controller
    {
        private readonly ISkillService skillService;
        private readonly ILogger<SkillController> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly string userName;

        public SkillController(ISkillService skillService, ILogger<SkillController> logger, IHttpContextAccessor accessor)
        {
            this.skillService = skillService;
            this.logger = logger;
            this.accessor = accessor;
            this.userName = this.accessor.HttpContext.User.Identity.Name;

        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SkillInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            int id = default(int);

            try
            {
                id = await this.skillService.SaveFormData(model, userName);
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
                this.logger.LogDebug($"User {this.userName} tries to edit skill negative {id} id.");
                return this.NotFound();
            }

            SkillEditViewModel model;

            try
            {
                model = await this.skillService.EditDeleteForm(id, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"User {this.userName} tries to edit someone else skill.");
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SkillEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.skillService.Update(model);
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
                this.logger.LogDebug($"User {this.userName} tries to delete skill with negative {id} id.");
                return this.NotFound();
            }

            SkillEditViewModel model;

            try
            {
                model = await this.skillService.EditDeleteForm(id, this.userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"User {this.userName} tries to delete null skill model.");
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SkillEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.skillService.Delete(model);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + "#skills");
        }

        public IActionResult EditDelete(int id)
        {
            this.ViewData["id"] = id;
            return this.View();
        }
    }
}
