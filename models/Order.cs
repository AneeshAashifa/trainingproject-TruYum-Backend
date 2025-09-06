using System;
using System.Collections.Generic;

namespace TruYum.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string Status { get; set; } = "Placed";
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal MenuItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}