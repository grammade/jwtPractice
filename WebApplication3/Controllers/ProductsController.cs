using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string host;

        public ProductsController(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            httpContextAccessor = httpContext;
        }

        private string getToken()
        {
            return HttpContext.Session.GetString("token");
        }

        // GET: Products
        public IActionResult Index()
        {
            List<Product> prods = new List<Product>();
            string token = getToken();

            try
            {
                string response = Utils.callGet(
                    getToken(),
                    httpContextAccessor,
                    "ProductsApi").Result;
                prods = JsonConvert.DeserializeObject<List<Product>>(response);
            }
            catch (Exception ex)
            {
                return Redirect("/home/error");
            }
            return View(prods);
        }

        public Product getProd(int? id)
        {
            Product prods;
            string token = getToken();
            string response = Utils.callGet(
                getToken(),
                httpContextAccessor,
                "ProductsApi/" + id).Result;

            try
            {
                prods = JsonConvert.DeserializeObject<Product>(response);
            }
            catch (Exception)
            {
                throw;
            }
            return prods;
        }

        public IActionResult updateView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = getProd(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Update(int id, [Bind("Id,Name,Type,Price,Qty")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            string token = getToken();
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(product),
                Encoding.UTF8,
                "application/json"
            );

            string response = Utils.callPost(
                getToken(),
                httpContextAccessor,
                "ProductsApi/update",
                content).Result;

            return View("details", product);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = getProd(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult createView()
        {
            return View();
        }

        public async Task<IActionResult> Create([Bind("Id,Name,Type,Price,Qty")] Product product)
        {
            string token = getToken();
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(product),
                Encoding.UTF8,
                "application/json"
            );

            string response = Utils.callPost(
                getToken(),
                httpContextAccessor,
                "ProductsApi/addProd",
                content).Result;

            return View("details", product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = getProd(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        public IActionResult DeleteConfirmed(int id)
        {
            string token = getToken();

            string response = Utils.callPost(
                getToken(),
                httpContextAccessor,
                "productsApi/deleteProd/"+id).Result;
            return Redirect("index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
