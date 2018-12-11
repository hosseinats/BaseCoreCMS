using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CMS.DataLayer.Context;
using CMS.Entities;
//using CMS.IDP.Settings;
//using CMS.IDP.ViewModels;
using CMS.ViewModel;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.Web.UI.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<User> userManager;


        public IdentityController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (returnUrl == null)
                {
                    return RedirectToAction("index", "home");
                }
                return Redirect(returnUrl);
            }

            return View(new LoginViewModel { returnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                //var httpClient = await _cmsHttpClient.GetHttpClientAsync();
                string url = "http://localhost:8000/account/login";
                HttpClient httpClient = new HttpClient();
                var serializedModelForCreation = JsonConvert.SerializeObject(loginVM);
                var getResponse = await httpClient.PostAsync(url, new StringContent(serializedModelForCreation, Encoding.UTF8, "application/json"));
                //IsSuccessStatusCode
                switch (getResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Json(new { state = "Ok", retUrl = loginVM.returnUrl });
                    case HttpStatusCode.BadRequest:
                        return Json(new { state = "Nok" });

                }
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
            //var privateKey = "6LenDnMUAAAAAMt0z9La7MUuIUACu4N3hpNavOeZ";
            var captchaStatus = ReCaptcha.ReCaptchaPassed(response, ReCaptcha.PrivateKey);

            if (ModelState.IsValid && captchaStatus)
            {

                var isUserNameExist = userManager.FindByNameAsync(registerVM.UserName).Result;
                if (isUserNameExist != null)
                {
                    return Json(new { state = "UserNameExist" });
                }
                var isMailExist = userManager.FindByEmailAsync(registerVM.Mail).Result;
                if (isMailExist != null)
                {
                    return Json(new { state = "MailExist" });
                }

                var user = new User
                {
                    FullName = registerVM.FullName,
                    UserName = registerVM.UserName,
                    Email = registerVM.Mail,
                    Gender = registerVM.Gender,
                    CreatedDateTime = DateTime.Now
                };
                var result = await userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    return Json(new { state = "Ok" });
                }
                else
                {
                    return Json(new { state = "NOk" });
                }
            }
            else if (!captchaStatus)
            {
                ViewBag.CaptchaEmpty = "CaptchaEmpty";
                return Json(new { state = "CaptchaError" });
            }
            return View();
        }


        //[HttpGet]
        //public async Task<IActionResult> Logout(string logoutId)
        //{
        //    // build a model so the logout page knows what to display
        //        var vm = await BuildLogoutViewModelAsync(logoutId);
        //        return await Logout(vm);

        //}

        ///// <summary>
        ///// Handle logout page postback
        ///// </summary>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout(LogoutInputModel model)
        //{
        //    // build a model so the logged out page knows what to display
        //    var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        //    if (User?.Identity.IsAuthenticated == true)
        //    {
        //        await signinManager.SignOutAsync();
        //    }

        //    if (vm.TriggerExternalSignout)
        //    {
        //        string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
        //        return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
        //    }

        //    return Redirect(vm.PostLogoutRedirectUri);
        //}
        //private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        //{
        //    var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        //    if (User?.Identity.IsAuthenticated != true)
        //    {
        //        vm.ShowLogoutPrompt = false;
        //        return vm;
        //    }

        //    var context = await _interaction.GetLogoutContextAsync(logoutId);
        //    if (context?.ShowSignoutPrompt == false)
        //    {
        //        vm.ShowLogoutPrompt = false;
        //        return vm;
        //    }
        //    return vm;
        //}
        //private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        //{
        //    var logout = await _interaction.GetLogoutContextAsync(logoutId);

        //    var vm = new LoggedOutViewModel
        //    {
        //        AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
        //        PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
        //        ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
        //        SignOutIframeUrl = logout?.SignOutIFrameUrl,
        //        LogoutId = logoutId
        //    };

        //    if (User?.Identity.IsAuthenticated == true)
        //    {
        //        var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
        //        if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
        //        {
        //            var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
        //            if (providerSupportsSignout)
        //            {
        //                if (vm.LogoutId == null)
        //                {
        //                    vm.LogoutId = await _interaction.CreateLogoutContextAsync();
        //                }

        //                vm.ExternalAuthenticationScheme = idp;
        //            }
        //        }
        //    }

        //    return vm;
        //}
    }
}