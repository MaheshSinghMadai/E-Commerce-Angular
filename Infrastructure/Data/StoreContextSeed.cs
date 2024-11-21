using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any(x => x.UserName == "admin@test.com"))
            {
                var user = new AppUser 
                { 
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(user, "P@$$w0rd");
                await userManager.AddToRoleAsync(user, "Admin");
            }
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.Products.Any())
                {
                    var productsData = await 
                        File.ReadAllTextAsync(path + @"/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    context.Products.AddRange(products);
                }

                if (!context.DeliveryMethods.Any())
                {
                    var dmData = await File.ReadAllTextAsync(path+ @"/Data/SeedData/delivery.json");
                    var delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    context.DeliveryMethods.AddRange(delivery);
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
