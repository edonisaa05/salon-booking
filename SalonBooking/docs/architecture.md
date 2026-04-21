# Dokumentimi i Arkitekturës Teknike

## Ndarja në Shtresa (Layered Architecture)
Aplikacioni ndjek parimin e ndarjes së përgjegjësive (Separation of Concerns):

1. **Shtresa e UI (ConsoleUI)**: Komunikon me përdoruesin dhe trajton gabimet (Reliability).
2. **Shtresa e Shërbimit (AppointmentService)**: Përmban logjikën e biznesit dhe validimet.
3. **Shtresa e të Dhënave (FileRepository)**: Merret me ruajtjen e të dhënave në skedarin CSV.
4. **Shtresa e Modeleve (Models)**: Përcakton strukturën e të dhënave (Booking).

## Diagrami i Lidhshmërisë
[UI] ----> [Service] ----> [Interface] ----> [Repository]

## Pse kjo arkitekturë?
- **Maintainability**: Ndryshimi i mënyrës së ruajtjes (p.sh. nga CSV në SQL) nuk kërkon ndryshimin e UI.
- **Testability**: Përdorimi i Interfaces lejon testimin e shërbimit pa pasur nevojë për skedarë realë.
