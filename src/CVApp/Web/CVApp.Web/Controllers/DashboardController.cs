using CVApp.Common.GeneratePDF.Contracts;
using CVApp.Common.IronPdfConverter;
using CVApp.Common.Services.Contracts;
using CVApp.Models;
using CVApp.ViewModels.Resume;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVApp.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Common.GeneratePDF.Contracts.IViewRenderService viewRenderService;
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IHostingEnvironment environment;
        private readonly IResumeService resumeService;

        public DashboardController(Common.GeneratePDF.Contracts.IViewRenderService viewRenderService, IHtmlToPdfConverter htmlToPdfConverter, IHostingEnvironment environment, IResumeService resumeService)
        {
            this.viewRenderService = viewRenderService;
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.environment = environment;
            this.resumeService = resumeService;
        }
        
        public async Task<IActionResult> GetPdf()
        {
            
            var model = await this.resumeService.DisplayResume(this.User.Identity.Name);
            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Resume/ResumeToPdf.cshtml", model);
            var basePath = this.environment.WebRootPath;
            var fileContents = this.htmlToPdfConverter.Convert(basePath, htmlData, Common.GeneratePDF.FormatType.A4, Common.GeneratePDF.OrientationType.Portrait);
            var result = this.File(fileContents, "application/pdf");
            return result;
        }

        //public async Task GetIronPdf()
        //{
        //    var model = await this.resumeService.DisplayResume(this.User.Identity.Name);
        //    var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Resume/ResumeToPdf.cshtml", model);
        //    var basePath = this.environment.WebRootPath;
        //    this.iron.GetHTMLPageAsPDF(basePath, htmlData);
        //}
    }
}
