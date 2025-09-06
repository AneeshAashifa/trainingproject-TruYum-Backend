using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruYum.Api.Data;

namespace TruYum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MenuController(AppDbContext db) { _db = db; }

        // GET api/menu
        // Returns a flattened list of all items (with a synthetic Category field)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var beverages = await _db.Beverages.AsNoTracking().Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "Beverage" }).ToListAsync();
            var starters = await _db.Starters.AsNoTracking().Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "Starter" }).ToListAsync();
            var mains = await _db.MainCourses.AsNoTracking().Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "MainCourse" }).ToListAsync();
            var snacks = await _db.Snacks.AsNoTracking().Select(x => new { x.Id, x.Name, x.Description, x.Price, x.Active, x.LaunchDate, x.ImageUrl, x.Veg, Category = "Snack" }).ToListAsync();

            var all = beverages.Concat(starters).Concat(mains).Concat(snacks);
            return Ok(all);
        }

        // GET api/menu/category/{name}
        [HttpGet("category/{name}")]
        public async Task<IActionResult> GetByCategory(string name)
        {
            name = name.ToLowerInvariant();
            return name switch
            {
                "beverage" or "beverages" => Ok(await _db.Beverages.AsNoTracking().ToListAsync()),
                "starter" or "starters" => Ok(await _db.Starters.AsNoTracking().ToListAsync()),
                "main" or "maincourse" or "mains" => Ok(await _db.MainCourses.AsNoTracking().ToListAsync()),
                "snack" or "snacks" => Ok(await _db.Snacks.AsNoTracking().ToListAsync()),
                _ => NotFound("Unknown category")
            };
        }
    }
}
