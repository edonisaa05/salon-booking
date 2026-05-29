using System;
using System.Collections.Generic;
using Npgsql;
using SalonBooking.Models;
using SalonBooking.Interfaces;

namespace SalonBooking.Data
{
    public class PostgreSqlRepository : IBookingRepository
    {
        // ── Ndrysho passwordin këtu ──────────────────────────────────────
        private const string ConnString =
            "Host=localhost;" +
            "Port=5432;" +
            "Database=salon_booking;" +
            "Username=postgres;" +
            "Password=1234;";
        // ────────────────────────────────────────────────────────────────

        public void InitializeDatabase()
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(@"
                    CREATE TABLE IF NOT EXISTS bookings (
                        id               SERIAL PRIMARY KEY,
                        customer_name    VARCHAR(100) NOT NULL,
                        service_name     VARCHAR(100) NOT NULL,
                        appointment_date TIMESTAMP    NOT NULL
                    );", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("[DB] Tabela 'bookings' është gati.");
            }
        }

        public void Save(Booking booking)
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(@"
                    INSERT INTO bookings (customer_name, service_name, appointment_date)
                    VALUES (@name, @service, @date)
                    RETURNING id;", conn))
                {
                    cmd.Parameters.AddWithValue("name", booking.CustomerName);
                    cmd.Parameters.AddWithValue("service", booking.ServiceName);
                    cmd.Parameters.AddWithValue("date", booking.AppointmentDate);
                    booking.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<Booking> GetAll()
        {
            var lista = new List<Booking>();
            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "SELECT id, customer_name, service_name, appointment_date FROM bookings ORDER BY appointment_date DESC;",
                    conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
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
                    }
                }
            }
            return lista;
        }

        public Booking GetById(int id)
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "SELECT id, customer_name, service_name, appointment_date FROM bookings WHERE id = @id;",
                    conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Booking
                            {
                                Id = reader.GetInt32(0),
                                CustomerName = reader.GetString(1),
                                ServiceName = reader.GetString(2),
                                AppointmentDate = reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Delete(int id)
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "DELETE FROM bookings WHERE id = @id;", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Booking booking)
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(@"
                    UPDATE bookings
                    SET customer_name    = @name,
                        service_name     = @service,
                        appointment_date = @date
                    WHERE id = @id;", conn))
                {
                    cmd.Parameters.AddWithValue("name", booking.CustomerName);
                    cmd.Parameters.AddWithValue("service", booking.ServiceName);
                    cmd.Parameters.AddWithValue("date", booking.AppointmentDate);
                    cmd.Parameters.AddWithValue("id", booking.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}