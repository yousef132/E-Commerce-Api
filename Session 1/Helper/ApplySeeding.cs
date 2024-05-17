using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Data.StoreDbContext;
using Store.Repository;

namespace Session_1.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();
                    var manager = services.GetRequiredService<UserManager<AppUser>>();
                    // apply any pending migrations when run
                    await context.Database.MigrateAsync();
                    await StoreContextSeeding.SeedAsync(context, loggerFactory);
                    await AppIdentityContextSeed.SeedUserAsync(manager);

				}
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<ApplySeeding>();
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
