using System;
using System.Collections.Generic;
using System.Linq;
using SalonBooking.Data;
using SalonBooking.Models;

namespace SalonBooking.Services
{
    public class AppointmentService
    {
        private readonly FileRepository _repository;

        // Dependency Injection: Service receives the Repository as a parameter
        public AppointmentService(FileRepository repository)
        {
            _repository = repository;
        }

        // Method 1: List with filtering (Requirement: 3 methods)
        public List<Appointment> GetAppointments(string filter = "")
        {
            var all = _repository.GetAll();
            if (string.IsNullOrEmpty(filter)) return all;

            return all.Where(a => a.ClientName.ToLower().Contains(filter.ToLower())).ToList();
        }

        // Method 2: Add with validation (Requirement: Name not empty)
        public void CreateAppointment(Appointment appointment)
        {
            if (string.IsNullOrWhiteSpace(appointment.ClientName))
            {
                throw new Exception("Validation Error: Client Name cannot be empty!");
            }

            // You can add additional logic here (e.g., date validation)
            _repository.Add(appointment);
        }

        // Method 3: Find by ID
        public Appointment GetById(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null)
            {
                throw new Exception($"Error: Appointment with ID {id} not found!");
            }
            return appointment;
        }

        // Update and Delete (For the bonus 10 points)
        public void UpdateAppointment(Appointment updatedItem)
        {
            if (string.IsNullOrWhiteSpace(updatedItem.ClientName))
                throw new Exception("Client Name is required for updates!");

            _repository.Update(updatedItem);
        }

        public void RemoveAppointment(int id)
        {
            _repository.Delete(id);
        }
    }
}