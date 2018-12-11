using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Contracts;
using CMS.DataLayer.Context;
using CMS.Entities;
using CMS.Entities.Contracts;
using CMS.Services;
using CMS.Web.UI.CoreClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CMS.Web.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<CMSContext>(c => c.UseSqlServer(Configuration.GetValue<string>("ConnectionString")));
            services.AddIdentityCore<User>().AddEntityFrameworkStores<CMSContext>().AddSignInManager<SignInManager<User>>().AddUserManager<UserManager<User>>();
            services.AddTransient<ISiteMessageRepository, SiteMessageRepository>();
            //services.AddSingleton(p => new MapperConfiguration(c =>
            //{
            //    c.CreateMap<Category, CategoryViewModel>().ForMember(d => d.ParentCategoryTitle, m => m.MapFrom(f => f.ParentCategory.Title));
            //}));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddHttpClient("APIBase", client => { client.BaseAddress = new Uri("http://localhost:8002"); });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie( option =>
                {
                    option.LoginPath = "/Account/login";
                    option.LogoutPath = "/Account/SignOut";
                    option.Cookie.Name = "SpecialCookie";
                });
            //services.AddAuthentication("Identity.External").AddCookie("Identity.External").AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //});

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
