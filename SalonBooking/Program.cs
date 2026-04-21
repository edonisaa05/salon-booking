using System;
using SalonBooking.Data;
using SalonBooking.Services;
using SalonBooking.UI;

namespace SalonBooking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Krijojmë instancën e Repository-t (Shtresa e të dhënave)
            // Kjo shtresë merret me leximin/shkrimin në skedarin CSV
            FileRepository repo = new FileRepository();

            // 2. Krijojmë Shërbimin dhe "injektojmë" repository-n
            // Kjo quhet Dependency Injection (DI) - Përmirësimi 1
            AppointmentService service = new AppointmentService(repo);

            // 3. Krijojmë UI dhe "injektojmë" shërbimin
            // UI do të përdorë try-catch për Reliability - Përmirësimi 2
            ConsoleUI ui = new ConsoleUI(service);

            // 4. Nisim ekzekutimin e menusë kryesore
            ui.ShfaqMenu();
        }
    }
}