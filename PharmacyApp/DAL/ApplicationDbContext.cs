using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PharmacyApp.Models;

namespace PharmacyApp.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }
            // Make sure to call the base method first:

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("User").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim").Property(p => p.Id).HasColumnName("ClaimId");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role").Property(p => p.Id).HasColumnName("RoleId");
        }



        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<DrugCategory> DrugCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<TaxSetting> TaxSettings { get; set; }

    }
}