using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public  class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                try
                {
                    var user = new AppUser
                    {
                        DisplayName = "Bob",
                        Email = "bob@gmail.com",
                        UserName= "bob@gmail.com",
                        Address = new Address
                        {
                            FirstName = "Bob",
                            LastName = "Bobbity",
                            Street = "10 the Street",
                            City = "NewYork",
                            State = "NY",
                            ZipCode = "90210"
                        }
                    };
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            };
           
        }
    }
}
