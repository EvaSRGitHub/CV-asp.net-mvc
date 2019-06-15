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
    }
}
