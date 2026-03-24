using System;
using SalonBooking.Services;

namespace SalonBooking.UI
{
    public class ConsoleUI
    {
        private AppointmentService _service;

        public ConsoleUI(AppointmentService service)
        {
            _service = service;
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== Salon Booking System ===");
                Console.WriteLine("1. View all appointments");
                Console.WriteLine("2. Add new appointment");
                Console.WriteLine("3. Search appointment by ID");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAll();
                        break;
                    case "2":
                        AddNew();
                        break;
                    case "3":
                        SearchById();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        private void ShowAll()
        {
            var list = _service.GetAllAppointments();
            if (list.Count == 0)
            {
                Console.WriteLine("No appointments found!");
                return;
            }
            foreach (var a in list)
                Console.WriteLine($"ID:{a.Id} | {a.ClientName} | {a.Service} | {a.Date} | {a.Time}");
        }

        private void AddNew()
        {
            Console.Write("Client name: ");
            string name = Console.ReadLine();
            Console.Write("Service (Haircut/Manicure/etc): ");
            string service = Console.ReadLine();
            Console.Write("Date (yyyy-MM-dd): ");
            string date = Console.ReadLine();
            Console.Write("Time (HH:mm): ");
            string time = Console.ReadLine();

            int id = _service.GetAllAppointments().Count + 1;
            _service.BookAppointment(id, name, service, date, time);
            Console.WriteLine("Appointment added successfully!");
        }

        private void SearchById()
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine());
            var a = _service.GetAppointment(id);
            if (a == null)
                Console.WriteLine("Appointment not found!");
            else
                Console.WriteLine($"ID:{a.Id} | {a.ClientName} | {a.Service} | {a.Date} | {a.Time}");
        }
    }
}