using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TruYum.Api.Data;
using TruYum.Api.Hubs;
using TruYum.Api.Models;

namespace TruYum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<NotificationHub> _hub;

        public CartController(AppDbContext db, IHubContext<NotificationHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        // ✅ Safe UserId parsing
        private int UserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not authenticated.");
                return int.Parse(userId);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cart = await _db.Carts
                .Include(c => c.Items).ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart == null)
                return Ok(new { items = new object[0] });

            var dto = new
            {
                cart.Id,
                cart.UserId,
                Items = cart.Items.Select(i => new
                {
                    i.MenuItemId,
                    MenuItem = new
                    {
                        i.MenuItem!.Id,
                        i.MenuItem.Name,
                        i.MenuItem.Price,
                        ImageUrl = i.MenuItem.ImageUrl
                    },
                    i.Quantity
                })
            };

            return Ok(dto);
        }

        [HttpPost("add/{menuItemId}")]
        public async Task<IActionResult> Add(int menuItemId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = UserId };
                _db.Carts.Add(cart);
            }

            var existing = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (existing == null)
                cart.Items.Add(new CartItem { MenuItemId = menuItemId, Quantity = 1 });
            else
                existing.Quantity++;

            await _db.SaveChangesAsync();

            var updatedCart = await _db.Carts
                .Where(c => c.UserId == UserId)
                .Include(c => c.Items).ThenInclude(i => i.MenuItem)
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    Items = c.Items.Select(i => new
                    {
                        i.MenuItemId,
                        MenuItem = new
                        {
                            i.MenuItem!.Id,
                            i.MenuItem.Name,
                            i.MenuItem.Price,
                            ImageUrl = i.MenuItem.ImageUrl
                        },
                        i.Quantity
                    })
                })
                .FirstOrDefaultAsync();

            // ✅ Fixed SignalR group naming
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", updatedCart);

            return Ok();
        }

        [HttpDelete("remove/{menuItemId}")]
        public async Task<IActionResult> Remove(int menuItemId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart == null) return NotFound();

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item == null) return NotFound();

            cart.Items.Remove(item);
            await _db.SaveChangesAsync();

            var updatedCart = await _db.Carts
                .Where(c => c.UserId == UserId)
                .Include(c => c.Items).ThenInclude(i => i.MenuItem)
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    Items = c.Items.Select(i => new
                    {
                        i.MenuItemId,
                        MenuItem = new
                        {
                            i.MenuItem!.Id,
                            i.MenuItem.Name,
                            i.MenuItem.Price,
                            ImageUrl = i.MenuItem.ImageUrl
                        },
                        i.Quantity
                    })
                })
                .FirstOrDefaultAsync();

            // ✅ Fixed SignalR group naming
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", updatedCart);

            return NoContent();
        }
    }
}