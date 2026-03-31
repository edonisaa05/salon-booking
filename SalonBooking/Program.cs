using SalonBooking.Data;
using SalonBooking.Services;
using SalonBooking.UI;

namespace SalonBooking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Krijohen shtresat sipas arkitekturës
            FileRepository repo = new FileRepository();
            AppointmentService service = new AppointmentService(repo);
            ConsoleUI ui = new ConsoleUI(service);

            // Nis programin
            ui.ShfaqMenu();
        }
    }
}