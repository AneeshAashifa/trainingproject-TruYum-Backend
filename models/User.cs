using System.ComponentModel.DataAnnotations.Schema;

namespace TruYum.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        [Column("PasswordHash")]   // 👈 map to DB column
        public string Password { get; set; } = "";

        public string Role { get; set; } = "";
    }
}
