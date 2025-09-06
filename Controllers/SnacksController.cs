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
    public class SnacksController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SnacksController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Snacks.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var item = await _db.Snacks.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] SnackCreateDto dto)
        {
            var entity = new Snack
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Active = dto.Active,
                LaunchDate = dto.LaunchDate,
                ImageUrl = dto.ImageUrl,
                Veg = dto.Veg,
                Baked = dto.Baked
            };
            _db.Snacks.Add(entity);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] SnackCreateDto dto)
        {
            var entity = await _db.Snacks.FindAsync(id);
            if (entity is null) return NotFound();
            entity.Name = dto.Name; entity.Description = dto.Description; entity.Price = dto.Price; entity.Active = dto.Active;
            entity.LaunchDate = dto.LaunchDate; entity.ImageUrl = dto.ImageUrl; entity.Veg = dto.Veg;
            entity.Baked = dto.Baked;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Snacks.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}


