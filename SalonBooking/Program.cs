using System;
using SalonBooking.Data;
using SalonBooking.Services;
using SalonBooking.UI;

namespace SalonBooking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Salon Booking — PostgreSQL ===\n");

            var repo = new PostgreSqlRepository();

            try
            {
                repo.InitializeDatabase();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[GABIM LIDHJEJE] Nuk u lidh me PostgreSQL:\n{ex.Message}");
                Console.WriteLine("\nKontroloni:");
                Console.WriteLine("  • PostgreSQL është duke u ekzekutuar?");
                Console.WriteLine("  • Kredencialet në PostgreSqlRepository.cs janë të sakta?");
                Console.ResetColor();
                Console.WriteLine("\nShtypni çfarëdo taste për të dalë...");
                Console.ReadKey();
                return;
            }

            var service = new AppointmentService(repo);
            var ui = new ConsoleUI(service);
            ui.ShfaqMenu();
        }
    }
}