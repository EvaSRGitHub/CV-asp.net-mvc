using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Common.Services;
using CVApp.ViewModels.Work;
using Microsoft.AspNetCore.Mvc;

namespace CVApp.Web.Controllers
{
    public class WorkController : Controller
    {
        private readonly IWorkService workService;

        public WorkController(IWorkService workService)
        {
            this.workService = workService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(WorkInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Work information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }

            var username = this.User.Identity.Name;

            try
            {
                await this.workService.SaveFormData(model, username);
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
            if(Id <= 0)
            {
                return NotFound();
            }

            var userName = this.User.Identity.Name;

            WorkEditViewModel model = await this.workService.EditForm(Id, userName);

            if(model == null)
            {
                return NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Work information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }

            try
            {
                await this.workService.Update(model);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

            return RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            var userName = this.User.Identity.Name;

            WorkEditViewModel model = await this.workService.DeleteForm(Id, userName);

            if(model == null)
            {
                return NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WorkEditViewModel model)
        {
            try
            {
                await this.workService.Delete(model);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View("Error");
            }

            return RedirectToAction("Display", "Resume");
        }
    }
}
