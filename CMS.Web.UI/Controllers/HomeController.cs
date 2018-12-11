using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CMS.DTO;
using Microsoft.AspNetCore.Mvc;
using CMS.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.Web.UI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _configuration;

        public HomeController( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(SiteMessageDTO siteMessageViewModel)
        {
            if (ModelState.IsValid)
            {
                //var httpClient = await _cmsHttpClient.GetHttpClientAsync();
                string url = "http://localhost:8002/api/SiteMessage";
                HttpClient httpClient = new HttpClient();
                var serializedModelForCreation = JsonConvert.SerializeObject(siteMessageViewModel);
                var getResponse = await httpClient.PostAsync(url, new StringContent(serializedModelForCreation , Encoding.UTF8 , "application/json" ) );
                if (getResponse.IsSuccessStatusCode)
                {
                    ViewBag.Msg = "Success";
                }
            }

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
        

    }
}
