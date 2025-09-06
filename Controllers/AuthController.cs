using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TruYum.Api.Data;
using TruYum.Api.Models;

namespace TruYum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // 🔹 Register endpoint
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists");
            }

            // Default role = User (unless explicitly provided as Admin)
            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = "User";
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new { message = "User registered successfully", role = user.Role });
        }

        // 🔹 Login endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Username.ToLower() == login.Username.ToLower() &&
                u.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // ✅ Add UserId for backend CartController
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new { token = jwtToken, role = user.Role });
        }

        // 🔹 Debug endpoint - check existing users
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }
    }
}