# 💰 Lični Planer Budžeta

WPF MVVM aplikacija za upravljanje ličnim finansijama, razvijena uz upotrebu Entity Framework Core i primenu dizajn šablona.

## 📋 Opis Projekta

Aplikacija za praćenje prihoda i rashoda, postavljanje budžeta po kategorijama, i generisanje mesečnih finansijskih izveštaja.

## ✨ Funkcionalnosti

- ✅ **Autentifikacija**: Registracija i login korisnika
- ✅ **Upravljanje Kategorijama**: Kreiranje, izmena i brisanje kategorija prihoda/rashoda
- ✅ **Transakcije**: Dodavanje, izmena i brisanje finansijskih transakcija
- ✅ **Pregled i Filtriranje**: Pretraga transakcija po datumu, kategoriji, i iznosu
- ✅ **Budžet**: Postavljanje mesečnih budžeta po kategorijama
- ✅ **Izveštaji**: Generisanje PDF izveštaja o prihodima i rashodima
- ✅ **Export/Import**: Serijalizacija podataka u JSON i XML format
- ✅ **Statistika**: Vizualizacija finansijskih podataka

## 🛠️ Tehnologije

- **Framework**: .NET 6.0
- **UI**: WPF (Windows Presentation Foundation)
- **Arhitektura**: MVVM (Model-View-ViewModel)
- **ORM**: Entity Framework Core 6.0
- **Baza**: SQLite
- **Testiranje**: xUnit
- **Serijalizacija**: System.Text.Json, XmlSerializer

## 🏗️ Arhitektura

### Struktura Projekta

```
BudgetPlanner/
├── BudgetPlanner.App/          # Glavni WPF projekat
│   ├── Models/                 # Entiteti i domenski modeli
│   ├── ViewModels/             # MVVM ViewModels
│   ├── Views/                  # XAML views
│   ├── Services/               # Servisi (Repository, Export, Report)
│   ├── Data/                   # DbContext i konfiguracije
│   ├── Commands/               # ICommand implementacije
│   └── Helpers/                # Helper klase
├── BudgetPlanner.Tests/        # Jedinični testovi
└── Documentation/              # UML dijagrami i dokumentacija
```

### Entiteti

1. **User** - Korisnik aplikacije
2. **Category** (apstraktna) - Bazna klasa za kategorije
3. **IncomeCategory** - Kategorija prihoda
4. **ExpenseCategory** - Kategorija rashoda
5. **Transaction** (apstraktna) - Bazna klasa za transakcije
6. **Income** - Prihod
7. **Expense** - Rashod
8. **Budget** - Budžet po kategoriji
9. **MonthlyReport** - Mesečni izveštaj

### Dizajn Šabloni

#### Kreacioni Šabloni
- **Singleton**: `DatabaseContext`, `UserSession` - obezbeđuje jednu instancu kroz celu aplikaciju

#### Strukturni Šabloni
- **Repository**: `IRepository<T>`, `Repository<T>` - apstrakcija data access layer-a

#### Ponašajni Šabloni
- **Command**: `RelayCommand` - implementacija ICommand za MVVM binding
- **Factory Method**: `TransactionFactory` - kreiranje različitih tipova transakcija

## 🚀 Pokretanje Aplikacije

### Preduslov

- .NET 6.0 SDK ili noviji
- Visual Studio 2022 ili Rider

### Instalacija

1. Klonirajte repozitorijum:
```bash
git clone https://github.com/your-username/BudgetPlanner.git
cd BudgetPlanner
```

2. Restore NuGet paketa:
```bash
dotnet restore
```

3. Pokrenite aplikaciju:
```bash
dotnet run --project BudgetPlanner.App
```

### Pokretanje Testova

```bash
dotnet test
```

## 📊 Use Case Dijagrami

Pogledajte `Documentation/UML/` folder za kompletne UML dijagrame:
- Use Case dijagram
- Class dijagram
- Sequence dijagrami
- Package dijagram

## 📖 Dokumentacija

Kompletna projektna dokumentacija dostupna je u `Documentation/ProjectDocumentation.pdf` i uključuje:
- Analizu zahteva
- UML dijagrame
- Opis arhitekture
- Screenshot-ove aplikacije
- Opis primenjenih dizajn šablona

## 🧪 Testiranje

Projekat sadrži jedinične testove za:
- ViewModels (transakcije, kategorije, budžet)
- Servise (repository, export)
- Validaciju podataka

## 📦 Export/Import Podataka

Aplikacija podržava:
- **JSON**: Export svih transakcija i kategorija
- **XML**: Backup kompletne baze podataka
- **PDF**: Generisanje mesečnih i godišnjih izveštaja

## 👨‍💻 Autor

Miloš Vlainić M5 11-2025

## 📝 Licenca

Obrazovni projekat - slobodno korišćenje u edukativne svrhe.
