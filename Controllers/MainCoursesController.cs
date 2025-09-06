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
    public class MainCoursesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MainCoursesController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.MainCourses.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var item = await _db.MainCourses.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] MainCourseCreateDto dto)
        {
            var entity = new MainCourse
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Active = dto.Active,
                LaunchDate = dto.LaunchDate,
                ImageUrl = dto.ImageUrl,
                Veg = dto.Veg,
                Cuisine = dto.Cuisine,
                Size = dto.Size
            };
            _db.MainCourses.Add(entity);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] MainCourseCreateDto dto)
        {
            var entity = await _db.MainCourses.FindAsync(id);
            if (entity is null) return NotFound();
            entity.Name = dto.Name; entity.Description = dto.Description; entity.Price = dto.Price; entity.Active = dto.Active;
            entity.LaunchDate = dto.LaunchDate; entity.ImageUrl = dto.ImageUrl; entity.Veg = dto.Veg;
            entity.Cuisine = dto.Cuisine; entity.Size = dto.Size;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.MainCourses.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

