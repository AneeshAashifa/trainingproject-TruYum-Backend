using TruYum.Api.Models;

namespace TruYum.models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    }
}
