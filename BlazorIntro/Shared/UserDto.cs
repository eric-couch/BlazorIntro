using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlazorIntro.Shared
{
    public class UserDto
    {
        public string Id { get; set; } = String.Empty;
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Movie> FavoriteMovies { get; set; }

        public List<IdentityRole> Roles { get; set; } = new();

    }
}
