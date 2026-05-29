using System;
using System.Collections.Generic;
using System.Linq;
using SalonBooking.Data;
using SalonBooking.Models;

namespace SalonBooking.Services
{
    public class AppointmentService
    {
        private readonly PostgreSqlRepository _repository;

        public AppointmentService(PostgreSqlRepository repository)
        {
            _repository = repository;
        }

        public void CreateBooking(Booking booking)
        {
            if (string.IsNullOrWhiteSpace(booking.CustomerName))
                throw new Exception("Gabim Validimi: Emri i klientit nuk mund të jetë bosh!");

            if (string.IsNullOrWhiteSpace(booking.ServiceName))
                throw new Exception("Gabim Validimi: Shërbimi nuk mund të jetë bosh!");

            _repository.Save(booking);
        }

        public List<Booking> GetHistory(string filter = "")
        {
            var all = _repository.GetAll();

            if (string.IsNullOrEmpty(filter))
                return all;

            return all
                .Where(b => b.CustomerName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public Booking GetById(int id)
        {
            var booking = _repository.GetById(id);
            if (booking == null)
                throw new Exception($"Gabim: Rezervimi me ID {id} nuk u gjet!");
            return booking;
        }

        public void UpdateBooking(Booking booking)
        {
            if (string.IsNullOrWhiteSpace(booking.CustomerName))
                throw new Exception("Emri i klientit është i detyrueshëm për përditësim!");

            _repository.Update(booking);
        }

        public void RemoveBooking(int id)
        {
            _repository.Delete(id);
        }

        public List<Booking> SearchByName(string name)
        {
            return _repository.GetAll()
                .Where(b => b.CustomerName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }
}