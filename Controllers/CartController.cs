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

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // 🔹 Helper: build cart DTO
        private async Task<object?> BuildCartDto()
        {
            return await _db.Carts
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
                            i.MenuItem.ImageUrl
                        },
                        i.Quantity
                    }),
                    Total = c.Items.Sum(i => i.MenuItem!.Price * i.Quantity)
                })
                .FirstOrDefaultAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cart = await BuildCartDto();
            if (cart == null) return Ok(new { items = new object[0], total = 0 });
            return Ok(cart);
        }

        [HttpPost("add/{menuItemId}")]
        public async Task<IActionResult> Add(int menuItemId)
        {
            var cart = await _db.Carts.Include(c => c.Items)
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

            var updatedCart = await BuildCartDto();
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", updatedCart);

            return Ok(updatedCart);
        }

        [HttpPost("increase/{menuItemId}")]
        public async Task<IActionResult> Increase(int menuItemId)
        {
            var item = await _db.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Cart!.UserId == UserId && i.MenuItemId == menuItemId);

            if (item == null) return NotFound();

            item.Quantity++;
            await _db.SaveChangesAsync();

            var updatedCart = await BuildCartDto();
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", updatedCart);

            return Ok(updatedCart);
        }

        [HttpPost("decrease/{menuItemId}")]
        public async Task<IActionResult> Decrease(int menuItemId)
        {
            var item = await _db.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Cart!.UserId == UserId && i.MenuItemId == menuItemId);

            if (item == null) return NotFound();

            item.Quantity--;
            if (item.Quantity <= 0)
                _db.CartItems.Remove(item);

            await _db.SaveChangesAsync();

            var updatedCart = await BuildCartDto();
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", updatedCart);

            return Ok(updatedCart);
        }

        [HttpDelete("remove/{menuItemId}")]
        public async Task<IActionResult> Remove(int menuItemId)
        {
            var cart = await _db.Carts.Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart == null) return NotFound();

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item == null) return NotFound();

            cart.Items.Remove(item);
            await _db.SaveChangesAsync();

            var updatedCart = await BuildCartDto();
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", updatedCart);

            return NoContent();
        }
    }
}