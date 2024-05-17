using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> Usermanager)
        {
            if(!Usermanager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Yousef",
                    Email = "temp@gmail.com",
                    UserName="YousefSaad",
                    address = new Address
                    {
                        FName ="Yousef",
                        LName = "Saad",
                        City = "Cairo",
                        Street="44",
                        ZipCode="234234",
                    }
                };

                 await Usermanager.CreateAsync(user,"Password123!");
            }

        }
    }
}
