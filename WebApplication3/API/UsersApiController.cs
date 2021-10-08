using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model;
using WebApplication3.Models;

namespace WebApplication3.API
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IJWTManager jwtManager;

        public UsersApiController(IJWTManager jwtManager, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.jwtManager = jwtManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public IActionResult auth([FromBody] UserAuth user)
        {
            var token = jwtManager.Authenticate(user.Username, user.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }

        [HttpGet]
        [Route("getAll")]
        public IQueryable<User> getAll()
        {
            return dbContext.Users;
        }

        [HttpGet]
        [Route("getUser/{id}")]
        public User getUser(int id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        [Route("addUser")]
        public IActionResult addUser(
            [FromBody] User user)
        {
            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("addUser123")]
        public IActionResult addUser(
            string name,
            string username,
            string password,
            string role)
        {
            User user = new User();
            user.Name = name;
            user.Username = username;
            user.Password = password;
            user.Role = role;

            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
