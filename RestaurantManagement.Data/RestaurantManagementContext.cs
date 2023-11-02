using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RestaurantManagement.Data
{
    public class RestaurantManagementContext: IdentityDbContext<ApplicationUser, IdentityRole, string> //inherit from Dbcontext
    {
        public RestaurantManagementContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet <Category> Categogy { get; set; }

        public DbSet <Cart> Cards { get; set; }

        public DbSet<CartDetail> CartDetail { get; set; }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuDetail> MenuDetail { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }

        public DbSet<UserToken> UserToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("ApplicationUserToken");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
        }
    }
}
