using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Database;

namespace Shop.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    context.Database.EnsureCreated();

                    if (!context.Users.Any())
                    {
                        var adminUser = new IdentityUser
                        {
                            UserName = "Admin"
                        };

                        var managerUser = new IdentityUser
                        {
                            UserName = "Manager"
                        };

                        userManager.CreateAsync(adminUser, "pass").GetAwaiter().GetResult();
                        userManager.CreateAsync(managerUser, "pass").GetAwaiter().GetResult();

                        userManager.AddClaimAsync(adminUser, new Claim("Role", "Admin")).GetAwaiter().GetResult();
                        userManager.AddClaimAsync(managerUser, new Claim("Role", "Manager")).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
