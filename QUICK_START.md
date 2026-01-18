# ⚡ QUICK START - Lični Planer Budžeta


1. **Otvorite Solution**
   ```
   Dvoklikom na: BudgetPlanner.sln
   ```

2. **Build projekat**
   ```
   Visual Studio: Ctrl+Shift+B
   Ili: Build → Build Solution
   ```

3. **Pokrenite aplikaciju**
   ```
   F5 ili klik na Start dugme
   ```

4. **Login**
   - Kreirajte novog korisnika ili koristite:
     - Username: `admin`
     - Password: `admin123`

---

## 📋 Brza provera zahteva

### Use Case-ovi (Zahtev: min 6) ✅
Implementovano **8 use case-ova**:
1. Login/Registracija
2. Upravljanje kategorijama (Create, Update, Delete)
3. Dodavanje transakcija
4. Pregled i filtriranje transakcija
5. Postavljanje budžeta
6. Generisanje izveštaja
7. Export podataka (JSON, XML, PDF)
8. Import podataka

**Gde testirati**: Svaki tab u MainView (Transactions, Categories, Budgets)

---

### MVVM Arhitektura ✅

**ViewModels** (bez logike u code-behind):
- `ViewModels/ViewModelBase.cs` - INotifyPropertyChanged
- `ViewModels/TransactionViewModel.cs` - Binding i Commands
- `ViewModels/CategoryViewModel.cs`
- `ViewModels/BudgetViewModel.cs`
- `ViewModels/LoginViewModel.cs`
- `ViewModels/MainViewModel.cs`

**Views** (čist XAML sa data binding-om):
- `Views/TransactionView.xaml` - ItemsSource="{Binding Transactions}"
- `Views/CategoryView.xaml`
- `Views/BudgetView.xaml`
- `Views/LoginView.xaml`
- `Views/MainView.xaml`

**Commands**:
- `Commands/RelayCommand.cs` - ICommand implementacija

**Gde proveriti**: Otvorite bilo koji .xaml fajl → nema logike u .xaml.cs

---

### Entity Framework Core (Zahtev: min 3 entiteta) ✅

**Entiteti** (9 klasa):
- `Models/User.cs`
- `Models/Transaction.cs` ← **APSTRAKTNA KLASA**
- `Models/Income.cs` → nasleđuje Transaction
- `Models/Expense.cs` → nasleđuje Transaction
- `Models/Category.cs` ← **APSTRAKTNA KLASA**
- `Models/IncomeCategory.cs` → nasleđuje Category
- `Models/ExpenseCategory.cs` → nasleđuje Category
- `Models/Budget.cs`
- `Models/MonthlyReport.cs`

**DbContext**:
- `Data/BudgetDbContext.cs` - TPH strategija, relacije

**CRUD operacije**:
- `Services/Repository.cs` - Add, Update, Delete, GetAll, GetById

**Gde proveriti**: 
- `Data/BudgetDbContext.cs` → OnModelCreating() metoda
- `bin/Debug/net6.0/budget.db` → SQLite baza

---

### Dizajn Šabloni (Zahtev: 2 šablona) ✅

Implementovano **4 šablona**:

#### 1. **Singleton** (Kreacioni) ✅
**Fajl**: `Services/UserSession.cs`
```csharp
public static UserSession Instance => _instance.Value;
```
**Svrha**: Jedna instanca korisničke sesije

#### 2. **Factory** (Kreacioni) ✅
**Fajl**: `Services/TransactionFactory.cs`
```csharp
public static Transaction CreateTransaction(string type, ...)
{
    return type.ToLower() switch
    {
        "income" => new Income { ... },
        "expense" => new Expense { ... },
        ...
    };
}
```
**Svrha**: Kreiranje Income ili Expense na osnovu tipa

#### 3. **Observer** (Ponašajni) ✅
**Fajl**: `ViewModels/ViewModelBase.cs`
```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
```
**Svrha**: Auto-update UI-a pri promeni podataka

#### 4. **Repository** (Strukturni) ✅
**Fajl**: `Services/IRepository.cs` + `Services/Repository.cs`
**Svrha**: Apstrakcija pristupa podacima

**Gde proveriti**: Svaki fajl pojedinačno

---

### Serijalizacija (JSON + XML) ✅

**Fajl**: `Services/ExportService.cs`

**JSON**:
```csharp
JsonSerializer.Serialize(transactions, options);
```

**XML**:
```csharp
var serializer = new XmlSerializer(typeof(List<Transaction>));
```

**Gde testirati**: 
- Pokrenite aplikaciju
- Transactions tab → Add Transaction
- Kliknite "Export to JSON" ili "Export to XML"

---

### PDF Izveštaji ✅

**Fajl**: `Services/ExportService.cs`
**Biblioteka**: iText7

**Gde testirati**:
- Main tab → Select Month/Year
- Kliknite "Generate Report"
- Kliknite "Export to PDF"

---

### Jedinični Testovi (Zahtev: min 3 testa) ✅

**Test projekat**: `BudgetPlanner.Tests/`

**Testovi** (8 testova):
- `TransactionViewModelTests.cs` (3 testa)
- `TransactionFactoryTests.cs` (2 testa)
- `UserSessionTests.cs` (3 testa)

**Pokretanje**:
```bash
dotnet test
```
Ili u Visual Studio-u: Test Explorer (Ctrl+E, T)

**Gde proveriti**: Test Explorer → Run All Tests

---

### UML Dijagrami ✅

**Folder**: `Documentation/UML/`

**Dijagrami** (PlantUML format):
1. `UseCaseDiagram.puml` - 8 use case-ova
2. `ClassDiagram.puml` - 9 klasa, nasleđivanje, kompozicija
3. `PackageDiagram.puml` - Organizacija u pakete
4. `LoginSequence.puml` - Login sekvenca
5. `AddTransactionSequence.puml` - Dodavanje transakcije
6. `GenerateReportSequence.puml` - Generisanje izveštaja

**Pregled**:
- Online: http://www.plantuml.com/plantuml
- VS Code: PlantUML extension
- Ili otvorite u bilo kom text editoru

---

### Git Commit-ovi (Zahtev: 10-15) ✅

**Provera**:
```bash
git log --oneline
```

**Očekivano**: 20+ commit-ova

**Grane**:
- `main` - glavna grana
- `feature/ef-core-setup`
- `feature/viewmodels`
- `feature/serialization`
- `feature/testing`

---

### PDF Dokumentacija ✅

**Fajl**: `Documentation/Projektna_Dokumentacija.pdf`

**Sadržaj** (23 stranice):
1. Naslovlna strana
2. Sadržaj
3. Uvod i tehnologije
4. Analiza (Use Case opisi)
5. Modelovanje (Class, Package, Sequence dijagrami)
6. Implementacija (MVVM, EF Core, Design Patterns)
7. Testiranje
8. Git struktura
9. Zaključak

**Gde proveriti**: Otvorite PDF u bilo kom pregledniku

---

### Bonus Funkcionalnosti ✅

- ✅ **Login sistem**: `Views/LoginView.xaml`


---
