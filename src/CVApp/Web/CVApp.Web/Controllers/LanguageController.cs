using CVApp.Common.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static CVApp.ViewModels.Language.LanguageViewModels;

namespace CVApp.Web.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService languageService;
        private readonly ILogger<LanguageController> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly string userName;

        public LanguageController(ILanguageService languageService, ILogger<LanguageController> logger, IHttpContextAccessor accessor)
        {
            this.languageService = languageService;
            this.logger = logger;
            this.accessor = accessor;
            this.userName = this.accessor.HttpContext.User.Identity.Name;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LanguageInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            int id = default(int);

            try
            {
                id = await this.languageService.SaveFormData(model, this.userName);
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
                this.logger.LogDebug($"User {this.userName} tries to edit language with negative {id} id.");
                return this.NotFound();
            }

            LanguageEditViewModel model;

            try
            {
                model = await this.languageService.EditDeleteForm(id, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"User {this.userName} tries to edit someone else education info.");
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LanguageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.languageService.Update(model);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + $"#{model.Id}");
        }

        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0)
            {
                this.logger.LogDebug($"User {this.userName} tries to delete language with negative {id} id.");
                return this.NotFound();
            }

            LanguageEditViewModel model;

            try
            {
                model = await this.languageService.EditDeleteForm(id, this.userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"User {this.userName} tries to delete null language.");
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(LanguageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.languageService.Delete(model);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.Redirect(Url.RouteUrl(new { controller = "Resume", action = "Display" }) + "#languages");
        }
    }
}
