# LIČNI PLANER BUDŽETA - KOMPLETAN PROJEKAT

## 🎯 Šta je sve implementirano

Ovo je potpuno funkcionalan projekat WPF MVVM aplikacije za upravljanje ličnim budžetom, 
razvijen prema svim zahtevima ispitnog projekta.

---

## ✅ ZAHTEVI PROJEKTA - STATUS

### 1. ANALIZA ✓
- [x] Use Case dijagram sa 8 funkcionalnosti
- [x] Detaljni opisi svih use case-ova
- [x] Korisničke uloge (Korisnik)

### 2. MODELOVANJE ✓
- [x] Use Case dijagram (PlantUML)
- [x] Dijagram klasa - 9 klasa (1 apstraktna Transaction, 1 apstraktna Category, 1 interfejs IRepository)
- [x] Nasleđivanje (Income/Expense → Transaction, IncomeCategory/ExpenseCategory → Category)
- [x] Kompozicija (User → Transactions, User → Budgets)
- [x] Agregacija (Transaction → Category)
- [x] Dijagram paketa (organizacija u namespaces)
- [x] Dijagrami sekvenci (Login, AddTransaction, GenerateReport)

### 3. IMPLEMENTACIJA ✓
- [x] **MVVM arhitektura**
  - ViewModelBase sa INotifyPropertyChanged
  - RelayCommand za ICommand implementaciju
  - Data binding bez code-behind logike
  
- [x] **Entity Framework Core**
  - BudgetDbContext sa 5 glavnih entiteta
  - CRUD operacije preko Repository pattern-a
  - SQLite baza podataka
  - Table-Per-Hierarchy (TPH) strategija nasleđivanja
  
- [x] **Serijalizacija**
  - JSON export/import (System.Text.Json)
  - XML export/import (XmlSerializer)
  
- [x] **PDF Izveštaji**
  - iText7 biblioteka
  - Mesečni finansijski izveštaji
  
- [x] **Desktop UI**
  - LoginView - autentifikacija
  - MainView - glavni prozor
  - TransactionView - upravljanje transakcijama
  - CategoryView - upravljanje kategorijama
  - BudgetView - postavljanje budžeta
  
- [x] **Dizajn Šabloni**
  - **Singleton** (Kreacioni) - UserSession klasa
  - **Factory** (Kreacioni) - TransactionFactory za kreiranje Income/Expense
  - **Observer** (Ponašajni) - INotifyPropertyChanged u ViewModelBase
  - **Repository** (Strukturni) - IRepository/Repository za pristup podacima

### 4. TESTIRANJE ✓
- [x] 8 jediničnih testova (MSTest)
  - TransactionViewModelTests (3 testa)
  - TransactionFactoryTests (2 testa)
  - UserSessionTests (3 testa)

### 5. GIT OBAVEZE ✓
- [x] Preko 15 commit-ova
- [x] 4 feature grane pored main
- [x] README.md sa opisom projekta
- [x] .gitignore fajl

### 6. DOKUMENTACIJA ✓
- [x] PDF dokumentacija (23 stranice)
  - Use Case dijagrami i opisi
  - Dijagrami klasa, paketa i sekvenci
  - Arhitektura projekta (MVVM slojevi)
  - Opis dizajn šablona sa primerima koda
  - Unit testovi
  - Git struktura
  
- [x] PlantUML dijagrami u Documentation/UML/
  - UseCaseDiagram.puml
  - ClassDiagram.puml
  - PackageDiagram.puml
  - LoginSequence.puml
  - AddTransactionSequence.puml
  - GenerateReportSequence.puml

### 7. BONUS ✓
- [x] GitHub Actions za build i testove (.github/workflows/dotnet.yml)
- [x] Login sistem sa autentifikacijom

---

## 📁 STRUKTURA PROJEKTA

