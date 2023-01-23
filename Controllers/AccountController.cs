using DatabaseLibrary.DBContext;
using DatabaseLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        LibraryContext db;
        public AccountController(LibraryContext db)
        {
            this.db = db;
        }
        
        [HttpPost("Register")]
        public async Task<IActionResult> Register( RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    user = new User { Login = model.Login, Password = model.Password };
                    Role? userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    return Ok(user);
                }
                else
                    return NotFound("This account is already created");
            }
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                User? user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return Ok(user);
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and(or) password");
                }
            }
            return  NotFound();          
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("you are logged out");
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,user.Role.Name),
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }
    }
}
