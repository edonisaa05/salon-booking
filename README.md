\# Salon Booking System - Dokumentimi i Projektit



Ky projekt është një sistem për menaxhimin e rezervimeve në sallone bukurie, i ndërtuar në C# me fokus në arkitekturën e shtresëzuar dhe përdorimin e Repository Pattern për ruajtjen e të dhënave.



\## 📁 Organizimi i Kodit (Struktura e Projektit)

Sipas kërkesave të detyrës, projekti është ndarë në katër shtresa kryesore:



1\. \*\*Shtresa Data\*\*: Përmban interface-in `IRepository.cs` dhe implementimin konkret `FileRepository.cs`. Kjo shtresë mundëson operacionet CRUD dhe ruajtjen e të dhënave në skedarë CSV.

2\. \*\*Shtresa Models\*\*: Këtu ndodhet klasa `Appointment.cs`, e cila shërben si modeli bazë i të dhënave që qarkullon nëpër sistem.

3\. \*\*Shtresa Services\*\*: Përmban `AppointmentService.cs`, ku është izoluar logjika e biznesit (p.sh. validimet), duke mos e ngarkuar kodin e UI.

4\. \*\*Shtresa UI\*\*: Përmban `ConsoleUI.cs`, e cila menaxhon të gjithë ndërveprimin me përdoruesin në terminal.



\## 🛠️ Repository Pattern \& Arkitektura

\- \*\*Program.cs\*\*: Është mbajtur minimal (max 10 rreshta), duke shërbyer vetëm si pikë nisjeje për aplikacionin.

\- \*\*Abstraksioni\*\*: Përdorimi i `IRepository` mundëson që logjika e aplikacionit të jetë e pavarur nga mënyra se si ruhen të dhënat (CSV, Database, etj.).



\## 🚀 Ekzekutimi

Aplikacioni niset nga `Program.cs`, i cili inicializon shërbimet dhe thërret metodat e `ConsoleUI` për të filluar menaxhimin e takimeve.

