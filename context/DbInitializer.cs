using TruYum.Api.Models;

namespace TruYum.Api.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            await db.Database.EnsureCreatedAsync();

            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User { Username = "admin", Password = "admin123", Role = "Admin" },
                    new User { Username = "user", Password = "user123", Role = "User" }
                );
                await db.SaveChangesAsync();
            }

          
            if (!db.MenuItems.Any())
            {
                db.MenuItems.AddRange(
                    // Beverages
                    new MenuItem { Name = "Cold Coffee", Description = "Refreshing iced coffee", Price = 149, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-3), Veg = true, Category = "Beverage", ImageUrl = "/assets/images/beverages.jpg" },
                    new MenuItem { Name = "Lemon Iced Tea", Description = "Zesty & chilled", Price = 129, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-2), Veg = true, Category = "Beverage", ImageUrl = "/assets/images/beverages.jpg" },

                    // Starters
                    new MenuItem { Name = "Garlic Bread", Description = "Buttery garlic bread", Price = 99, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-10), Veg = true, Category = "Starter", ImageUrl = "/assets/images/starters.jpg" },
                    new MenuItem { Name = "Chicken Wings", Description = "Spicy wings", Price = 249, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-8), Veg = false, Category = "Starter", ImageUrl = "/assets/images/starters.jpg" },

                    // Main Courses
                    new MenuItem { Name = "Margherita Pizza", Description = "Classic cheese pizza", Price = 299, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-12), Veg = true, Category = "MainCourse", ImageUrl = "/assets/images/maincourses.jpg" },
                    new MenuItem { Name = "Veggie Supreme Pizza", Description = "Loaded veggies", Price = 349, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-6), Veg = true, Category = "MainCourse", ImageUrl = "/assets/images/maincourses.jpg" },

                    // Snacks
                    new MenuItem { Name = "French Fries", Description = "Crispy fries", Price = 99, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-4), Veg = true, Category = "Snack", ImageUrl = "/assets/images/snacks.jpg" },
                    new MenuItem { Name = "Nachos", Description = "Cheesy nachos", Price = 149, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-1), Veg = true, Category = "Snack", ImageUrl = "/assets/images/snacks.jpg" }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}