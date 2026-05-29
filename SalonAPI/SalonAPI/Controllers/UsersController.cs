using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SalonAPI.Models;

namespace SalonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private const string ConnString =
            "Host=localhost;" +
            "Port=5432;" +
            "Database=salon_booking;" +
            "Username=postgres;" +
            "Password=1234;";

        // POST: api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Email dhe fjalëkalimi janë të detyrueshëm!");

            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT id, name, email, role FROM users WHERE email = @email AND password = @password;",
                conn);
            cmd.Parameters.AddWithValue("email", req.Email.ToLower().Trim());
            cmd.Parameters.AddWithValue("password", req.Password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var user = new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Role = reader.GetString(3)
                };
                return Ok(user);
            }

            return Unauthorized("Email ose fjalëkalimi i gabuar!");
        }

        // POST: api/users/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Emri është i detyrueshëm!");
            if (string.IsNullOrWhiteSpace(req.Email))
                return BadRequest("Email është i detyrueshëm!");
            if (string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Fjalëkalimi është i detyrueshëm!");
            if (req.Password.Length < 6)
                return BadRequest("Fjalëkalimi duhet të ketë të paktën 6 karaktere!");

            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();

            // Kontrollo nëse email ekziston
            using var checkCmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM users WHERE email = @email;", conn);
            checkCmd.Parameters.AddWithValue("email", req.Email.ToLower().Trim());
            var count = Convert.ToInt32(checkCmd.ExecuteScalar());
            if (count > 0)
                return Conflict("Ky email është i regjistruar tashmë!");

            // Shto userin e ri
            using var insertCmd = new NpgsqlCommand(@"
                INSERT INTO users (name, email, password, role)
                VALUES (@name, @email, @password, 'user')
                RETURNING id;", conn);
            insertCmd.Parameters.AddWithValue("name", req.Name.Trim());
            insertCmd.Parameters.AddWithValue("email", req.Email.ToLower().Trim());
            insertCmd.Parameters.AddWithValue("password", req.Password);

            var newId = Convert.ToInt32(insertCmd.ExecuteScalar());

            return Ok(new User
            {
                Id = newId,
                Name = req.Name.Trim(),
                Email = req.Email.ToLower().Trim(),
                Role = "user"
            });
        }
    }

    // Request model për login
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
