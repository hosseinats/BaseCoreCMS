using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CMS.DataLayer.Context;
using CMS.DTO;
using CMS.Entities;
using CMS.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signinManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(UserManager<User> userManager, SignInManager<User> signinManager, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnurl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (returnurl == null)
                {
                    return RedirectToAction("index", "home");
                }
                return Redirect(returnurl);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginVM, string returnurl = null)
        {

            if (ModelState.IsValid)
            {

                //is private part of project
                //related to token - jwt web api
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            var response = Request.Form["g-recaptcha-response"];
            var privateKey = _configuration["GoogleRecaptcha"];
            var captchaStatus = ReCaptcha.ReCaptchaPassed(response, privateKey);


            if (ModelState.IsValid && captchaStatus)
            {

                //is private part of project
                //related to token - jwt web api
                return Json(new { state = "Ok" });

            }
            else if (!captchaStatus)
            {
                ViewBag.CaptchaEmpty = "CaptchaEmpty";
                return Json(new { state = "CaptchaError" });
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignOut()
        {
            var user = User.Identity.IsAuthenticated ? await userManager.FindByNameAsync(User.Identity.Name) : null;
            await HttpContext.SignOutAsync();
            if (user != null)
            {
                await userManager.UpdateSecurityStampAsync(user);
            }
            return RedirectToAction("Index", "Home");
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}