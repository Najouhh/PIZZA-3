using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizza.Data;
using Pizza.Data.Models.Entities;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Data
{
    public class PizzaContext : IdentityDbContext<ApplicationUser>
    {
        public PizzaContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This is essential for Identity

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderID, od.DishID });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(od => od.DishID);


        }
    }
}
