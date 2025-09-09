using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruYum.Api.Data;
using TruYum.Api.Models;

namespace TruYum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MenuController(AppDbContext db) { _db = db; }

        // 🔹 GET api/menu
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var beverages = await _db.Beverages.AsNoTracking()
                .Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "Beverage" }).ToListAsync();
            var starters = await _db.Starters.AsNoTracking()
                .Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "Starter" }).ToListAsync();
            var mains = await _db.MainCourses.AsNoTracking()
                .Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "MainCourse" }).ToListAsync();
            var snacks = await _db.Snacks.AsNoTracking()
                .Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "Snack" }).ToListAsync();

            var all = beverages.Concat(starters).Concat(mains).Concat(snacks);
            return Ok(all);
        }

        // 🔹 GET api/menu/category/{name}
        [HttpGet("category/{name}")]
        public async Task<IActionResult> GetByCategory(string name)
        {
            name = name.ToLowerInvariant();
            return name switch
            {
                "beverage" or "beverages" => Ok(await _db.Beverages.ToListAsync()),
                "starter" or "starters" => Ok(await _db.Starters.ToListAsync()),
                "main" or "maincourse" or "mains" => Ok(await _db.MainCourses.ToListAsync()),
                "snack" or "snacks" => Ok(await _db.Snacks.ToListAsync()),
                _ => NotFound("Unknown category")
            };
        }

        // 🔹 POST api/menu (Add new item)
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] MenuItemDto item)
        {
            if (item == null) return BadRequest();

            switch (item.Category.ToLower())
            {
                case "beverage":
                    _db.Beverages.Add(new Beverage { Name = item.Name, Description = item.Description, Price = item.Price, Active = item.Active, LaunchDate = DateTime.Now, ImageUrl = item.ImageUrl, Veg = true });
                    break;
                case "starter":
                    _db.Starters.Add(new Starter { Name = item.Name, Description = item.Description, Price = item.Price, Active = item.Active, LaunchDate = DateTime.Now, ImageUrl = item.ImageUrl, Veg = true });
                    break;
                case "maincourse":
                    _db.MainCourses.Add(new MainCourse { Name = item.Name, Description = item.Description, Price = item.Price, Active = item.Active, LaunchDate = DateTime.Now, ImageUrl = item.ImageUrl, Veg = true });
                    break;
                case "snack":
                    _db.Snacks.Add(new Snack { Name = item.Name, Description = item.Description, Price = item.Price, Active = item.Active, LaunchDate = DateTime.Now, ImageUrl = item.ImageUrl, Veg = true });
                    break;
                default:
                    return BadRequest("Invalid category");
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "✅ Item added successfully" });
        }

        // 🔹 PUT api/menu/{category}/{id} (Update item)
        [HttpPut("{category}/{id}")]
        public async Task<IActionResult> UpdateItem(string category, int id, [FromBody] MenuItemDto item)
        {
            if (item == null) return BadRequest();

            switch (category.ToLower())
            {
                case "beverage":
                    var bev = await _db.Beverages.FindAsync(id);
                    if (bev == null) return NotFound();
                    bev.Name = item.Name;
                    bev.Description = item.Description;
                    bev.Price = item.Price;
                    bev.Active = item.Active;
                    bev.ImageUrl = item.ImageUrl;
                    break;

                case "starter":
                    var starter = await _db.Starters.FindAsync(id);
                    if (starter == null) return NotFound();
                    starter.Name = item.Name;
                    starter.Description = item.Description;
                    starter.Price = item.Price;
                    starter.Active = item.Active;
                    starter.ImageUrl = item.ImageUrl;
                    break;

                case "maincourse":
                    var main = await _db.MainCourses.FindAsync(id);
                    if (main == null) return NotFound();
                    main.Name = item.Name;
                    main.Description = item.Description;
                    main.Price = item.Price;
                    main.Active = item.Active;
                    main.ImageUrl = item.ImageUrl;
                    break;

                case "snack":
                    var snack = await _db.Snacks.FindAsync(id);
                    if (snack == null) return NotFound();
                    snack.Name = item.Name;
                    snack.Description = item.Description;
                    snack.Price = item.Price;
                    snack.Active = item.Active;
                    snack.ImageUrl = item.ImageUrl;
                    break;

                default:
                    return BadRequest("Invalid category");
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "✅ Item updated successfully" });
        }

        // 🔹 DELETE api/menu/{category}/{id}
        [HttpDelete("{category}/{id}")]
        public async Task<IActionResult> DeleteItem(string category, int id)
        {
            object? entity = category.ToLower() switch
            {
                "beverage" => await _db.Beverages.FindAsync(id),
                "starter" => await _db.Starters.FindAsync(id),
                "maincourse" => await _db.MainCourses.FindAsync(id),
                "snack" => await _db.Snacks.FindAsync(id),
                _ => null
            };

            if (entity == null) return NotFound();

            _db.Remove(entity);
            await _db.SaveChangesAsync();

            return Ok(new { message = "🗑️ Item deleted successfully" });
        }
    }

    // DTO (Data Transfer Object)
    public class MenuItemDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public string Category { get; set; } = "";
        public string ImageUrl { get; set; } = "";
    }
}
