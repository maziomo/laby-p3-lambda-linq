# Programowanie 3 C# · Laboratorium 05

## Raport sklepu papierniczego · 20 pkt

Sklep przygotowuje raport za październik 2026. Wybierz zamówienia z tego okresu, policz zamówienia przypadające na klienta, a następnie przygotuj trzy zestawienia LINQ. Prowadzący może uruchomić ten sam kod z innymi danymi i dla innego okresu.

Ćwiczenie łączy `Filter` i `Collect` z TASK 2 oraz trzy raporty sklepowe z TASK 3 (punkty 1, 4 i 5). Dane, kontrakty metod i testy są przygotowane dla tego zadania. [Materiały MiNI PW do laboratorium 05](https://csharp.mini.pw.edu.pl/pl/labs/05.lambdas-enumerable-linq-extensions/)

## Dane wejściowe

| Typ | Znaczenie |
| --- | --- |
| `Customer(Id, FirstName, LastName)` | Klient sklepu |
| `LoyaltyCard(CustomerId, DiscountRate)` | Karta klienta. Stawka z przedziału 0–1, np. `0.15m` oznacza 15% |
| `Product(Id, Name, UnitPrice)` | Produkt i cena jednostkowa w PLN, typu `decimal` |
| `Order(Id, CustomerId, PlacedOn)` | Zamówienie klienta z datą typu `DateOnly` |
| `OrderLine(OrderId, ProductId, Quantity)` | Pozycja zamówienia z liczbą sztuk typu `int` |

Identyfikatory są unikatowe w każdej z tabel klientów, produktów i zamówień. Klient ma najwyżej jedną kartę. Cena jest nieujemna, a liczba sztuk dodatnia. Zamówienie może mieć zero lub więcej pozycji. Każdy wiersz pozycji liczy się osobno, również gdy ten sam produkt pojawia się w tym samym zamówieniu kilka razy.

Karty i zamówienia wskazują klientów, a pozycje wskazują istniejące produkty i zamówienia z pełnego zbioru. W raportach okresowych `orders` zawiera już wybrane zamówienia, natomiast `lines` może nadal zawierać pozycje ze wszystkich zamówień. Używaj identyfikatorów do łączenia tabel. Kolekcje mogą być puste i mieć dowolną kolejność. Możesz założyć, że argumenty typu referencyjnego przekazane do metod są różne od `null`.

## Zasady

- Wypełnij metody oznaczone `STAGE01`–`STAGE05` w trzech plikach wskazanych poniżej. Zachowaj ich publiczne sygnatury i przestrzeń nazw `RaportSklepu`. Możesz dodać prywatne metody pomocnicze.
- W etapach 01–02 zaimplementuj przetwarzanie samodzielnie. Użyj delegatów i lambd. W `Filter` zastosuj `yield return`.
- W etapach 03–05 zapisz raporty za pomocą LINQ. Składnia metod i składnia zapytań są dozwolone. Możesz podzielić raport na kilka zapytań. Zwróć zmaterializowane listy wynikowe.
- Nie zmieniaj wejściowych danych, programu demonstracyjnego, modeli ani testów. Wyjątkiem jest kontener przekazany do `Collect`, który ma być modyfikowany przez akcję akumulującą.
- Testy sprawdzają zachowanie. Punktacja obejmuje także techniki wskazane w poleceniu, w tym ponowne użycie `Collect`.

## STAGE01 · Wybór okresu (4 pkt)

Pliki `EnumerableExtensions.cs` i `OrderSelection.cs`.

1. **`Filter<T>` (2 pkt).** Uzupełnij metodę rozszerzającą `IEnumerable<T>`, która przyjmuje `Predicate<T>`. Zwracaj elementy spełniające warunek, zachowując kolejność i powtórzenia. Samo wywołanie `Filter` nie czyta źródła i nie wywołuje predykatu. Pobieraj elementy dopiero w trakcie enumeracji i tylko w miarę potrzeby odbiorcy. Każde ponowne przejście przez wynik, jeśli źródło na to pozwala, ma korzystać z bieżącej zawartości źródła.
2. **`CreatePeriodFilter` (2 pkt).** Zwróć lambdę typu `Predicate<Order>`, która przechwytuje obie granice okresu. Data zamówienia ma należeć do przedziału `[fromInclusive, toExclusive)`. Równe granice dają pusty okres. Gdy koniec jest wcześniejszy niż początek, rzuć `ArgumentOutOfRangeException` dla parametru `toExclusive` już przy tworzeniu predykatu.

## STAGE02 · Licznik zamówień (3 pkt)

Pliki `EnumerableExtensions.cs` i `OrderSelection.cs`.

1. **`Collect<TSource, TAccumulate>` (1 pkt).** Przejdź po źródle dokładnie raz. Dla każdego elementu, w kolejności źródła, wywołaj `Action<TAccumulate, TSource>` z tym samym kontenerem i bieżącym elementem. Zwróć tę samą instancję kontenera, także dla pustego źródła.
2. **`CountOrdersByCustomer` (2 pkt).** Użyj `Collect` i lambdy, która aktualizuje słownik. Kluczem wyniku jest `CustomerId`, a wartością liczba jego zamówień z przekazanej sekwencji. Licz każde zamówienie, także zamówienie bez pozycji. Zwracaj tylko klientów występujących w sekwencji. Puste wejście daje pusty słownik. Kolejność wpisów jest dowolna.

## STAGE03 · Klienci bez karty (3 pkt)

Plik `ShopReports.cs`, metoda `CustomersWithoutCard`.

Zwróć klientów, których identyfikator nie występuje wśród kart. Karta z rabatem 0% nadal jest kartą. Sortuj rosnąco po nazwisku, imieniu, a następnie identyfikatorze. Do obu porównań tekstowych użyj `StringComparer.Ordinal`. Ten raport obejmuje wszystkich klientów sklepu niezależnie od okresu.

Punktacja obejmuje wybór klientów według identyfikatora (2 pkt) oraz kolejność wyniku (1 pkt).

## STAGE04 · Ranking produktów (4 pkt)

Plik `ShopReports.cs`, metoda `TopProducts`.

Zsumuj liczbę sztuk każdego produktu w pozycjach przypisanych do przekazanych zamówień. Zwróć wiersze `ProductSales(ProductId, Name, UnitsSold)`. Pobierz nazwę z tabeli produktów. `UnitsSold` jest typu `long`, więc sumuj ilości jako `long`. Uwzględnij wyłącznie produkty z co najmniej jedną pasującą pozycją.

Sortuj malejąco według sumy sztuk, a przy remisie rosnąco według identyfikatora produktu. Po sortowaniu ogranicz wynik do `limit` pozycji (domyślnie 5). Limit 0 daje pustą listę, a ujemny limit wymaga `ArgumentOutOfRangeException` dla parametru `limit`.

Punktacja obejmuje łączenie po identyfikatorach i sumowanie (2 pkt), sortowanie (1 pkt) oraz obsługę limitu (1 pkt).

## STAGE05 · Wydatki po rabacie (6 pkt)

Plik `ShopReports.cs`, metoda `SpendingByCustomer`.

Dla każdego klienta zsumuj wartości pozycji ze wszystkich jego zamówień z przekazanej sekwencji. Wartość pozycji to liczba sztuk razy cena jednostkowa. Od sumy odejmij rabat wynikający z karty klienta. Klient bez karty ma rabat 0%. Obliczenia wykonuj w `decimal`. Zaokrąglij raz, dopiero po zsumowaniu pozycji i naliczeniu rabatu, do dwóch miejsc po przecinku z użyciem `MidpointRounding.AwayFromZero`.

Zwróć po jednym wierszu `CustomerSpending(CustomerId, FirstName, LastName, TotalPaid)` dla **każdego** klienta. Jeśli w okresie nie ma dla niego pasujących pozycji, wpisz 0 PLN. Sortuj jak w STAGE03, czyli po nazwisku, imieniu i identyfikatorze.

Punktacja obejmuje łączenie danych i obliczanie sumy pozycji (2 pkt), rabat i zaokrąglenie (2 pkt), uwzględnienie wszystkich klientów (1 pkt) oraz kolejność wyniku (1 pkt).

## Przykład dla dołączonych danych

Okres demonstracyjny obejmuje daty od 1 października do 31 października 2026 włącznie.

| Etap | Oczekiwany wynik w demonstracji |
| --- | --- |
| STAGE01 | Identyfikatory `1004, 1001, 1002, 1003, 1006` w kolejności źródła |
| STAGE02 | Klient 101 ma 1 zamówienie, 102 ma 2, a 103 ma 2 |
| STAGE03 | Jan Nowak, Ola Zielińska |
| STAGE04 | Notes 4 szt., Pióro 3 szt., Teczka 3 szt. |
| STAGE05 | Adam Kowal 0,00 PLN, Anna Kowal 36,00 PLN, Ewa Nowak 7,50 PLN, Jan Nowak 28,25 PLN, Ola Zielińska 0,00 PLN |

## Struktura

```text
MiniLab05-RaportSklepu/
├── README.md
├── RaportSklepu.sln
├── src/RaportSklepu/
│   ├── RaportSklepu.csproj
│   ├── Models.cs
│   ├── SampleData.cs
│   ├── Program.cs
│   ├── EnumerableExtensions.cs
│   ├── OrderSelection.cs
│   └── ShopReports.cs
└── tests/RaportSklepu.Tests/
    ├── RaportSklepu.Tests.csproj
    ├── Stage01Tests.cs … Stage05Tests.cs
    ├── IntegrationTests.cs
    └── TestSettings.cs
```

Projekt aplikacji ma modele, dane demonstracyjne i siedem metod do uzupełnienia. Osobny projekt MSTest odwołuje się do aplikacji przez `ProjectReference`. Testy etapowe można uruchamiać oddzielnie. Test integracyjny sprawdza pełny przepływ od wyboru zamówień do raportów. W programie demonstracyjnym etapy 02, 04 i 05 korzystają z wyboru okresu z etapu 01.

## Uruchomienie przez dotnet

Zainstaluj **SDK .NET 10** i rozpakuj cały ZIP. W terminalu przejdź do katalogu z `RaportSklepu.sln`. Przy pierwszym przywracaniu pakietów testowych potrzebny jest dostęp do NuGet albo wcześniej pobrane pakiety.

```sh
dotnet --list-sdks
dotnet restore RaportSklepu.sln
dotnet build RaportSklepu.sln --no-restore
dotnet test RaportSklepu.sln --no-build
```

Na początku kompilacja przechodzi, a testy kończą się niepowodzeniem, ponieważ metody oznaczone numerami etapów rzucają `NotImplementedException`. Po zmianach użyj poniższej komendy bez `--no-build`, żeby przebudować kod i sprawdzić jeden etap. Zmień numer kategorii dla kolejnych etapów.

```sh
dotnet test RaportSklepu.sln --filter "TestCategory=STAGE01"
dotnet run --project src/RaportSklepu/RaportSklepu.csproj
```

Na końcu uruchom `dotnet test RaportSklepu.sln`, aby sprawdzić także integrację.

## Uruchomienie w Riderze

1. Otwórz `RaportSklepu.sln` i poczekaj na przywrócenie pakietów NuGet. Jeśli Rider zgłasza brak SDK, sprawdź w ustawieniach **Toolset and Build**, czy korzysta z instalacji .NET 10.
2. Zbuduj rozwiązanie. Otwórz **View → Tool Windows → Unit Tests**. Uruchom wybraną klasę, np. `Stage01Tests`, albo cały projekt `RaportSklepu.Tests`. Możesz też użyć ikony uruchomienia obok klasy lub metody testowej w edytorze.
3. Do demonstracji uruchom projekt konsolowy `RaportSklepu`. Porównaj jego wynik z tabelą przykładu.

[Dokumentacja okna Unit Tests w Riderze](https://www.jetbrains.com/help/rider/Unit_Testing_in_Solution.html)
