using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PublisherController : Controller
    {
        private readonly IPlatformRepo _repo;
        private readonly DataContext _context;
        public PublisherController(IPlatformRepo repo, DataContext context)
        {
            _repo = repo;
            _context = context;
        }

        [Route("getAllGames")]
        [HttpGet]
        public ActionResult getAllGames()
        {
            return Ok(_repo.preuzmiSveIgrice());
        }

        [Route("vratiIgricu/{id}")]
        [HttpGet]
        public ActionResult vratiIgricu(string id)
        {
            try{
                var igra = _repo.preuzmiIgricu(id);

                return Ok(igra);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("getGames/{username}")]
        [HttpGet]
        public async Task<ActionResult> getGames(string username)
        {
            try{
                var user = await _context.Users.Where(p => p.Username == username).FirstOrDefaultAsync();

                var lista = user.Wishlist;

                var wishlist = lista.Split(';');

                var igrice = _repo.preuzmiOmiljeneIgrice(wishlist);

                return Ok(igrice);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("postojiIgrica/{id}")]//id:2
        [HttpGet]
        public ActionResult postojiIgrica(string id)
        {
            var username = User?.Identity?.Name;

            Console.WriteLine(username + "POSTOJIGRICA");

            var pom = _repo.postojiIgrica(id, username);
            
            return Ok(pom);
        }

        [Route("addToWishlist/{igrica}/{username}")]
        [HttpPost]
        public async Task<ActionResult> addToWishlist(string igrica, string username)
        {
            var user = await _context.Users.Where(p => p.Username == username).FirstOrDefaultAsync();

            if(user != null)
            {
                
                var igrice = _context.Users.Where(p => p.Username == username).Select(p => p.Wishlist).FirstOrDefault();
                
                if (igrice == null)
                {
                    user.Wishlist = igrica;
                    _context.Users.Update(user);

                }
                else if (!igrice.Contains(igrica))
                {
                    user.Wishlist += $";{igrica}";
                    _context.Users.Update(user);
                }
                else{
                    return StatusCode(201, "Vec postoji!");
                }

                _context.SaveChanges();

                return Ok("Igrica dodata");
            }

            return BadRequest();
        }

        
        [Route("removeFromWishlist/{igrica}/{username}")]
        [HttpPost]
        public async Task<ActionResult> removeFromWishlist(string igrica, string username)
        {
            var user = await _context.Users.Where(p => p.Username == username).FirstOrDefaultAsync();

            if(user != null)
            {
                
                var igrice = _context.Users.Where(p => p.Username == username).Select(p => p.Wishlist).FirstOrDefault();

                if(igrice == null)
                {
                    return BadRequest();
                }

                var listaIgrica = igrice.Split(';').ToList();

                if (listaIgrica.Contains(igrica))
                {
                    listaIgrica.Remove(igrica);

                    var novaWishlist = string.Join(";", listaIgrica);

                    user.Wishlist = novaWishlist;;

                    _context.Users.Update(user);
                }

                _context.SaveChanges();

                return Ok("Igrica uklonjena");
            }

            return BadRequest();
        }

    }
}