using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVApp.Web.Controllers
{
    [Authorize]
    public class StartController : Controller
    {
        private readonly IStartService startService;

        public StartController(IStartService startService)
        {
            this.startService = startService;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            var userName = this.User.Identity.Name;

            var model = this.startService.GetStartInfoByUserName(userName);

            return View(model);
        }

    }
}
