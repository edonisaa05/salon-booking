# Dokumentimi i Arkitekturës

Për këtë projekt është zgjedhur **Arkitektura e Shtresëzuar** (N-Tier Architecture) për të garantuar që kodi të jetë i pastër dhe i lehtë për t'u testuar.

### Shtresat:
1. **Domain/Models**: Përmban definicionet e objekteve (Entities).
2. **Data Access**: Përdor *Repository Pattern* për të izoluar punën me skedarët CSV.
3. **Business Logic (Services)**: Këtu ndodh përpunimi i të dhënave para se të shkojnë në UI.
4. **Presentation (UI)**: Vetëm shfaqja e informatave për përdoruesin.

### Pse Repository Pattern?
Ky model na lejon që në të ardhmen ta ndërrojmë mënyrën e ruajtjes së të dhënave (p.sh. nga CSV në SQL) pa pasur nevojë të ndryshojmë asnjë rresht kod në pjesën e UI apo të Shërbimeve.