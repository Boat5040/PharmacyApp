namespace PharmacyApp.Migrations
{
    using PharmacyApp.DAL;
    using PharmacyApp.Helper;
    using PharmacyApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // prep security policy
            string policy = new SecurityPolicy
            {
                MaximumPasswordLength = 12,
                MinimumPasswordLength = 6,
                RequireDigit = true,
                RequireLowercase = true,
                RequireSpecialCharacter = true,
                RequireUppercase = true,
                SpecialCharacters = "@$%&?*"
            }.ToXmlString();


        }
    }
}
