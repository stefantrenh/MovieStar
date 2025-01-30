using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStar.Data.Identity;
using MovieStar.Domain.Entities;
using MovieStar.Domain.Enums;

namespace MovieStar.Data
{
    namespace MovieStar.Data
    {
        public class ApplicationDbContext : IdentityDbContext<AdminUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options) { }

            public DbSet<Customer> Customers { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<OrderProduct> OrderProducts { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                var adminRole = new IdentityRole("Admin") { Id = Guid.NewGuid().ToString() };

                var hasher = new PasswordHasher<AdminUser>();
                var adminUser = new AdminUser
                {
                    UserName = "Admin@MovieStar.com",
                    FullName = "Super User",
                    Email = "Admin@MovieStar.com"
                };
                adminUser.PasswordHash = hasher.HashPassword(adminUser, Environment.GetEnvironmentVariable("JWT_SECRET") ?? "Admin123");

                builder.Entity<AdminUser>().HasData(adminUser);
                builder.Entity<IdentityRole>().HasData(adminRole);

                builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = adminRole.Id });

                builder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Mega Man - DVD",
                        Price = 29,
                        Discount = false,
                        FormatType = VideoFormatType.DVD
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "One Piece - Blu-ray",
                        Price = 39,
                        Discount = false,
                        FormatType = VideoFormatType.BlueRay
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "One Piece - Blu-ray",
                        Price = 39m * 0.85m,
                        Discount = true,
                        FormatType = VideoFormatType.BlueRay
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Mega Man - DVD",
                        Price = 29m * 0.9m,
                        Discount = true,
                        FormatType = VideoFormatType.DVD
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Super Deal",
                        Price = 100,
                        Discount = true,
                        FormatType = VideoFormatType.Other
                    }
                    );



                builder.Entity<Order>()
                    .HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId);

                builder.Entity<OrderProduct>()
                    .HasKey(op => new { op.OrderId, op.ProductId });

                builder.Entity<OrderProduct>()
                    .HasOne(op => op.Order)
                    .WithMany(o => o.OrderProducts)
                    .HasForeignKey(op => op.OrderId);

                builder.Entity<OrderProduct>()
                    .HasOne(op => op.Product)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(op => op.ProductId);
            }
        }
    }
}
