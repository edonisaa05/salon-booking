using SalonBooking.Data;
using SalonBooking.Services;
using SalonBooking.UI;

var repository = new FileRepository();
var service = new AppointmentService(repository);
var ui = new ConsoleUI(service);
ui.Run();