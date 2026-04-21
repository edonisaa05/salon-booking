using System;

namespace SalonBooking.Services
{
    public static class ValidationService
    {
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length >= 2;
        }

        public static bool IsFutureDate(string dateStr, out DateTime validatedDate)
        {
            bool isDate = DateTime.TryParse(dateStr, out validatedDate);
            return isDate && validatedDate.Date >= DateTime.Now.Date;
        }

        public static bool IsValidId(int id)
        {
            return id > 0;
        }
    }
}
