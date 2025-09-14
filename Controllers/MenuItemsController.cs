using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruYum.Api.Data;
using TruYum.Api.Dtos;
using TruYum.Api.Models;

namespace TruYum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MenuController(AppDbContext db) { _db = db; }

        // ✅ GET all menu items (with optional category filter)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
        {
            var query = _db.MenuItems
                .AsNoTracking()
                .Include(m => m.Category)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            var items = await query
                .Select(m => new MenuItemDto(
                    m.Id,
                    m.Name,
                    m.Description,
                    m.Price,
                    m.Active,
                    m.LaunchDate,
                    m.ImageUrl,
                    m.Veg,
                    m.CategoryId,
                    m.Category.Name
                ))
                .ToListAsync();

            return Ok(items);
        }

        // ✅ GET single item by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.MenuItems
                .Include(m => m.Category)
                .Where(m => m.Id == id)
                .Select(m => new MenuItemDto(
                    m.Id,
                    m.Name,
                    m.Description,
                    m.Price,
                    m.Active,
                    m.LaunchDate,
                    m.ImageUrl,
                    m.Veg,
                    m.CategoryId,
                    m.Category.Name
                ))
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // ✅ ADD new menu item
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] MenuItemCreateDto dto)
        {
            if (!await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId))
                return BadRequest("Invalid category id");

            var menuItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Active = dto.Active,
                LaunchDate = DateTime.Now,
                ImageUrl = dto.ImageUrl,
                Veg = dto.Veg,
                CategoryId = dto.CategoryId
            };

            _db.MenuItems.Add(menuItem);
            await _db.SaveChangesAsync();

            return Ok(new { message = "✅ Item added successfully", menuItem.Id });
        }

        // ✅ UPDATE existing menu item
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] MenuItemCreateDto dto)
        {
            var menuItem = await _db.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();

            if (!await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId))
                return BadRequest("Invalid category id");

            menuItem.Name = dto.Name;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;
            menuItem.Active = dto.Active;
            menuItem.ImageUrl = dto.ImageUrl;
            menuItem.Veg = dto.Veg;
            menuItem.CategoryId = dto.CategoryId;

            await _db.SaveChangesAsync();
            return Ok(new { message = "✏ Item updated successfully" });
        }

        // ✅ DELETE menu item
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
}