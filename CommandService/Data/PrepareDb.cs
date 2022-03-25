using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public static class PrepareDb
    {
        public static void Populate(this WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var ctx = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData(ctx, app.Environment.IsProduction());
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Unable to apply migrations: {ex.Message}");
                }
            }
        }
    }
}
