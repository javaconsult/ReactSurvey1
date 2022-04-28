using ClassLibrary1.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Repository
{
    public class Seed
    {
        public static async Task SeedData(SurveyDbContext context, UserManager<AdminUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var users = new List<AdminUser>
                {
                    new AdminUser{Email="simondineen@gmail.com", UserName="sdineen", DisplayName="Simon"},
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }
    }
}
