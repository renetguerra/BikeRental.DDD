using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BikeRental.DDD.Infrastructure
{
    /// <summary>
    /// Clase que se encarga la inserción de los datos, si la tabla User está vacía.
    /// </summary>
    public class Seed
    {
        public static async Task ClearConnections(DataContext context)
        {
            context.Connections.RemoveRange(context.Connections);
            await context.SaveChangesAsync();
        }

        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var httpClient = new HttpClient();
            if (!userManager.Users.Any())
            {
                await CreateRoles(roleManager);
                await SeedCustomers(userManager);
                await SeedAdmins(userManager);                
            }
        }

        private static async Task CreateRoles(RoleManager<Role> roleManager)
        {
            var roles = new List<Role>
            {
                new Role { Name = "Customer" },
                new Role { Name = "Admin" },
                new Role { Name = "Moderator" },
                new Role { Name = "VIP" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task SeedCustomers(UserManager<User> userManager)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "BikeRental.DDD.Infrastructure", "Data", "UserSeedData.json");
            var userData = File.ReadAllText(path);
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                user.UserName = user.KnownAs!.ToLower();
                user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
                user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);

                await userManager.CreateAsync(user, "Pa$$w0rd");                
                await userManager.AddToRoleAsync(user, "Customer");
            }
        }

        private static async Task SeedAdmins(UserManager<User> userManager)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "BikeRental.DDD.Infrastructure", "Data", "UserAdminSeedData.json");            
            var adminData = File.ReadAllText(path);
            var admins = JsonConvert.DeserializeObject<List<User>>(adminData);

            foreach (var admin in admins)
            {
                admin.UserName = admin.KnownAs!.ToLower();
                admin.Created = DateTime.SpecifyKind(admin.Created, DateTimeKind.Utc);
                admin.LastActive = DateTime.SpecifyKind(admin.LastActive, DateTimeKind.Utc);

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Admin" });
            }
        }

        public static async Task SeedBikes(DataContext context)
        {            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "BikeRental.DDD.Infrastructure", "Data", "BikeSeedData.json");
            var bikeData = File.ReadAllText(path);
            var bikes = JsonConvert.DeserializeObject<List<Bike>>(bikeData);

            if (bikes != null && !context.Bikes.Any())
            {
                context.Bikes.AddRange(bikes);
                await context.SaveChangesAsync();
            }
        }
    }
}
