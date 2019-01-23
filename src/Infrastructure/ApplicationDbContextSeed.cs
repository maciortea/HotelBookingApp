using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            await EnsureUser(userManager, "test@domain.com", "P@ssword1");
        }

        private static async Task EnsureUser(UserManager<IdentityUser> userManager, string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser { UserName = userName, Email = userName };
                await userManager.CreateAsync(user, password);
            }
        }
    }
}
