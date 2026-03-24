using System;
using System.Collections.Generic;
using System.IO;
using SalonBooking.Models;

namespace SalonBooking.Data
{
    public class FileRepository : IRepository<Appointment>
    {
        private string _filePath = "Data/appointments.csv";
        private List<Appointment> _appointments = new List<Appointment>();

        public FileRepository()
        {
            Directory.CreateDirectory("Data");
            Load();
        }

        private void Load()
        {
            if (!File.Exists(_filePath)) return;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                _appointments.Add(new Appointment(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4]
                ));
            }
        }

        public List<Appointment> GetAll() => _appointments;

        public Appointment GetById(int id)
        {
            return _appointments.Find(a => a.Id == id);
        }

        public void Add(Appointment item)
        {
            _appointments.Add(item);
        }

        public void Save()
        {
            var lines = new List<string>();
            foreach (var a in _appointments)
                lines.Add(a.ToString());

            File.WriteAllLines(_filePath, lines);
        }
    }
}
