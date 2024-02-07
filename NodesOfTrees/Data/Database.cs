using Microsoft.EntityFrameworkCore;
using System;

namespace NodesOfTrees.Data
{
    public class Database
    {
        public static void Migrate(WebApplication app)
        {
            using (var container = app.Services.CreateScope())
            {
                var dbContext = container.ServiceProvider.GetService<DataContext>();
                var pendingMigration = dbContext!.Database.GetPendingMigrations();
                if (pendingMigration.Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
