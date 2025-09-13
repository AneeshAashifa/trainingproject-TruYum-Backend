using TruYum.Api.Models;
using TruYum.models;

namespace TruYum.Api.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            await db.Database.EnsureCreatedAsync();

            // 🔹 Seed Users
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User { Username = "admin", Password = "admin123", Role = "Admin" },
                    new User { Username = "user", Password = "user123", Role = "User" }
                );
                await db.SaveChangesAsync();
            }

            // 🔹 Seed Categories
            if (!db.Categories.Any())
            {
                db.Categories.AddRange(
                    new Category { Name = "Beverage" },
                    new Category { Name = "Starter" },
                    new Category { Name = "MainCourse" },
                    new Category { Name = "Snack" }
                );
                await db.SaveChangesAsync();
            }

            // Get category IDs
            var beverage = db.Categories.First(c => c.Name == "Beverage");
            var starter = db.Categories.First(c => c.Name == "Starter");
            var mainCourse = db.Categories.First(c => c.Name == "MainCourse");
            var snack = db.Categories.First(c => c.Name == "Snack");

            // 🔹 Seed MenuItems
            if (!db.MenuItems.Any())
            {
                db.MenuItems.AddRange(
                    // Beverages
                    new MenuItem { Name = "Cold Coffee", Description = "Refreshing iced coffee", Price = 149, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-3), Veg = true, ImageUrl = "/assets/images/beverages.jpg", CategoryId = beverage.Id },
                    new MenuItem { Name = "Lemon Iced Tea", Description = "Zesty & chilled", Price = 129, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-2), Veg = true, ImageUrl = "/assets/images/beverages.jpg", CategoryId = beverage.Id },

                    // Starters
                    new MenuItem { Name = "Garlic Bread", Description = "Buttery garlic bread", Price = 99, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-10), Veg = true, ImageUrl = "/assets/images/starters.jpg", CategoryId = starter.Id },
                    new MenuItem { Name = "Chicken Wings", Description = "Spicy wings", Price = 249, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-8), Veg = false, ImageUrl = "/assets/images/starters.jpg", CategoryId = starter.Id },

                    // Main Courses
                    new MenuItem { Name = "Margherita Pizza", Description = "Classic cheese pizza", Price = 299, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-12), Veg = true, ImageUrl = "/assets/images/maincourses.jpg", CategoryId = mainCourse.Id },
                    new MenuItem { Name = "Veggie Supreme Pizza", Description = "Loaded veggies", Price = 349, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-6), Veg = true, ImageUrl = "/assets/images/maincourses.jpg", CategoryId = mainCourse.Id },

                    // Snacks
                    new MenuItem { Name = "French Fries", Description = "Crispy fries", Price = 99, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-4), Veg = true, ImageUrl = "/assets/images/snacks.jpg", CategoryId = snack.Id },
                    new MenuItem { Name = "Nachos", Description = "Cheesy nachos", Price = 149, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-1), Veg = true, ImageUrl = "/assets/images/snacks.jpg", CategoryId = snack.Id }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}