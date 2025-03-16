# prjCFLP
Se da o structura arborescenta pentru proiectarea unei constructii metalice.
Primul nivel este cel de proiect.
Proiectul poate sa contina mai multe obiecte.
Obiectul poate sa contine mai multe planse.
Plansa poate contine mai multe pozitii principale.
Pozitia principala poate contine mai multe pozitii secundare.
Pozitia secundara poate contine mai multe elemente concrete si anume:

- profile metalice de diverse tipuri (HEA10x100, HEB20x200, HEM10x120, IPE20x500, etc.) la care ne intereseaza:
 - formatul profilului: 3 litere mari, latime, "x", lungime
 - lungimea [cm] si latimea [cm] se vor extrage din numele profiului
 - numele profiulului nu are nicio relevanta pentru noi, se poate ignora
 - suprafata vopsita este 6 * latime * lungime

- flanse de tabla la care ne intereseaza:
 - lungime [mm], 
 - latime [mm], 
 - grosime [mm],
 - suprafata decupata (mp),
 - nr. fete vopsite (1 sau 2)
 - suprafata vopsita este latime * lungime * nr. fete - suprafata decupata

- elemente singulare (de ex. suruburi) la care ne intereseaza:
 - suprafata vopsita per bucata [mp]

Toate entitatile proiect, obiect, plansa, poz principala, poz sec au fiecare un nume unic la nivel global.

Sa se creeze facilitatile pentru a introduce un astfel de proiect in aplicatie.
Sa se exporte in format CSV proiectul.
Sa se calculeze suprafata care trebuie vopsita la nivelul fiecarei entitati (proiect, obiect, ...).
