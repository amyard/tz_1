using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "delme",
                    Email = "delme@gmail.com",
                    UserName = "delme@gmail.com"
                };

                await userManager.CreateAsync(user, "Admin123*");
            }
        }
    }
}
