using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BlazorIntro.Server.Models;
using BlazorIntro.Server.Data;
using BlazorIntro.Shared;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace BlazorIntro.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles ="Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly ApplicationDbContext _context;

    public AdminController( UserManager<ApplicationUser> _userManager,
                            RoleManager<IdentityRole> _roleManager,
                            ApplicationDbContext context)
    {
        userManager = _userManager;
        roleManager = _roleManager;
        _context = context;
    }

    [HttpGet]
    public async Task<List<UserDto>> Get()
    {
        var users = await _context.Users
                    .Include(m => m.FavoriteMovies)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        FavoriteMovies = u.FavoriteMovies
                    }).ToListAsync();
        //var users = await (from u in _context.Users
        //                   join ur in _context.UserRoles on u.Id equals ur.UserId
        //                   join r in _context.Roles on ur.RoleId equals r.Id
        //                   select new UserDto
        //                   {
        //                       Id = u.Id,
        //                       UserName = u.UserName,
        //                       FirstName = u.FirstName,
        //                       LastName = u.LastName,
        //                       r
        //                   }).Include(m => m.FavoriteMovies)
        //                   .ToListAsync();

        foreach (var user in users)
        {
            var iUser = await userManager.FindByIdAsync(user.Id);
            var roles = await userManager.GetRolesAsync(iUser);
            foreach (var role in roles)
            {
                IdentityRole? iRole = await roleManager.FindByNameAsync(role);
                if (iRole is not null)
                {
                    user.Roles.Add(iRole);
                }
            }
        }

        return users;
    }
}
