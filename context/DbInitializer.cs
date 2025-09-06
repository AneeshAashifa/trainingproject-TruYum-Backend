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

            if (!db.Beverages.Any() && !db.Starters.Any() && !db.MainCourses.Any() && !db.Snacks.Any())
            {
                db.Beverages.AddRange(
                    new Beverage { Name = "Cold Coffee", Description = "Refreshing iced coffee", Price = 149, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-3), ImageUrl = null, Veg = true, IsIced = true, VolumeMl = 350 },
                    new Beverage { Name = "Lemon Iced Tea", Description = "Zesty & chilled", Price = 129, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-2), Veg = true, IsIced = true, VolumeMl = 400 }
                );

                db.Starters.AddRange(
                    new Starter { Name = "Garlic Bread", Description = "Buttery garlic bread", Price = 99, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-10), Veg = true, Spicy = false, Serves = 1 },
                    new Starter { Name = "Chicken Wings", Description = "Spicy wings", Price = 249, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-8), Veg = false, Spicy = true, Serves = 1 }
                );

                db.MainCourses.AddRange(
                    new MainCourse { Name = "Margherita Pizza", Description = "Classic cheese pizza", Price = 299, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-12), Veg = true, Cuisine = "Italian", Size = "Medium" },
                    new MainCourse { Name = "Veggie Supreme Pizza", Description = "Loaded veggies", Price = 349, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-6), Veg = true, Cuisine = "Italian", Size = "Large" }
                );

                db.Snacks.AddRange(
                    new Snack { Name = "French Fries", Description = "Crispy fries", Price = 99, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-4), Veg = true, Baked = false },
                    new Snack { Name = "Nachos", Description = "Cheesy nachos", Price = 149, Active = true, LaunchDate = DateTime.UtcNow.AddDays(-1), Veg = true, Baked = true }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
