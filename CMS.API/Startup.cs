using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Contracts;
using CMS.DataLayer.Context;
using CMS.DTO;
using CMS.Entities;
using CMS.Services;
using CMS.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace CMS.API
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

            services.AddDbContext<CMSContext>(c => c.UseSqlServer(Configuration.GetValue<string>("ConnectionString")));
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<CMSContext>();//.AddDefaultTokenProviders();
            services.AddTransient<ISiteMessageRepository, SiteMessageRepository>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(p => new MapperConfiguration(c =>
            {
                c.CreateMap<SiteMessage, SiteMessageDTO>();
                c.CreateMap<SiteMessageDTO, SiteMessage>();
                c.CreateMap<User, UserListDTO>();
            }));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.AddAuthentication(cfg =>
                {
                    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    }
                );


            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build(); });
                });


            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Swashbuckle.AspNetCore.Swagger.ApiKeyScheme()
                {
                    Description = "Authorization format : Bearer {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Resources  Api", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
