using CVApp.Common.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var userName = this.User.Identity.Name;

            var model = await this.startService.GetStartInfoByUserName(userName);

            return this.View(model);
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
