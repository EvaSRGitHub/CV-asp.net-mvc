using CVApp.Common.Services.Contracts;
using CVApp.ViewModels.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CVApp.Web.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResumeService resumeService;
        private readonly ILogger<ResumeController> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly string userName;

        public ResumeController(IResumeService resumeService, ILogger<ResumeController> logger, IHttpContextAccessor accessor)
        {
            this.resumeService = resumeService;
            this.logger = logger;
            this.accessor = accessor;
            this.userName = this.accessor.HttpContext.User.Identity.Name;
        }

        public IActionResult Start()
        {
            var userName = this.User.Identity.Name;

            var model = this.resumeService.GetStartInfo();

            return this.View(model);
        }

        public IActionResult Display()
        {
            ResumeDisplayViewModel model;

            try
            {
                model = this.resumeService.DisplayResume();
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.resumeService.Delete(id);
            
            }
            catch (Exception e)
            {

                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.RedirectToAction("Display", "Resume");
        }


        public IActionResult ResumeToPDF()
        {
            var model = this.resumeService.DisplayResume();
            return this.View(model);
        }
    }
}
