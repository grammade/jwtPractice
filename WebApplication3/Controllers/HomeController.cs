using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ChoETL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication3.Model;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            httpContextAccessor = httpContext;
        }

        public IActionResult Index()
        {
            var token = httpContextAccessor.HttpContext.Session.GetString("token");
            return View();
        }

        public IActionResult login(UserAuth user) 
        {
            string token = HttpContext.Session.GetString("token");
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(user),
                Encoding.UTF8,
                "application/json"
            );

            string response = Utils.callPost(
                token,
                httpContextAccessor,
                "UsersApi/auth",
                content).Result;
            if (response != null)
            {
                httpContextAccessor.HttpContext.Session.SetString("token", response);
                return Redirect("/products/index");
            }
            else
            {
                return Redirect("/home/index");
            }
        }

        public IActionResult register()
        {
            return Redirect("/Users/create");
        }

        public IActionResult swagger()
        {
            return Redirect("/swagger/index.html");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
