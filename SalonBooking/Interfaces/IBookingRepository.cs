using SalonBooking.Interfaces;
using SalonBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonBooking.Interfaces
{
    public interface IBookingRepository
    {
        void Save(Booking booking);
        List<Booking> GetAll();
    }
}
