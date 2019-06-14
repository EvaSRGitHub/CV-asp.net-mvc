using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CVApp.Common.Services;
using CVApp.ViewModels.Education;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVApp.Web.Controllers
{
    [Authorize]
    public class Education : Controller
    {
        private readonly IEducationService educationService;

        public Education(IEducationService educationService)
        {
            this.educationService = educationService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EducationInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Education information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }

            try
            {
                await this.educationService.SaveFormData(model, this.User.Identity.Name);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

           return RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
            {
                return RedirectToAction("Home", "Index");
            }

            EducationEditViewModel model;

            try
            {
                model = await this.educationService.EditForm(id);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View("Error");
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EducationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Education information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }

            try
            {
                await this.educationService.Update(model);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

            return RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Home", "Index");
            }

            EducationEditViewModel model;

            try
            {
                model = await this.educationService.DeleteForm(id);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View("Error");
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EducationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Education information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }

            try
            {
                await this.educationService.Delete(model);
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
