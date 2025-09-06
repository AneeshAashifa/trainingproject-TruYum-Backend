using Microsoft.EntityFrameworkCore;
using TruYum.Api.Models;

namespace TruYum.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Base + derived sets
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Beverage> Beverages => Set<Beverage>();
        public DbSet<Starter> Starters => Set<Starter>();
        public DbSet<MainCourse> MainCourses => Set<MainCourse>();
        public DbSet<Snack> Snacks => Set<Snack>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<User> Users => Set<User>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Use TPT (Table per Type) mapping – each derived type has its own table
            modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
            modelBuilder.Entity<Beverage>().ToTable("Beverages");
            modelBuilder.Entity<Starter>().ToTable("Starters");
            modelBuilder.Entity<MainCourse>().ToTable("MainCourses");
            modelBuilder.Entity<Snack>().ToTable("Snacks");

            // Cart -> Items cascade
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart!)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed minimal data via separate initializer
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order!)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
