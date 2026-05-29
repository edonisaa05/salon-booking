using System;
using System.Collections.Generic;
using SalonBooking.Models;
using SalonBooking.Services;

namespace SalonBooking.UI
{
    public class ConsoleUI
    {
        private readonly AppointmentService _service;

        public ConsoleUI(AppointmentService service)
        {
            _service = service;
        }

        public void ShfaqMenu()
        {
            bool vazhdo = true;
            while (vazhdo)
            {
                Console.WriteLine("\n--- SISTEMI I REZERVIMEVE (PostgreSQL) ---");
                Console.WriteLine("1. Shto Rezervim");
                Console.WriteLine("2. Shfaq te gjitha Rezervimet");
                Console.WriteLine("3. Kerko sipas Emrit");
                Console.WriteLine("4. Fshi Rezervim");
                Console.WriteLine("5. Dil");
                Console.Write("Zgjidhni nje opsion: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": ShtoRezervim(); break;
                    case "2": ShfaqRezervimet(); break;
                    case "3": KerkoSipasEmrit(); break;
                    case "4": FshiRezervim(); break;
                    case "5": vazhdo = false; break;
                    default:
                        Console.WriteLine("Zgjedhje e gabuar! Provoni perseri.");
                        break;
                }
            }
        }

        private void ShtoRezervim()
        {
            try
            {
                Console.WriteLine("\n--- REZERVIM I RI ---");

                Console.Write("Emri i Klientit: ");
                string emri = Console.ReadLine();

                Console.Write("Sherbimi (p.sh. Prerje, Ngjyrosje): ");
                string sherbimi = Console.ReadLine();

                Console.Write("Data dhe Ora (Format: yyyy-MM-dd HH:mm): ");
                string dataInput = Console.ReadLine();

                if (!DateTime.TryParse(dataInput, out DateTime data))
                    throw new FormatException("Formati i dates nuk eshte i sakte!");

                var rezervimi = new Booking
                {
                    CustomerName = emri,
                    ServiceName = sherbimi,
                    AppointmentDate = data
                };

                _service.CreateBooking(rezervimi);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"SUKSES: Rezervimi u ruajt ne PostgreSQL me ID = {rezervimi.Id}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ShfaqGabim(ex.Message);
            }
        }

        private void ShfaqRezervimet()
        {
            try
            {
                var lista = _service.GetHistory();
                Console.WriteLine("\n--- LISTA E REZERVIMEVE ---");

                if (lista.Count == 0)
                {
                    Console.WriteLine("(Nuk ka asnje rezervim ne databaze.)");
                    return;
                }

                PrintHeader();
                foreach (var b in lista)
                    PrintRow(b);
            }
            catch (Exception ex)
            {
                ShfaqGabim(ex.Message);
            }
        }

        private void KerkoSipasEmrit()
        {
            try
            {
                Console.Write("\nJepni emrin per kerkim: ");
                string emri = Console.ReadLine();

                var rezultatet = _service.SearchByName(emri);
                Console.WriteLine($"\n--- REZULTATET ({rezultatet.Count} gjetur) ---");

                if (rezultatet.Count == 0)
                {
                    Console.WriteLine("Asnje rezervim nuk u gjet.");
                    return;
                }

                PrintHeader();
                foreach (var b in rezultatet)
                    PrintRow(b);
            }
            catch (Exception ex)
            {
                ShfaqGabim(ex.Message);
            }
        }

        private void FshiRezervim()
        {
            try
            {
                Console.Write("\nJepni ID-n e rezervimit per ta fshire: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                    throw new FormatException("ID duhet te jete numer!");

                var b = _service.GetById(id);
                Console.WriteLine($"Rezervimi: {b.CustomerName} | {b.ServiceName} | {b.AppointmentDate:yyyy-MM-dd HH:mm}");
                Console.Write("Jeni te sigurt? (p/j): ");
                if (Console.ReadLine()?.Trim().ToLower() != "p")
                {
                    Console.WriteLine("Anuluar.");
                    return;
                }

                _service.RemoveBooking(id);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Rezervimi #{id} u fshi nga databaza.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ShfaqGabim(ex.Message);
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine($"{"ID",-5} {"Klienti",-25} {"Sherbimi",-20} {"Data & Ora",-20}");
            Console.WriteLine(new string('-', 72));
        }

        private void PrintRow(Booking b)
        {
            Console.WriteLine($"{b.Id,-5} {b.CustomerName,-25} {b.ServiceName,-20} {b.AppointmentDate:yyyy-MM-dd HH:mm}");
        }

        private void ShfaqGabim(string mesazhi)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[GABIM]: {mesazhi}");
            Console.ResetColor();
            Console.WriteLine("Shtypni cfaredо taste per te vazhduar...");
            Console.ReadKey();
        }
    }
} 