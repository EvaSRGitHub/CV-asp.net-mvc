using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Common.Services;
using CVApp.ViewModels;
using CVApp.ViewModels.PersonalInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVApp.Web.Controllers
{
    [Authorize]
    public class PersonalInfoController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;

        public PersonalInfoController(IPersonalInfoService personalInfoService)
        {
            this.personalInfoService = personalInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PersonalInfoViewModel model)
        {
           if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userName = this.User.Identity.Name;
                await this.personalInfoService.SaveFormData(model, userName);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }
            return RedirectToAction("Display", "Resume");
        }

        public async Task<IActionResult> Edit()
        {
            var currentUser = this.User.Identity.Name;

            PersonalInfoViewModel model;

            try
            {
                model = await this.personalInfoService.EditForm(currentUser);

                if(model == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }
           
            return this.View(model);
        }

       [HttpPost]
        public async Task<IActionResult> DeletePicture()
        {
            var userName = this.User.Identity.Name;

            try
            {
               await this.personalInfoService.DeletePicture(userName);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }

            return Json(new { Result = "OK" });
        }
    }
}