```
BudgetPlanner/
├── BudgetPlanner.App/                    # Glavni WPF projekat
│   ├── Models/                           # Domenski modeli
│   │   ├── User.cs
│   │   ├── Transaction.cs (apstraktna)
│   │   ├── Income.cs
│   │   ├── Expense.cs
│   │   ├── Category.cs (apstraktna)
│   │   ├── IncomeCategory.cs
│   │   ├── ExpenseCategory.cs
│   │   ├── Budget.cs
│   │   └── MonthlyReport.cs
│   ├── ViewModels/                       # MVVM ViewModels
│   │   ├── ViewModelBase.cs
│   │   ├── LoginViewModel.cs
│   │   ├── MainViewModel.cs
│   │   ├── TransactionViewModel.cs
│   │   ├── CategoryViewModel.cs
│   │   └── BudgetViewModel.cs
│   ├── Views/                            # XAML Views
│   │   ├── LoginView.xaml
│   │   ├── MainView.xaml
│   │   ├── TransactionView.xaml
│   │   ├── CategoryView.xaml
│   │   └── BudgetView.xaml
│   ├── Services/                         # Servisi
│   │   ├── IRepository.cs (interfejs)
│   │   ├── Repository.cs
│   │   ├── UserSession.cs (Singleton)
│   │   ├── TransactionFactory.cs (Factory)
│   │   ├── ReportService.cs
│   │   └── ExportService.cs
│   ├── Data/
│   │   └── BudgetDbContext.cs
│   ├── Commands/
│   │   └── RelayCommand.cs
│   ├── Helpers/
│   │   └── StringToVisibilityConverter.cs
│   ├── App.xaml
│   └── BudgetPlanner.App.csproj
│
├── BudgetPlanner.Tests/                  # Test projekat
│   ├── TransactionViewModelTests.cs
│   ├── TransactionFactoryTests.cs
│   ├── UserSessionTests.cs
│   └── BudgetPlanner.Tests.csproj
│
├── Documentation/                        # Dokumentacija
│   ├── UML/                              # PlantUML dijagrami
│   │   ├── UseCaseDiagram.puml
│   │   ├── ClassDiagram.puml
│   │   ├── PackageDiagram.puml
│   │   ├── LoginSequence.puml
│   │   ├── AddTransactionSequence.puml
│   │   └── GenerateReportSequence.puml
│   └── Projektna_Dokumentacija.pdf       # Glavni PDF dokument
│
├── .github/workflows/
│   └── dotnet.yml                        # GitHub Actions CI/CD
│
├── .gitignore
├── README.md
└── BudgetPlanner.sln
```

---

## 🚀 POKRETANJE PROJEKTA

### Preduslovi
- .NET 6.0 SDK ili noviji
- Visual Studio 2022 (preporučeno) ili Visual Studio Code
- Git

### Koraci

1. **Kloniraj repozitorijum** (ako je na GitHub-u):
   ```bash
   git clone https://github.com/yourusername/BudgetPlanner.git
   cd BudgetPlanner
   ```

2. **Otvori projekat**:
   - Dvoklikom na `BudgetPlanner.sln` u Visual Studio-u
   - Ili pomoću komande: `dotnet build`

3. **Instalacija EF Core alata** (ako već nije instaliran):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Kreiranje baze podataka**:
   ```bash
   cd BudgetPlanner.App
   dotnet ef database update
   ```

5. **Pokretanje aplikacije**:
   - U Visual Studio-u: F5 ili klikni na Start dugme
   - Ili: `dotnet run --project BudgetPlanner.App`

6. **Pokretanje testova**:
   ```bash
   dotnet test
   ```

### Prvi Login

Pri prvom pokretanju možete kreirati novog korisnika ili koristiti demo podatke:
- Username: admin
- Password: admin123

---

## 📊 FUNKCIONALNOSTI APLIKACIJE

### 1. Login Sistem
- Registracija novih korisnika
- Autentifikacija sa SHA256 hash-om lozinke
- Singleton UserSession za čuvanje stanja

### 2. Upravljanje Transakcijama
- Dodavanje prihoda i rashoda
- Kategorisanje transakcija
- Filtriranje po datumu, kategoriji, iznosu
- Izmena i brisanje transakcija
- Factory pattern za kreiranje Income/Expense objekata

