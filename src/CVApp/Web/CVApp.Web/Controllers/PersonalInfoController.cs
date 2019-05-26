using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Common.Services;
using CVApp.ViewModels;
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
        public async Task<IActionResult> Index(PersonalInfoViewModel model, string userName)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.personalInfoService.SaveFormData(model, userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewData["Error"] = e.Message;
                return this.View("Error");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Display()
        {
            var currentUser = this.User.Identity.Name;

            var model = this.personalInfoService.GetFormToEditOrDelete(currentUser);

            return this.View(model);
        }


    }
}
