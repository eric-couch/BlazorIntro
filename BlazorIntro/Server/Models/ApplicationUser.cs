using Microsoft.AspNetCore.Identity;
using BlazorIntro.Shared;

namespace BlazorIntro.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
    }
}