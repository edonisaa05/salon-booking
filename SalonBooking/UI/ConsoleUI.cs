using System;
using System.Collections.Generic;
using SalonBooking.Services;
using SalonBooking.Models;
using System.Linq;

namespace SalonBooking.UI
{
    public class ConsoleUI
    {
        private readonly AppointmentService _appointmentService;

        public ConsoleUI(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public void ShfaqMenu()
        {
            bool vazhdo = true;
            while (vazhdo)
            {
                Console.WriteLine("\n--- SISTEMI I REZERVIMEVE ---");
                Console.WriteLine("1. Shto Rezervim");
                Console.WriteLine("2. Shfaq Historikun");
                Console.WriteLine("3. Dil");
                Console.Write("Zgjidhni një opsion: ");

                string zgjedhja = Console.ReadLine();

                switch (zgjedhja)
                {
                    case "1":
                        ShtoRezervimInteraktiv();
                        break;
                    case "2":
                        ShfaqRezervimet();
                        break;
                    case "3":
                        vazhdo = false;
                        break;
                    default:
                        Console.WriteLine("Zgjedhje e gabuar! Provoni përsëri.");
                        break;
                }
            }
        }

        private void ShtoRezervimInteraktiv()
        {
            // FILLIMI I TRY-CATCH (Përmirësimi në Reliability)
            try
            {
                Console.WriteLine("\n--- REGJISTRIMI I REZERVIMIT TË RI ---");

                Console.Write("Emri i Klientit: ");
                string emri = Console.ReadLine();

                Console.Write("Shërbimi (p.sh. Prerje): ");
                string sherbimi = Console.ReadLine();

                Console.Write("Data (Format: vvvv-mm-dd): ");
                string dataInput = Console.ReadLine();

                if (!DateTime.TryParse(dataInput, out DateTime data))
                {
                    // Hedhim një përjashtim nëse formati i datës është i gabuar
                    throw new FormatException("Formati i datës nuk është i saktë!");
                }

                // Krijojmë objektin e modelit
                var rezervimiIRi = new Booking
                {
                    CustomerName = emri,
                    ServiceName = sherbimi,
                    AppointmentDate = data
                };

                // Thërrasim shërbimin për ruajtje
                _appointmentService.CreateBooking(rezervimiIRi);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SUKSES: Rezervimi u ruajt me sukses!");
                Console.ResetColor();
            }
            catch (FormatException ex)
            {
                // Kapim gabimet e formatit (p.sh. shkronja në vend të numrave te data)
                ShfaqMesazhinEGabimit($"Gabim Formati: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                // Kapim gabimet e validimit nga AppointmentService
                ShfaqMesazhinEGabimit($"Gabim Validimi: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Kapim çdo gabim tjetër të papritur (p.sh. probleme me skedarin)
                ShfaqMesazhinEGabimit($"Një gabim i papritur ndodhi: {ex.Message}");
            }
        }

        private void ShfaqRezervimet()
        {
            var lista = _appointmentService.GetHistory();
            Console.WriteLine("\n--- LISTA E REZERVIMEVE TË REGJISTRUARA ---");

            if (lista == null || !lista.Any())
            {
                Console.WriteLine("Nuk ka asnjë rezervim në sistem.");
                return;
            }

            foreach (Booking b in lista)
            {
                Console.WriteLine($"ID: {b.Id} | Klienti: {b.CustomerName} | Shërbimi: {b.ServiceName} | Data: {b.AppointmentDate.ToShortDateString()}");
            }
        }

        private void ShfaqMesazhinEGabimit(string mesazhi)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[RELIABILITY ALERT]: {mesazhi}");
            Console.ResetColor();
            Console.WriteLine("Shtypni çfarëdo taste për të vazhduar...");
            Console.ReadKey();
        }
    }
}