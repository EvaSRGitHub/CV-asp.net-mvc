using CVApp.Common.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static CVApp.ViewModels.PersonalInfo.PersonalInfoViewModels;

namespace CVApp.Web.Controllers
{
    [Authorize]
    public class PersonalInfoController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly ILogger<PersonalInfoController> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly string userName;

        public PersonalInfoController(IPersonalInfoService personalInfoService, ILogger<PersonalInfoController> logger, IHttpContextAccessor accessor)
        {
            this.personalInfoService = personalInfoService;
            this.logger = logger;
            this.accessor = accessor;
            this.userName = this.accessor.HttpContext.User.Identity.Name;
        }

        public async Task<IActionResult> Index()
        {
            if (await this.personalInfoService.HasPersonalInfoFormFilled(userName))
            {
                return this.RedirectToAction("Display", "Resume");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PersonalInfoInputViewModel model)
        {
           if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.personalInfoService.SaveFormData(model, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"Exception happened for user {userName}");
                return this.BadRequest();
            }

            return this.RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult> Edit()
        {
            PersonalInfoEditViewModel model;

            try
            {
                model = await this.personalInfoService.EditForm(userName);

                if (model == null)
                {
                    this.logger.LogDebug($"User {userName} tries to edit someone else personal model.");
                    return this.NotFound();
                }
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $" An exception happened for user {userName}");
                return this.BadRequest();
            }
           
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonalInfoEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.personalInfoService.SaveFormData(model, userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $" An exception happened for user {userName}");
                return this.BadRequest();
            }

            return this.RedirectToAction("Display", "Resume");
        }

       [HttpPost]
        public async Task<IActionResult> DeletePicture()
        {
            try
            {
               await this.personalInfoService.DeletePicture(userName);
            }
            catch (Exception e)
            {
                this.logger.LogDebug(e, $"An exception happened for user {userName}");
                return this.BadRequest();
            }

            return this.Json(new { Result = "OK" });
        }
    }
}
