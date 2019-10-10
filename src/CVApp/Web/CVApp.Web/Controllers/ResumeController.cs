using CVApp.Common.Services;
using CVApp.ViewModels.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CVApp.Web.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResumeService resumeService;
        private readonly IHttpContextAccessor accessor;
        private readonly string userName;

        public ResumeController(IResumeService resumeService, IHttpContextAccessor accessor)
        {
            this.resumeService = resumeService;
            this.accessor = accessor;
            this.userName = this.accessor.HttpContext.User.Identity.Name;
        }

        public async Task<IActionResult> Display()
        {
            ResumeDisplayViewModel model;

            try
            {
                model = await this.resumeService.DisplayResume(this.userName);
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
