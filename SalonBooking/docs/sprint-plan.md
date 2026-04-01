# Sprint 2 Plan — [Edonisa Osmani]
Data: 1 Prill 2026

## Gjendja Aktuale
- **Çka funksionon tani?**
  - Arkitektura N-Tier (UI -> Service -> Repository) është e rregulluar.
  - Leximi dhe shkrimi në skedarin CSV (`appointments.csv`) funksionon.
  - Menuja interaktive në Console lejon shtimin dhe listimin e takimeve.
  - Funksionet bazë CRUD (Create, Read, Update, Delete) janë aktive.

- **Çka nuk funksionon?**
  - Nuk ka mbrojtje nëse përdoruesi shkruan shkronja në vend të numrave (ID).
  - Programi mund të crash-ojë nëse mungon folderi `Data` ose skedari CSV.
  - Nuk ka një opsion për të kërkuar takimet sipas emrit të klientit.

- **A kompajlohet dhe ekzekutohet programi?**
  - Po.

## Plani i Sprintit

### Feature e Re (Kërkim i takimeve sipas emrit)
- **Përshkrimi:** Do të shtoj një funksion të ri ku përdoruesi shkruan emrin e klientit dhe programi filtron e shfaq vetëm takimet e atij personi.
- **Përdorimi:** Useri zgjedh opsionin e ri në menu, jep emrin, dhe Service-i kthen listën e filtruar.

### Error Handling (Mbrojtja nga crash)
- **Rasti 1 (File missing):** Do të shtoj try-catch te Repository që nëse skedari mungon, ta krijojë automatikisht me header-at e duhur në vend që të jep error.
- **Rasti 2 (Invalid Input):** Te UI do të përdor `int.TryParse` që nëse useri shkruan tekst te ID-ja, t'i dalë mesazhi "Please enter a valid number".
- **Rasti 3 (Null Reference):** Nëse kërkohet një ID që nuk ekziston, Service do të kthejë një mesazh "Appointment not found" në vend që të ndalet programi.

### Teste (Unit Tests me xUnit)
- Do të testoj metodën e kërkimit (`SearchByName`):
  1. **Rast normal:** Kërkimi i një emri që ekziston duhet të kthejë takimin e saktë.
  2. **Rast kufitar:** Kërkimi me emër bosh ose emër që nuk ekziston duhet të kthejë një listë boshe (jo null crash).
  3. **Rast validimi:** Testimi i metodës së krijimit që nuk lejon emra me më pak se 2 shkronja.

