using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Common.Services;
using CVApp.ViewModels.Language;
using Microsoft.AspNetCore.Mvc;

namespace CVApp.Web.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService languageService;

        public LanguageController(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LanguageInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Language information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }
            var username = this.User.Identity.Name;

            try
            {
                await this.languageService.SaveFormData(model, username);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

            return RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            if (Id <= 0)
            {
                return NotFound();
            }


            var userName = this.User.Identity.Name;

            LanguageEditViewModel model = await this.languageService.EditForm(Id, userName);

            if (model == null)
            {
                return NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LanguageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Language", new { Id = model.Id });
            }

            try
            {
                await this.languageService.Update(model);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

            return RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult>Delete(int Id)
        {
            if (Id <= 0)
            {
                return NotFound();
            }

            var userName = this.User.Identity.Name;

            LanguageEditViewModel model = await this.languageService.DeleteForm(Id, userName);

            if (model == null)
            {
                return NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(LanguageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Delete", "Language", new { Id = model.Id });
            }

            try
            {
                await this.languageService.Delete(model);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

            return RedirectToAction("Display", "Resume");
        }
    }
}
