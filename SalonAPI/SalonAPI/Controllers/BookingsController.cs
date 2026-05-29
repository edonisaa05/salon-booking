using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SalonAPI.Models;

namespace SalonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private const string ConnString =
            "Host=localhost;" +
            "Port=5432;" +
            "Database=salon_booking;" +
            "Username=postgres;" +
            "Password=1234;";

        // GET: api/bookings
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = new List<Booking>();
            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT id, customer_name, service_name, appointment_date FROM bookings ORDER BY appointment_date DESC;",
                conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Booking
                {
                    Id = reader.GetInt32(0),
                    CustomerName = reader.GetString(1),
                    ServiceName = reader.GetString(2),
                    AppointmentDate = reader.GetDateTime(3)
                });
            }
            return Ok(lista);
        }

        // POST: api/bookings
        [HttpPost]
        public IActionResult Create([FromBody] Booking booking)
        {
            if (string.IsNullOrWhiteSpace(booking.CustomerName))
                return BadRequest("Emri i klientit nuk mund të jetë bosh!");

            if (string.IsNullOrWhiteSpace(booking.ServiceName))
                return BadRequest("Shërbimi nuk mund të jetë bosh!");

            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();

            // Kontroll konflikti: a është ky orar i zënë?
            using var checkCmd = new NpgsqlCommand(@"
                SELECT COUNT(*) FROM bookings
                WHERE appointment_date = @date;", conn);
            checkCmd.Parameters.AddWithValue("date", booking.AppointmentDate);
            var count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count > 0)
                return BadRequest("Ky orar është i zënë! Ju lutemi zgjidhni një orë tjetër.");

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO bookings (customer_name, service_name, appointment_date)
                VALUES (@name, @service, @date)
                RETURNING id;", conn);

            cmd.Parameters.AddWithValue("name", booking.CustomerName);
            cmd.Parameters.AddWithValue("service", booking.ServiceName);
            cmd.Parameters.AddWithValue("date", booking.AppointmentDate);
            booking.Id = Convert.ToInt32(cmd.ExecuteScalar());

            return Ok(booking);
        }

        // DELETE: api/bookings/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM bookings WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            return Ok("Rezervimi u fshi!");
        }

        // PUT: api/bookings/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Booking booking)
        {
            if (string.IsNullOrWhiteSpace(booking.CustomerName))
                return BadRequest("Emri i klientit nuk mund të jetë bosh!");

            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
                UPDATE bookings
                SET customer_name    = @name,
                    service_name     = @service,
                    appointment_date = @date
                WHERE id = @id;", conn);

            cmd.Parameters.AddWithValue("name", booking.CustomerName);
            cmd.Parameters.AddWithValue("service", booking.ServiceName);
            cmd.Parameters.AddWithValue("date", booking.AppointmentDate);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            return Ok("Rezervimi u përditësua!");
        }
    }
}