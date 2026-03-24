using System.Collections.Generic;
using SalonBooking.Data;
using SalonBooking.Models;

namespace SalonBooking.Services
{
    public class AppointmentService
    {
        private IRepository<Appointment> _repository;

        public AppointmentService(IRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _repository.GetAll();
        }

        public Appointment GetAppointment(int id)
        {
            return _repository.GetById(id);
        }

        public void BookAppointment(int id, string clientName, string service, string date, string time)
        {
            var appointment = new Appointment(id, clientName, service, date, time);
            _repository.Add(appointment);
            _repository.Save();
        }
    }
}