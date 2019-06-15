using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Common.Services;
using CVApp.ViewModels.Resume;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CVApp.Web.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResumeService resumeService;

        public ResumeController(IResumeService resumeService)
        {
            this.resumeService = resumeService;
        }

        public async Task<IActionResult> Display()
        {
            var userName = this.User.Identity.Name;

            ResumeDisplayViewModel model;

            try
            {
                model = await this.resumeService.DisplayResume(userName);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View("Error");
            }

            return this.View(model);
        }
    }
}
