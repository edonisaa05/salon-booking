using System;
using SalonBooking.Models;
using SalonBooking.Services;

namespace SalonBooking.UI
{
    public class ConsoleUI
    {
        private readonly AppointmentService _service;

        public ConsoleUI(AppointmentService service)
        {
            _service = service;
        }

        public void ShfaqMenu()
        {
            while (true)
            {
                Console.WriteLine("\n===============================");
                Console.WriteLine("   SALON BOOKING SYSTEM");
                Console.WriteLine("===============================");
                Console.WriteLine("1. List All Appointments");
                Console.WriteLine("2. Create New Appointment");
                Console.WriteLine("3. Search Appointment by ID");
                Console.WriteLine("4. Update Appointment");
                Console.WriteLine("5. Delete Appointment");
                Console.WriteLine("6. Exit");
                Console.Write("\nSelect an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAll();
                        break;
                    case "2":
                        CreateNew();
                        break;
                    case "3":
                        SearchById();
                        break;
                    case "4":
                        UpdateExisting();
                        break;
                    case "5":
                        Delete();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ListAll()
        {
            Console.WriteLine("\n--- Appointment List ---");
            var appointments = _service.GetAppointments();
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments found.");
                return;
            }

            foreach (var app in appointments)
            {
                Console.WriteLine($"ID: {app.Id} | Client: {app.ClientName} | Service: {app.Service} | Date: {app.Date} {app.Time}");
            }
        }

        private void CreateNew()
        {
            try
            {
                Console.WriteLine("\n--- Create Appointment ---");
                Console.Write("Client Name: "); string name = Console.ReadLine();
                Console.Write("Service Type: "); string serviceType = Console.ReadLine();
                Console.Write("Date (dd/mm/yyyy): "); string date = Console.ReadLine();
                Console.Write("Time (hh:mm): "); string time = Console.ReadLine();

                var newApp = new Appointment(0, name, serviceType, date, time);
                _service.CreateAppointment(newApp);
                Console.WriteLine("Success: Appointment created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void SearchById()
        {
            Console.Write("\nEnter Appointment ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var app = _service.GetById(id);
                    Console.WriteLine($"Found: {app.ClientName} - {app.Service} at {app.Time} on {app.Date}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void UpdateExisting()
        {
            Console.Write("\nEnter ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var app = _service.GetById(id);
                    Console.Write($"New Client Name ({app.ClientName}): "); string name = Console.ReadLine();
                    Console.Write($"New Service ({app.Service}): "); string srv = Console.ReadLine();

                    app.ClientName = string.IsNullOrWhiteSpace(name) ? app.ClientName : name;
                    app.Service = string.IsNullOrWhiteSpace(srv) ? app.Service : srv;

                    _service.UpdateAppointment(app);
                    Console.WriteLine("Success: Appointment updated!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void Delete()
        {
            Console.Write("\nEnter ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _service.RemoveAppointment(id);
                Console.WriteLine("Success: Appointment deleted.");
            }
        }
    }
}