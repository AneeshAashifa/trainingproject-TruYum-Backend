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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<NotificationHub> _hub;
        public OrdersController(AppDbContext db, IHubContext<NotificationHub> hub) { _db = db; _hub = hub; }
        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder()
        {
            var cart = await _db.Carts
                .Include(c => c.Items).ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart == null || !cart.Items.Any()) return BadRequest("Cart empty");

            var order = new Order
            {
                UserId = UserId,
                PlacedAt = System.DateTime.UtcNow,
                Items = cart.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItem!.Name,
                    MenuItemPrice = i.MenuItem.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
            order.Total = order.Items.Sum(it => it.MenuItemPrice * it.Quantity);

            _db.Orders.Add(order);

            // clear cart
            _db.CartItems.RemoveRange(cart.Items);
            await _db.SaveChangesAsync();

            var orderDto = new
            {
                order.Id,
                order.PlacedAt,
                order.Total,
                order.Status,
                Items = order.Items.Select(it => new { it.MenuItemName, it.MenuItemPrice, it.Quantity })
            };

            await _hub.Clients.Group($"user{UserId}").SendAsync("OrderPlaced", orderDto);

            var emptyCartDto = new { Id = cart.Id, UserId = cart.UserId, Items = new object[0] };
            await _hub.Clients.Group($"user{UserId}").SendAsync("CartUpdated", emptyCartDto);

            return Ok(orderDto);
        }

        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            var orders = await _db.Orders
                .Where(o => o.UserId == UserId)
                .OrderByDescending(o => o.PlacedAt)
                .Select(o => new
                {
                    o.Id,
                    o.PlacedAt,
                    o.Total,
                    o.Status,
                    Items = o.Items.Select(i => new { i.MenuItemName, i.MenuItemPrice, i.Quantity })
                }).ToListAsync();

            return Ok(orders);
        }
    }
}
