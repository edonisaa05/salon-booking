using System;

namespace SalonBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}