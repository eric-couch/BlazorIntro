using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlazorIntro.Server.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BlazorIntro.Server.Models;
using BlazorIntro.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorIntro.Server.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/get-movies")]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            var movies = await _context.Users
                            .Include(m => m.FavoriteMovies)
                            .Select(u => new UserDto
                            {
                                Id = u.Id,
                                UserName = u.UserName!,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                FavoriteMovies = u.FavoriteMovies
                            })
                            .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }

        [HttpPost]
        [Route("api/add-movie")]
        public async Task<ActionResult> AddMovie([FromBody] Movie movie)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
            {
                return NotFound();
            }
            
            user.FavoriteMovies.Add(movie);
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPost]
        [Route("api/remove-movie")]
        public async Task<ActionResult> RemoveMovie([FromBody] Movie movie)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
            {
                return NotFound();
            }

            var movieToRemove = _context.Movies.FirstOrDefault(m => m.imdbId == movie.imdbId);
            user.FavoriteMovies.Remove(movieToRemove);
            _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/get-admin-status")]
        public async Task<IActionResult> GetAdminStatus() {
            var currentUser = await _userManager.GetUserAsync(User);

            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            if (isAdmin) {
                return Ok("You are an admin");
            } else
            {
                return Unauthorized("You are not authroized to access this resource.");
            }
        }
        
    }
}
