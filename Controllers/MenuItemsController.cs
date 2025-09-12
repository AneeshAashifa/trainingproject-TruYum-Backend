using Microsoft.AspNetCore.Authorization;
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

       
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category)
        {
            var query = _db.MenuItems.AsNoTracking();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.ToLower() == category.ToLower());
            }

            var items = await query.ToListAsync();
            return Ok(items);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.MenuItems.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] MenuItemDto item)
        {
            if (item == null) return BadRequest();

            var menuItem = new MenuItem
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Active = item.Active,
                LaunchDate = DateTime.Now,
                ImageUrl = item.ImageUrl,
                Veg = item.Veg,
                Category = item.Category
            };

            _db.MenuItems.Add(menuItem);
            await _db.SaveChangesAsync();

            return Ok(new { message = "✅ Item added successfully", menuItem });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] MenuItemDto item)
        {
            if (item == null) return BadRequest();

            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();

            menuItem.Name = item.Name;
            menuItem.Description = item.Description;
            menuItem.Price = item.Price;
            menuItem.Active = item.Active;
            menuItem.ImageUrl = item.ImageUrl;
            menuItem.Veg = item.Veg;
            menuItem.Category = item.Category;

            await _db.SaveChangesAsync();
            return Ok(new { message = "✅ Item updated successfully", menuItem });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();

            _db.MenuItems.Remove(menuItem);
            await _db.SaveChangesAsync();

            return Ok(new { message = "🗑 Item deleted successfully" });
        }
    }

   
    public class MenuItemDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public DateTime LaunchDate { get; set; } 
        public bool Veg { get; set; } = true;
        public string Category { get; set; } = "";
        public string ImageUrl { get; set; } = "";
    }
}