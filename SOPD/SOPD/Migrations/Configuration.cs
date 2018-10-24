namespace SOPD.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SOPD.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SOPD.Models.ApplicationDbContext";
        }

        protected override void Seed(SOPD.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            /*        context.Roles.AddOrUpdate(
                    p => p.Name,
                    new IdentityRole { Name = "Dyplomant" },
                    new IdentityRole { Name = "Pracownik" },
                    new IdentityRole { Name = "Promotor" },
                    new IdentityRole { Name = "Recenzent" },
                    new IdentityRole { Name = "Dziekan" },
                    new IdentityRole { Name = "Administrator" }
                );
            */
            //
        }
    }
}
