using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.ViewModels.Education;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CVApp.Web.Controllers
{
    [Authorize]
    public class Education : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(EducationCollectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "An error occurred with your Education information. Please pay attention to the data needed and submit the form again.";

                return View("Error");
            }
            //TODO - rediredt to Resume/Display
           return Redirect("/");
        }
             
    }
}
