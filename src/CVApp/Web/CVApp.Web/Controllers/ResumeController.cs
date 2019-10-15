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

        public async Task<IActionResult> Display()
        {
            ResumeDisplayViewModel model;

            try
            {
                model = await this.resumeService.DisplayResume(this.userName);
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
                await this.resumeService.Delete(id, userName);
            
            }
            catch (Exception e)
            {

                this.logger.LogDebug(e, $"An exception happened for user {this.userName}");
                return this.BadRequest();
            }

            return this.RedirectToAction("Display", "Resume");
        }


        public async Task<IActionResult> ResumeToPDF()
        {
            var model = await this.resumeService.DisplayResume(this.User.Identity.Name);
            return this.View(model);
        }
    }
}
