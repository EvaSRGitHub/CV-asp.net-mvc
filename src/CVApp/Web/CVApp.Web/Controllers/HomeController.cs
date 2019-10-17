using CVApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Net;

namespace CVApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Errors(string code)
        {
            HttpStatusCode sc;
            if (!Enum.IsDefined(typeof(HttpStatusCode), code))
            {
                sc = HttpStatusCode.NotFound;
            }
            else
            {
                sc = Enum.Parse<HttpStatusCode>(code, true);
            }

            this.ViewData["code"] = sc.ToString();
            this.ViewData["codeValue"] = (int)sc;

            return this.View();
        }
    }
}
