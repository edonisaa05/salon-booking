# Raporti i Përmirësimeve Teknike - SalonBooking

Ky dokument detajon ndryshimet arkitekturore dhe funksionale të realizuara në projekt për të rritur cilësinë, qëndrueshmërinë dhe mirëmbajtjen e softuerit.

---

## 1. Implementimi i Dependency Injection (DI)
* **Problemi:** Shtresat e kodit ishin të lidhura ngushtë (tightly coupled). `AppointmentService` krijonte vetë instancën e `FileRepository`, duke e bërë të pamundur testimin pa krijuar skedarë realë në disk.
* **Ndryshimi:** U krijua ndërfaqja `IBookingRepository`. Tani `FileRepository` implementon këtë interface, dhe shërbimi e pranon atë përmes konstruktorit.
* **Pse është më mirë:** Kodi është më fleksibël. Mund të ndërrojmë mënyrën e ruajtjes së të dhënave (p.sh. nga CSV në SQL) pa ndryshuar asnjë rresht kodi në logjikën e biznesit.

## 2. Reliability: Centralized Error Handling & Validation
* **Problemi:** Programi dështonte (crash) nëse përdoruesi jepte data në format të gabuar ose fusha boshe. Nuk kishte asnjë mekanizëm për të kapur përjashtimet (exceptions).
* **Ndryshimi:** U implementua blloku `try-catch` në shtresën e UI dhe u shtua validimi me `DateTime.TryParse`.
* **Pse është më mirë:** Sistemi është "resilient". Në rast gabimi, përdoruesi merr një mesazh informues dhe programi vazhdon punën në vend që të mbyllet papritur.

## 3. Përmirësimi i Dokumentimit Teknik
* **Problemi:** Mungesa e një udhëzuesi e bënte të vështirë kuptimin e strukturës së projektit dhe mënyrën e ekzekutimit të testeve automatike.
* **Ndryshimi:** Krijimi i `README.md` dhe `architecture.md` me udhëzime të qarta mbi arkitekturën UI -> Service -> Repository.
* **Pse është më mirë:** Lehtëson bashkëpunimin në ekip dhe procesin e "onboarding" për inxhinierët e rinj që do të merren me mirëmbajtjen e projektit.

---

## Çka mbetet ende e dobët (Limitimet e projektit)
Megjithëse projekti ka pësuar përmirësime të mëdha, ekzistojnë disa pika që mund të zhvillohen në të ardhmen:

1.  **Siguria e të dhënave:** Skedari CSV nuk është i enkriptuar. Kushdo që ka qasje në folder mund t'i modifikojë manualisht rezervimet.
2.  **Konkurrenca (Concurrency):** Nëse dy përdorues tentojnë të shkruajnë në skedar në të njëjtën milisekondë, mund të ndodhë një konflikt (File Lock).
3.  **Mungesa e Database-it:** Për volume të mëdha të dhënash, kërkimi në skedar CSV bëhet i ngadaltë. Nevojitet kalimi në një sistem si SQL Server.
4.  **UI e limituar:** Ndërfaqja është vetëm tekstuale (Console). Për një përdorues real, do të ishte më e përshtatshme një ndërfaqe Web ose Desktop (WPF/WinForms).