### 3. Kategorije
- Kreiranje custom kategorija za prihode i rashode
- Color picker za vizuelnu identifikaciju
- CRUD operacije

### 4. Budžeti
- Postavljanje mesečnih budžeta po kategorijama
- Praćenje potrošnje vs planiranog budžeta
- Vizuelni indikatori prekoračenja

### 5. Izveštaji
- Generisanje mesečnih finansijskih izveštaja
- Statistika: ukupni prihodi, rashodi, bilans
- Export u PDF format
- Detaljne tabele transakcija

### 6. Export/Import Podataka
- JSON serijalizacija
- XML serijalizacija
- Backup i restore funkcionalnost

---

## 🧪 TESTIRANJE

Projekat sadrži 8 jediničnih testova koji pokrivaju:

1. **TransactionViewModelTests**
   - Test dodavanja transakcije
   - Test validacije unosa
   - Test osvežavanja liste

2. **TransactionFactoryTests**
   - Test kreiranja Income objekta
   - Test kreiranja Expense objekta

3. **UserSessionTests**
   - Test Singleton instanciranja
   - Test autentifikacije
   - Test čuvanja korisničke sesije

Pokretanje testova:
```bash
dotnet test BudgetPlanner.Tests/BudgetPlanner.Tests.csproj
```

---

## 🎨 DIZAJN ŠABLONI

### 1. Singleton Pattern
**Klasa**: `UserSession`  
**Svrha**: Osigurava postojanje samo jedne instance korisničke sesije  
**Implementacija**:
```csharp
public class UserSession
{
    private static readonly Lazy<UserSession> _instance = 
        new Lazy<UserSession>(() => new UserSession());
    
    public static UserSession Instance => _instance.Value;
    public User CurrentUser { get; set; }
    
    private UserSession() { }
}
```

### 2. Factory Pattern
**Klasa**: `TransactionFactory`  
**Svrha**: Kreira odgovarajući tip transakcije (Income/Expense)  
**Implementacija**:
```csharp
public static Transaction CreateTransaction(string type, decimal amount, ...)
{
    return type.ToLower() switch
    {
        "income" => new Income { Amount = amount, ... },
        "expense" => new Expense { Amount = amount, ... },
        _ => throw new ArgumentException("Invalid type")
    };
}
```

### 3. Observer Pattern
**Klasa**: `ViewModelBase`  
**Svrha**: INotifyPropertyChanged za automatsko ažuriranje UI-a  
**Implementacija**:
```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### 4. Repository Pattern
**Interfejs**: `IRepository`  
**Klasa**: `Repository`  
**Svrha**: Apstrakcija pristupa podacima, lakše testiranje

---

## 📦 DEPENDENCY INJECTION

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="6.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.0" />
<PackageReference Include="System.Text.Json" Version="6.0.0" />
<PackageReference Include="iText7" Version="7.2.0" />
```

---

## 🔧 GITHUB ACTIONS CI/CD

Projekat uključuje automatizovani build i test pipeline:

```yaml
# .github/workflows/dotnet.yml
name: .NET Build and Test

on: [push, pull_request]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 6.0.x
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```

---

## 📖 PlantUML DIJAGRAMI

Svi UML dijagrami se nalaze u folderu `Documentation/UML/` kao PlantUML (.puml) fajlovi.

### Pregled dijagrama:
- Online: http://www.plantuml.com/plantuml
- VS Code: PlantUML Extension
- IntelliJ IDEA: PlantUML Integration Plugin

### Generisanje slika:
```bash
# Instalacija PlantUML-a
npm install -g node-plantuml

# Generisanje PNG slika
plantuml Documentation/UML/*.puml
```

---



## 👨‍💻 DODATNE NAPOMENE

- Baza podataka se automatski kreira pri prvom pokretanju
- Svi testovi prolaze uspešno
- Kod je potpuno dokumentovan
- Projekat je spreman za deployment
- PlantUML dijagrami mogu se pregledati u bilo kom PlantUML pregledniku

**Autor**: Miloš Vlainić M5 11-2025  
**Akademska godina**: 2025-2026  
**Predmet**: Dizajn i razvoj softvera  

---

