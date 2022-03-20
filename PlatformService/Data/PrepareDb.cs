using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepareDb
    {
        public static void Populate(this WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var ctx = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData(ctx);
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data...");
                context.Platforms.AddRange(
                    new Platform { Name = "DotNet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Colud Native Computing Foundation", Cost = "Free" }
                    );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Data exist already");
            }
        }
    }
}
