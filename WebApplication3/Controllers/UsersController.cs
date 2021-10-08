using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication3.Model;

namespace WebApplication3.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UsersController(IHttpContextAccessor httpContext)
        {
            httpContextAccessor = httpContext;
        }

        private string getToken()
        {
            return HttpContext.Session.GetString("token");
        }

        public async Task<IActionResult> index()
        {
            List<User> users;           

            try
            {
                string token = getToken();
                string response = Utils.callGet(
                    getToken(),
                    httpContextAccessor,
                    "UsersApi/getAll").Result;
                users = JsonConvert.DeserializeObject<List<User>>(response);
            }
            catch (Exception)
            {
                return Redirect("/home/error");
            }
            return View(users);
        }

        public IActionResult create()
        {
            return View();
        }
    }
}
