using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model;
using WebApplication3.Models;

namespace WebApplication3.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly AppDbContext dbContext;
            
        public ProductsApiController(AppDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IQueryable<Product> GetProducts([FromQuery] Filter filter)
        {
            IQueryable<Product> prods = dbContext.Products.AsNoTracking();
            //search
            if (filter.Keyword != null)
                prods = prods.Where(x => x.Name == filter.Keyword);
            //paging
            prods = prods.Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit);
            //order
            prods = prods.OrderBy(filter.OrderBy, filter.Order);

            return prods;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        [HttpPost("update")]
        public async Task<IActionResult> PostUpdate(Product product)
        {
            if (product == null)
            {
                return NotFound();
            }
            dbContext.Update(product);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("addProd")]
        public async Task<IActionResult> PostProduct(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("deleteProd/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var prod = await dbContext.Products.FindAsync(id);
            dbContext.Products.Remove(prod);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        private bool ProductExists(int id)
        {
            return dbContext.Products.Any(e => e.Id == id);
        }
    }
}
