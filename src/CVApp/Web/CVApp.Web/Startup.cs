using CVApp.Common.GeneratePDF;
using CVApp.Common.GeneratePDF.Contracts;
using CVApp.Common.IronPdfConverter;
using CVApp.Common.Repository;
using CVApp.Common.Sanitizer;
using CVApp.Common.Services;
using CVApp.Common.Services.Contracts;
using CVApp.Data;
using CVApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using IViewRenderService = CVApp.Common.GeneratePDF.Contracts.IViewRenderService;
using ViewRenderService = CVApp.Common.GeneratePDF.ViewRenderService;

namespace CVApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<CVAppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CVAppDbContextConnection"), sqlServerOptionsAction: sqlOptions =>
                     {
                        //enables resilient SQL connections that are retried if the connection fails.
                        sqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(30),
                         errorNumbersToAdd: null);
                     }));

            services.AddDefaultIdentity<CVAppUser>(options =>
            {
                //to override pass length should change minLength attribute in Pages: Register, Set, Change and ResetPass;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<CVAppDbContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ISanitizer, HtmlSanitizerAdapter>();
            services.AddScoped<IPersonalInfoService, PersonalInfoService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IStartService, StartService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IHtmlToPdfConverter, HtmlToPdfConverter>();
           // services.AddScoped<IIronHtmlToPdfConverter, IronHtmlToPdfConverter>();
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Start/Errors?code={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
