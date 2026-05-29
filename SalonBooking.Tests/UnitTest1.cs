using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalonBooking.Models;
using SalonBooking.Services;
using SalonBooking.Data;
using System.Collections.Generic;
using System.Linq;

namespace SalonBooking.Tests
{
    [TestClass]
    public class AppointmentTests
    {
        [TestMethod]
        public void SearchByName_ShouldReturnResults_WhenNameExists()
        {
            // Përgatitja (Arrange)
            var repo = new PostgreSqlRepository();
            var service = new AppointmentService(repo);
            var testApp = new Appointment(999, "FilanFisteku", "Prerje", "01/01/2026", "10:00");

            try { service.CreateAppointment(testApp); } catch { }

            // Veprimi (Act)
            var results = service.SearchByName("Filan");

            // Verifikimi (Assert)
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any(a => a.ClientName.Contains("Filan")));
        }

        [TestMethod]
        public void SearchByName_ShouldReturnEmpty_WhenNameDoesNotExist()
        {
            var repo = new PostgreSqlRepository();
            var service = new AppointmentService(repo);
            var results = service.SearchByName("EmërQëNukEkziston123");

            Assert.AreEqual(0, results.Count);
        }
    }
}