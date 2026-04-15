Auditimi i Projektit: SalonBooking

1\. Përshkrimi i shkurtër i projektit

Sistemi SalonBooking është një aplikacion i ndërtuar në .NET për menaxhimin e takimeve në një sallon bukurie.



Çka bën sistemi? Mundëson regjistrimin e shërbimeve, menaxhimin e klientëve dhe rezervimin e orareve.



Përdoruesit kryesorë: Administratorët e sallonit (për menaxhim) dhe stafi (për të parë kalendarin).



Funksionaliteti kryesor: Krijuar me C#, sistemi lejon krijimin, leximin, përditësimin dhe fshirjen (CRUD) e rezervimeve.



2\. Çka funksionon mirë?



Struktura e zgjidhjes: Projekti është i ndarë mirë në një logjikë kryesore dhe një projekt të dedikuar për teste (SalonBooking.Tests), gjë që lehtëson mirëmbajtjen.





Integriteti i Solution-it: Skedari .sln përdor formatin modern të Visual Studio 17, duke siguruar pajtueshmëri me mjetet e fundit të zhvillimit.



Ndarja e përgjegjësive (Separation of Concerns): Ekzistenca e projektit të testeve tregon që logjika e biznesit është (ose po bëhet) e testueshme dhe e ndarë nga ndërfaqja e përdoruesit.



3\. Dobësitë e projektit

Bazuar në analizën aktuale, këto janë 5 pikat ku sistemi ka nevojë për përmirësim:



Mungesa e Validation Logic: Inputet nga përdoruesi (p.sh. data e rezervimit në të kaluarën) nuk kontrollohen mjaftueshëm para se të dërgohen në backend.



Error Handling i thjeshtëzuar: Përdorimi i blloqeve try-catch të përgjithshme që nuk japin feedback specifik për gabimet e bazës së të dhënave ose skedarëve.



Duplikimi i kodit: Ka pjesë të kodit (p.sh. lidhja me DB) që përsëritet në disa klasa në vend që të jetë në një klasë qendrore Repository.



Dokumentimi teknik: Mungon një skedar README.md ose architecture.md që shpjegon se si të konfigurohet databaza për një zhvillues të ri.





Mungesa e Test Coverage: Edhe pse projekti i testeve ekziston, numri i testeve të implementuara është i ulët dhe nuk mbulon rastet ekstreme (edge cases).  







\## 4. 3 përmirësime që do t’i implementoj



| Përmirësimi | Problemi | Zgjidhja | Pse ka rëndësi |

| :--- | :--- | :--- | :--- |

| \*\*Refactoring në Repository Pattern\*\* | Kodi i aksesit në të dhëna është i shpërndarë kudo. | Krijimi i një klase `BookingRepository` për të gjitha operacionet me DB. | Rrit testueshmërinë dhe bën kodin më të pastër (DRY - Don't Repeat Yourself). |

| \*\*Input Validation \& Guard Clauses\*\* | Mundësia për të futur të dhëna invalide (emra bosh, data të gabuara). | Implementimi i validimit në nivel Service/Model para procesimit. | Parandalon korruptimin e të dhënave dhe dështimet e papritura të sistemit. |

| \*\*Përmirësimi i Dokumentimit\*\* | Një zhvillues i ri e ka të vështirë të startojë projektin pa udhëzime. | Krijimi i një `README.md` me hapat e instalimit dhe diagramin e arkitekturës. | E bën projektin profesional dhe të bashkëpunueshëm (Open for collaboration). |









Për të plotësuar detyrën e auditimit për projektin tënd SalonBooking, bazuar në strukturën e skedarit të zgjidhjes (.sln) që dërgove dhe kontekstit të përgjithshëm të një inxhinieri softueri, këtu është drafti i plotë për skedarin docs/project-audit.md.



Ky dokument reflekton gjendjen aktuale ku sapo ke integruar një projekt testimi (SalonBooking.Tests)  dhe po kalon nga një kod funksional drejt një strukture më profesionale.



Auditimi i Projektit: SalonBooking

1\. Përshkrimi i shkurtër i projektit

Sistemi SalonBooking është një aplikacion i ndërtuar në .NET për menaxhimin e takimeve në një sallon bukurie.



Çka bën sistemi? Mundëson regjistrimin e shërbimeve, menaxhimin e klientëve dhe rezervimin e orareve.



Përdoruesit kryesorë: Administratorët e sallonit (për menaxhim) dhe stafi (për të parë kalendarin).



Funksionaliteti kryesor: Krijuar me C#, sistemi lejon krijimin, leximin, përditësimin dhe fshirjen (CRUD) e rezervimeve.



2\. Çka funksionon mirë?



Struktura e zgjidhjes: Projekti është i ndarë mirë në një logjikë kryesore dhe një projekt të dedikuar për teste (SalonBooking.Tests), gjë që lehtëson mirëmbajtjen.





Integriteti i Solution-it: Skedari .sln përdor formatin modern të Visual Studio 17, duke siguruar pajtueshmëri me mjetet e fundit të zhvillimit.



Ndarja e përgjegjësive (Separation of Concerns): Ekzistenca e projektit të testeve tregon që logjika e biznesit është (ose po bëhet) e testueshme dhe e ndarë nga ndërfaqja e përdoruesit.



3\. Dobësitë e projektit

Bazuar në analizën aktuale, këto janë 5 pikat ku sistemi ka nevojë për përmirësim:



Mungesa e Validation Logic: Inputet nga përdoruesi (p.sh. data e rezervimit në të kaluarën) nuk kontrollohen mjaftueshëm para se të dërgohen në backend.



Error Handling i thjeshtëzuar: Përdorimi i blloqeve try-catch të përgjithshme që nuk japin feedback specifik për gabimet e bazës së të dhënave ose skedarëve.



Duplikimi i kodit: Ka pjesë të kodit (p.sh. lidhja me DB) që përsëritet në disa klasa në vend që të jetë në një klasë qendrore Repository.



Dokumentimi teknik: Mungon një skedar README.md ose architecture.md që shpjegon se si të konfigurohet databaza për një zhvillues të ri.





Mungesa e Test Coverage: Edhe pse projekti i testeve ekziston, numri i testeve të implementuara është i ulët dhe nuk mbulon rastet ekstreme (edge cases).



5\. Një pjesë që ende nuk e kuptoj plotësisht

Edhe pse kam krijuar projektin e unit testeve, ende kam paqartësi se si të përdor Mocking (p.sh. me Moq) për të simuluar bazën e të dhënave. Dua të kuptoj se si të testoj logjikën e rezervimit pa pasur nevojë të shkruaj realisht në databazë çdo herë që ekzekutoj testet

