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

    }
}
