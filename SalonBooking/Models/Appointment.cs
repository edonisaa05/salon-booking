using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonBooking.Models
{
   public class Appointment
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Service { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        public Appointment() { }

        public Appointment(int id, string clientName, string service, string date, string time)
        {
            Id = id;
            ClientName = clientName;
            Service = service;
            Date = date;
            Time = time;
        }

        public override string ToString()
        {
            return $"{Id},{ClientName},{Service},{Date},{Time}";
        }
    }
}
