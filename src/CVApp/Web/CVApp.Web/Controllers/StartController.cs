using CVApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
