using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruYum.Api.Data;
using TruYum.Api.Models;
using TruYum.models;

namespace TruYum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) { _db = db; }

        // ✅ GET all categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _db.Categories
                .Select(c => new {
                    c.Id,
                    c.Name
                })
       .ToListAsync();

            return Ok(categories);
        }

        // ✅ GET category by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // ✅ ADD new category
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Category name is required");

            // Prevent duplicates
            if (await _db.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower()))
                return BadRequest("Category already exists");

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return Ok(new { message = "✅ Category added successfully", category.Id });
        }

        // ✅ UPDATE existing category
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category updated)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();

            if (string.IsNullOrWhiteSpace(updated.Name))
                return BadRequest("Category name is required");

            // Prevent duplicates
            if (await _db.Categories.AnyAsync(c => c.Name.ToLower() == updated.Name.ToLower() && c.Id != id))
                return BadRequest("Category with same name already exists");

            category.Name = updated.Name;

            await _db.SaveChangesAsync();
            return Ok(new { message = "✏ Category updated successfully" });
        }

        // ✅ DELETE category
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.Include(c => c.MenuItems).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();

            if (category.MenuItems.Any())
                return BadRequest("❌ Cannot delete category that has menu items");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return Ok(new { message = "🗑 Category deleted successfully" });
        }
    }
}