________________________________________
Dokumentacja techniczna – Aplikacja WPF: Plan lekcji
________________________________________
1. Opis projektu
Aplikacja desktopowa stworzona w technologii WPF (Windows Presentation Foundation) służy do tworzenia,
przeglądania i edytowania tygodniowego planu lekcji.
Zawiera przejrzysty interfejs użytkownika oraz funkcje pozwalające na zarządzanie lekcjami w ramach dni roboczych
i ustalonych godzin szkolnych. Dane są trwale zapisywane w pliku tekstowym i wczytywane automatycznie przy każdym uruchomieniu programu.
________________________________________
2. Funkcjonalności

•	Wyświetlanie planu lekcji od poniedziałku do piątku, z podziałem na 11 godzin lekcyjnych.

•	Możliwość dodawania, edytowania i usuwania lekcji z poziomu interfejsu.

•	Lekcja zawiera:

o	nazwę przedmiotu,

o	numer sali.

•	Automatyczny zapis i odczyt danych z pliku plan_lekcji.txt.

•	Zachowanie danych pomiędzy uruchomieniami aplikacji.

•	Intuicyjny i estetyczny interfejs graficzny.
________________________________________
3. Proces tworzenia aplikacji

Etap 1 – Interfejs użytkownika (XAML)

•	Zaprojektowano siatkę (Grid) z 5 kolumnami (dni tygodnia: Poniedziałek–Piątek) i 11 wierszami (godziny lekcyjne).

•	Każde pole reprezentujące konkretną lekcję (dzień + godzina) zostało zaimplementowane jako przycisk (Button), co umożliwia interakcję.

•	Do przycisków przypisano tagi z identyfikatorem (dzień|numer_lekcji) umożliwiające rozpoznanie klikniętego pola.



Etap 2 – Logika aplikacji (C# - MainWindow.xaml.cs)

•	Utworzono klasę reprezentującą lekcję z polami: Nazwa, Sala, Dzień, Godzina.

•	Dane przechowywane są w słowniku Dictionary<(int dzien, int lekcja), Lesson>.

•	Po uruchomieniu aplikacji dane są wczytywane z pliku, a każdemu przyciskowi przypisywany jest tekst odpowiadający danej lekcji (jeśli istnieje).

•	Po kliknięciu przycisku otwierane jest okno dialogowe, które umożliwia:

o	dodanie nowej lekcji,

o	edycję istniejącej,

o	usunięcie lekcji.

•	Po każdej zmianie (dodaniu, edycji, usunięciu), dane są aktualizowane w interfejsie oraz natychmiast zapisywane do pliku.
________________________________________
4. Format pliku plan_lekcji.txt

Każda linia pliku reprezentuje jedną lekcję w formacie:

<dzień>|<numer_lekcji>|<nazwa_przedmiotu>|<numer_sali>

Przykład:

1|3|Matematyka|101

– Wtorek, lekcja nr 3 (czyli 9:40 – 10:25), przedmiot: Matematyka, sala: 101.
________________________________________
5. Godziny lekcyjne

Nr	Godzina

0	7:10 – 7:55

1	8:00 – 8:45

2	8:50 – 9:35

3	9:40 – 10:25

4	10:35 – 11:20

5	11:30 – 12:15

6	12:30 – 13:15

7	13:25 – 14:10

8	14:20 – 15:05

9	15:10 – 15:55

10	16:00 – 16:45
________________________________________
6. Dni tygodnia
7. 
Kod	Dzień tygodnia

0	Poniedziałek

1	Wtorek

2	Środa

3	Czwartek

4	Piątek
________________________________________
7. Działanie aplikacji (krok po kroku)

1.	Start programu

o	Program sprawdza istnienie pliku plan_lekcji.txt. Jeśli nie istnieje – tworzy pusty.

o	Następnie wczytuje dane i wyświetla je w odpowiednich polach siatki.

3.	Kliknięcie w pole planu

o	Otwiera się okno dialogowe z informacjami o danej lekcji (jeśli istnieje).

o	Użytkownik może:

	zmodyfikować wpis,

	usunąć lekcję,

	lub dodać nową.

5.	Zapis danych

o	Po każdej operacji dane są zapisywane do pliku plan_lekcji.txt.

o	Format pliku pozwala na prosty i szybki odczyt oraz zapis bez użycia baz danych.

7.	Zamknięcie i ponowne otwarcie programu

o	Wczytywane są ostatnie zapisane dane, dzięki czemu plan pozostaje niezmieniony.
________________________________________
8. Zabezpieczenia i uproszczenia

•	Program nie wymaga instalacji dodatkowych bibliotek.

•	Wszystkie dane przechowywane są lokalnie – nie wymaga internetu ani serwera.

•	Nie występuje ryzyko utraty danych w przypadku zamknięcia programu – zmiany są zapisywane na bieżąco.
________________________________________
9. Potencjalna rozbudowa

•	Dodanie nazw nauczycieli.

•	Obsługa planów wielu klas lub użytkowników.

•	Eksport planu do PDF.

•	Dodanie przycisku „Drukuj”.

•	Integracja z kalendarzem lub przypomnieniami.
________________________________________
10. Informacje końcowe

Autor: Maciej Strzelec 2P

Data wykonania: Czerwiec 2025

Projekt stworzony w ramach: Plan Lekcji projekt na 6

Użyte pomoce: YouTube, Forum C#, ChatGPT oraz wiedza własna przedewszystkim
________________________________________
