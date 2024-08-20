using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IPlatformRepo _repo;
        private readonly DataContext _context;
        public UserController(IPlatformRepo repo, DataContext context)
        {
            _repo = repo;
            _context = context;
        }
        
        [Route("Login/{username}/{lozinka}")]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string lozinka)
        {
            
            var korisnik = await _context.Users.Where(p=>p.Username==username).FirstOrDefaultAsync();
            if(korisnik==null)
            {
                ViewBag.Error = "Email invalid!";
                return StatusCode(401, "Pogresan Username");
            }

            var korisnikPravi = await _context.Users.Where(p=>p.Username==username&&p.Password==lozinka).FirstOrDefaultAsync();
            if (korisnikPravi == null)
            {
                ViewBag.Error = "Password invalid!";
                return StatusCode(402, "Pogresna lozinka");
            }
            

            try
            {
                if (korisnikPravi.Username != null)
                {

                    Console.WriteLine(korisnikPravi.Username + korisnikPravi.Password);

                    var claims = new List<Claim>
                    {
                        new Claim("username", korisnikPravi.Username),
                        new Claim(ClaimTypes.NameIdentifier, korisnikPravi.Username),
                        new Claim(ClaimTypes.Name, korisnikPravi.Username),
                        new Claim(ClaimTypes.IsPersistent, "True")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                    return Ok();
                }
                else
                {
                    return StatusCode(403, "NULL Username");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        [HttpGet("Odradi")]
        public IActionResult odradi()
        {
            _repo.CreatePlatfrom();

            return Ok(User.Identity.Name);
        }
        
        
        [HttpGet("Odradi2")]
        public IActionResult odradi2()
        {
            _repo.unsubscribeAll();

            return Ok(User.Identity.Name);
        }

        [Route("Posalji")]
        [HttpGet]
        public ActionResult posaljiNotifikaciju()
        {
            var username = User?.Identity?.Name;

            Console.WriteLine(username + "nesto");

            _repo.SendSaleNotificationsAsync();
            
            return Ok(User.Identity.Name);
        }

        [HttpGet("LoginStranica")]
        public IActionResult LoginStranica()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                HttpContext.SignOutAsync();
            }
            return View("Login");
        }
        
        [HttpGet("PocetnaStranica")]
        public IActionResult PocetnaStranica()
        {
            return View("Pocetna");
        }

        [HttpGet("LogovanStranica")]
        public IActionResult LogovanStranica()
        {
            
            return View("Logovan");
        }
        
        [HttpGet("ProfilStranica")]
        public IActionResult ProfilStranica()
        {
            
            return View("Profil");
        }

        [HttpGet("Odjavi")]
        public async Task<IActionResult> Odjavi()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}