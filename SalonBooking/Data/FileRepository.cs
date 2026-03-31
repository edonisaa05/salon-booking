using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Kjo është shumë e rëndësishme për Max()
using SalonBooking.Models;

namespace SalonBooking.Data
{
    public class FileRepository : IRepository<Appointment>
    {
        private string _filePath = "Data/appointments.csv";
        private List<Appointment> _appointments = new List<Appointment>();

        public FileRepository()
        {
            // Krijo folderin Data nëse nuk ekziston që të mos dështojë programi
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            Load();
        }

        private void Load()
        {
            if (!File.Exists(_filePath)) return;

            var lines = File.ReadAllLines(_filePath);
            _appointments.Clear();
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    _appointments.Add(new Appointment(
                        int.Parse(parts[0]), parts[1], parts[2], parts[3], parts[4]
                    ));
                }
            }
        }

        public List<Appointment> GetAll() => _appointments;

        public Appointment GetById(int id) => _appointments.Find(a => a.Id == id);

        public void Add(Appointment item)
        {
            // Logjika për auto-increment ID
            item.Id = _appointments.Any() ? _appointments.Max(a => a.Id) + 1 : 1;
            _appointments.Add(item);
            Save();
        }

        public void Delete(int id)
        {
            _appointments.RemoveAll(a => a.Id == id);
            Save();
        }

        public void Update(Appointment updatedItem)
        {
            var index = _appointments.FindIndex(a => a.Id == updatedItem.Id);
            if (index != -1)
            {
                _appointments[index] = updatedItem;
                Save();
            }
        }

        public void Save()
        {
            var lines = new List<string>();
            foreach (var a in _appointments)
                lines.Add($"{a.Id},{a.ClientName},{a.Service},{a.Date},{a.Time}");

            File.WriteAllLines(_filePath, lines);
        }
    }
}