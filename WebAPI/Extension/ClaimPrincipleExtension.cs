using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Security.Claims;

namespace WebAPI.Extension
{
    public static class ClaimPrincipleExtension
    {
        public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var userToReturn = await userManager.Users.FirstOrDefaultAsync(x => x.Email == user.GetEmail());

            if (userToReturn == null) throw new AuthenticationException("User Not Found");

            return userToReturn;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                throw new AuthenticationException("Email claim not found");
            }

            return email;
        }

        public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var userToReturn = await userManager.Users
                                                 .Include(x => x.Address)
                                                 .FirstOrDefaultAsync(x => x.Email == user.GetEmail());

            if (userToReturn == null) throw new AuthenticationException("User Not Found");

            return userToReturn;
        }
    }
}
