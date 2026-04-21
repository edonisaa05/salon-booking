using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SalonBooking.Models;
using SalonBooking.Interfaces;

namespace SalonBooking.Data
{
    // Kjo klasë tashmë implementon saktë ndërfaqen
    public class FileRepository : IBookingRepository
    {
        private string filePath = "rezervimet.csv";

        public void Save(Booking booking)
        {
            // Sigurohemi që ID të gjenerohet nëse mungon
            var rezervimet = GetAll();
            booking.Id = rezervimet.Count > 0 ? rezervimet.Max(x => x.Id) + 1 : 1;

            string linja = $"{booking.Id},{booking.CustomerName},{booking.ServiceName},{booking.AppointmentDate:yyyy-MM-dd}";
            File.AppendAllLines(filePath, new[] { linja });
        }

        public List<Booking> GetAll()
        {
            var lista = new List<Booking>();

            if (!File.Exists(filePath)) return lista;

            var rreshtat = File.ReadAllLines(filePath);
            foreach (var rresht in rreshtat)
            {
                var teDhenat = rresht.Split(',');
                if (teDhenat.Length == 4)
                {
                    lista.Add(new Booking
                    {
                        Id = int.Parse(teDhenat[0]),
                        CustomerName = teDhenat[1],
                        ServiceName = teDhenat[2],
                        AppointmentDate = DateTime.Parse(teDhenat[3])
                    });
                }
            }
            return lista;
        }

        // Shtojmë metodat e tjera që mund t'i kërkojë ndërfaqja IBookingRepository
        public void Delete(int id) { /* Logjika fakultative */ }
        public Booking GetById(int id) => GetAll().FirstOrDefault(x => x.Id == id);
    }
}