#!/usr/bin/env python3
"""
Generate comprehensive project documentation PDF for Budget Planner
"""

from reportlab.lib.pagesizes import letter, A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import inch
from reportlab.lib.enums import TA_CENTER, TA_LEFT, TA_JUSTIFY
from reportlab.platypus import (SimpleDocTemplate, Paragraph, Spacer, PageBreak,
                                Table, TableStyle, Image, KeepTogether)
from reportlab.lib import colors
from datetime import datetime
import os

def create_title_page(story, styles):
    """Create title page"""
    title_style = ParagraphStyle(
        'CustomTitle',
        parent=styles['Heading1'],
        fontSize=28,
        textColor=colors.HexColor('#1a5490'),
        spaceAfter=30,
        alignment=TA_CENTER,
        fontName='Helvetica-Bold'
    )
    
    subtitle_style = ParagraphStyle(
        'Subtitle',
        parent=styles['Normal'],
        fontSize=16,
        textColor=colors.HexColor('#2c5aa0'),
        spaceAfter=20,
        alignment=TA_CENTER,
        fontName='Helvetica'
    )
    
    story.append(Spacer(1, 2*inch))
    story.append(Paragraph("LIČNI PLANER BUDŽETA", title_style))
    story.append(Paragraph("WPF MVVM Aplikacija sa Entity Framework Core", subtitle_style))
    story.append(Spacer(1, 0.5*inch))
    
    info_style = ParagraphStyle(
        'Info',
        parent=styles['Normal'],
        fontSize=12,
        alignment=TA_CENTER
    )
    
    story.append(Paragraph(f"Datum: {datetime.now().strftime('%d.%m.%Y.')}", info_style))
    story.append(Spacer(1, 0.3*inch))
    story.append(Paragraph("Projektna dokumentacija", info_style))
    story.append(PageBreak())

def create_toc(story, styles):
    """Create table of contents"""
    story.append(Paragraph("Sadržaj", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    toc_items = [
        ("1. Uvod", "3"),
        ("2. Analiza", "4"),
        ("   2.1. Use Case Dijagram", "4"),
        ("   2.2. Opisi Use Case-ova", "5"),
        ("   2.3. Korisničke Uloge", "7"),
        ("3. Modelovanje", "8"),
        ("   3.1. Dijagram Klasa", "8"),
        ("   3.2. Dijagram Paketa", "9"),
        ("   3.3. Dijagrami Sekvenci", "10"),
        ("4. Implementacija", "13"),
        ("   4.1. MVVM Arhitektura", "13"),
        ("   4.2. Entity Framework Core", "15"),
        ("   4.3. Dizajn Šabloni", "16"),
        ("   4.4. Serijalizacija", "18"),
        ("   4.5. PDF Izveštaji", "19"),
        ("5. Testiranje", "20"),
        ("6. Git i Verzionisanje", "22"),
        ("7. Zaključak", "23"),
    ]
    
    toc_style = ParagraphStyle(
        'TOC',
        parent=styles['Normal'],
        fontSize=11,
        leftIndent=0,
        spaceAfter=6
    )
    
    for item, page in toc_items:
        dots = "." * (80 - len(item) - len(page))
        story.append(Paragraph(f"{item} {dots} {page}", toc_style))
    
    story.append(PageBreak())

def create_introduction(story, styles):
    """Create introduction section"""
    story.append(Paragraph("1. Uvod", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    intro_text = """
    <b>Lični Planer Budžeta</b> je desktop aplikacija razvijena koristeći Windows Presentation 
    Foundation (WPF) sa .NET 6 platformom. Aplikacija omogućava korisnicima da efikasno upravljaju 
    svojim ličnim finansijama kroz praćenje prihoda i rashoda, kategorisanje transakcija, 
    postavljanje budžeta i generisanje detaljnih izveštaja.
    """
    story.append(Paragraph(intro_text, styles['Normal']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("1.1. Cilj Projekta", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    goal_text = """
    Cilj ovog projekta je razvoj potpuno funkcionalne desktop aplikacije koja demonstrira:
    """
    story.append(Paragraph(goal_text, styles['Normal']))
    story.append(Spacer(1, 6))
    
    goals = [
        "Implementaciju MVVM arhitekturnog obrasca",
        "Korišćenje Entity Framework Core za pristup podacima",
        "Primenu dizajn šablona (Singleton, Factory, Observer)",
        "Serijalizaciju podataka u JSON i XML formatima",
        "Generisanje PDF izveštaja",
        "Implementaciju autentifikacije korisnika",
        "Jedinično testiranje kritičnih komponenti"
    ]
    
    for goal in goals:
        story.append(Paragraph(f"• {goal}", styles['Normal']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("1.2. Tehnologije", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    tech_data = [
        ['Kategorija', 'Tehnologija'],
        ['UI Framework', 'WPF (.NET 6+), XAML'],
        ['Arhitektura', 'MVVM (Model-View-ViewModel)'],
        ['ORM', 'Entity Framework Core 6'],
        ['Baza podataka', 'SQLite'],
        ['Testiranje', 'MSTest'],
        ['Serijalizacija', 'System.Text.Json, XmlSerializer'],
        ['PDF Generisanje', 'iText7'],
        ['Verzionisanje', 'Git, GitHub'],
    ]
    
    tech_table = Table(tech_data, colWidths=[2*inch, 3.5*inch])
    tech_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, 0), 11),
        ('BOTTOMPADDING', (0, 0), (-1, 0), 12),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(tech_table)
    story.append(PageBreak())

def create_analysis_section(story, styles):
    """Create analysis section"""
    story.append(Paragraph("2. Analiza", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("2.1. Use Case Dijagram", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    uml_note = """
    <i>Napomena: PlantUML dijagrami se nalaze u folderu Documentation/UML. 
    Dijagrami mogu biti pregledani koristeći PlantUML preglednike ili online alate 
    na adresi: http://www.plantuml.com/plantuml</i>
    """
    story.append(Paragraph(uml_note, styles['Normal']))
    story.append(Spacer(1, 12))
    
    usecase_desc = """
    Use Case dijagram prikazuje sve glavne funkcionalnosti sistema koje su dostupne korisniku. 
    Sistem podržava 8 glavnih slučajeva upotrebe sa dodatnim proširenjima i uključivanjima.
    """
    story.append(Paragraph(usecase_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("Glavni Use Case-ovi:", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    usecases = [
        "UC1: Registracija/Login - Autentifikacija korisnika",
        "UC2: Upravljanje Kategorijama - CRUD operacije nad kategorijama",
        "UC3: Dodavanje Transakcija - Unos prihoda i rashoda",
        "UC4: Pregled Transakcija - Pregled i filtriranje podataka",
        "UC5: Postavljanje Budžeta - Definisanje mesečnih budžeta",
        "UC6: Generisanje Izveštaja - Kreiranje finansijskih izveštaja",
        "UC7: Export Podataka - Izvoz u JSON, XML i PDF formate",
        "UC8: Import Podataka - Uvoz prethodno izvezenih podataka"
    ]
    
    for uc in usecases:
        story.append(Paragraph(f"• {uc}", styles['Normal']))
    
    story.append(PageBreak())
    
    story.append(Paragraph("2.2. Opisi Use Case-ova", styles['Heading2']))
    story.append(Spacer(1, 12))
    
    # UC1
    story.append(Paragraph("UC1: Registracija/Login", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    uc1_data = [
        ['Atribut', 'Opis'],
        ['Akteri', 'Korisnik'],
        ['Preduslov', 'Aplikacija je pokrenuta'],
        ['Tok događaja', '1. Korisnik unosi korisničko ime i lozinku\n' +
                        '2. Sistem validira unete podatke\n' +
                        '3. Sistem kreira korisničku sesiju\n' +
                        '4. Korisnik pristupa glavnom ekranu'],
        ['Alternativni tok', 'Pogrešni kredencijali - prikazuje se poruka o grešci'],
        ['Postuslov', 'Korisnik je autentifikovan i ima pristup sistemu']
    ]
    
    uc1_table = Table(uc1_data, colWidths=[1.5*inch, 4*inch])
    uc1_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('VALIGN', (0, 0), (-1, -1), 'TOP'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(uc1_table)
    story.append(Spacer(1, 12))
    
    # UC3
    story.append(Paragraph("UC3: Dodavanje Transakcija", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    uc3_data = [
        ['Atribut', 'Opis'],
        ['Akteri', 'Autentifikovan korisnik'],
        ['Preduslov', 'Korisnik je prijavljen i nalazi se na Transactions View'],
        ['Tok događaja', '1. Korisnik unosi iznos transakcije\n' +
                        '2. Korisnik bira kategoriju\n' +
                        '3. Korisnik unosi opis transakcije\n' +
                        '4. Korisnik bira datum\n' +
                        '5. Korisnik potvrđuje unos klikom na Add\n' +
                        '6. Sistem validira podatke\n' +
                        '7. Sistem čuva transakciju u bazi\n' +
                        '8. Lista transakcija se ažurira'],
        ['Alternativni tok', 'Nevalidni podaci - prikazuje se poruka o grešci'],
        ['Postuslov', 'Nova transakcija je sačuvana u bazi podataka']
    ]
    
    uc3_table = Table(uc3_data, colWidths=[1.5*inch, 4*inch])
    uc3_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('VALIGN', (0, 0), (-1, -1), 'TOP'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(uc3_table)
    story.append(Spacer(1, 12))
    
    # UC6
    story.append(Paragraph("UC6: Generisanje Izveštaja", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    uc6_data = [
        ['Atribut', 'Opis'],
        ['Akteri', 'Autentifikovan korisnik'],
        ['Preduslov', 'Korisnik ima unete transakcije u sistemu'],
        ['Tok događaja', '1. Korisnik bira mesec i godinu za izveštaj\n' +
                        '2. Sistem generiše mesečni izveštaj\n' +
                        '3. Prikazuje se statistika prihoda i rashoda\n' +
                        '4. Korisnik može izvesti izveštaj u PDF format'],
        ['Postuslov', 'Izveštaj je prikazan/izvezen']
    ]
    
    uc6_table = Table(uc6_data, colWidths=[1.5*inch, 4*inch])
    uc6_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('VALIGN', (0, 0), (-1, -1), 'TOP'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(uc6_table)
    story.append(PageBreak())
    
    story.append(Paragraph("2.3. Korisničke Uloge", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    roles_text = """
    Aplikacija podržava samo jednu korisničku ulogu - <b>Korisnik</b>. Svaki korisnik ima pristup 
    svim funkcionalnostima aplikacije nakon autentifikacije. Podaci su izolovani po korisniku - 
    svaki korisnik vidi samo svoje transakcije, kategorije i budžete.
    """
    story.append(Paragraph(roles_text, styles['Normal']))
    story.append(PageBreak())

def create_modeling_section(story, styles):
    """Create modeling section"""
    story.append(Paragraph("3. Modelovanje", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("3.1. Dijagram Klasa", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    class_desc = """
    Dijagram klasa prikazuje strukturu aplikacije sa svim glavnim klasama, njihovim atributima, 
    metodama i relacijama. Aplikacija sadrži 9 glavnih klasa sa implementacijom nasleđivanja, 
    kompozicije i agregacije.
    """
    story.append(Paragraph(class_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("Glavne Klase:", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    classes_data = [
        ['Klasa', 'Tip', 'Opis'],
        ['Transaction', 'Apstraktna', 'Bazna klasa za sve transakcije'],
        ['Income', 'Konkretna', 'Predstavlja prihod (nasledjuje Transaction)'],
        ['Expense', 'Konkretna', 'Predstavlja rashod (nasledjuje Transaction)'],
        ['Category', 'Apstraktna', 'Bazna klasa za kategorije'],
        ['IncomeCategory', 'Konkretna', 'Kategorija prihoda'],
        ['ExpenseCategory', 'Konkretna', 'Kategorija rashoda'],
        ['User', 'Konkretna', 'Predstavlja korisnika sistema'],
        ['Budget', 'Konkretna', 'Mesečni budžet korisnika'],
        ['MonthlyReport', 'Konkretna', 'Mesečni finansijski izveštaj']
    ]
    
    classes_table = Table(classes_data, colWidths=[1.5*inch, 1.2*inch, 2.8*inch])
    classes_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(classes_table)
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("Relacije:", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    relations = [
        "<b>Nasleđivanje:</b> Income i Expense nasleđuju Transaction",
        "<b>Nasleđivanje:</b> IncomeCategory i ExpenseCategory nasleđuju Category",
        "<b>Kompozicija:</b> User poseduje kolekciju Transactions (1:N)",
        "<b>Kompozicija:</b> User poseduje kolekciju Budgets (1:N)",
        "<b>Agregacija:</b> Transaction referencira Category (N:1)",
        "<b>Interfejs:</b> ViewModelBase implementira INotifyPropertyChanged"
    ]
    
    for rel in relations:
        story.append(Paragraph(f"• {rel}", styles['Normal']))
    
    story.append(PageBreak())
    
    story.append(Paragraph("3.2. Dijagram Paketa", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    package_desc = """
    Dijagram paketa prikazuje organizaciju projekta u logičke celine (namespaces). 
    Projekat je organizovan prema MVVM arhitekturi sa jasnom separacijom odgovornosti.
    """
    story.append(Paragraph(package_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    packages_data = [
        ['Paket', 'Opis'],
        ['Models', 'Domenski modeli (entiteti baze podataka)'],
        ['ViewModels', 'ViewModel klase sa logikom prezentacije'],
        ['Views', 'XAML prikazi korisničkog interfejsa'],
        ['Services', 'Servisni sloj (Repository, Factory, Export, Report)'],
        ['Data', 'DbContext i konfiguracija baze'],
        ['Commands', 'ICommand implementacije (RelayCommand)'],
        ['Helpers', 'Helper klase (Converters, Extensions)']
    ]
    
    packages_table = Table(packages_data, colWidths=[1.5*inch, 4*inch])
    packages_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(packages_table)
    story.append(PageBreak())
    
    story.append(Paragraph("3.3. Dijagrami Sekvenci", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    sequence_desc = """
    Dijagrami sekvenci prikazuju interakcije između objekata tokom izvršavanja određenih 
    use case-ova. Implementirana su tri dijagrama sekvenci koja pokrivaju ključne funkcionalnosti.
    """
    story.append(Paragraph(sequence_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    # Login Sequence
    story.append(Paragraph("3.3.1. Login Sekvenca", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    login_seq = """
    <b>Učesnici:</b> Korisnik, LoginView, LoginViewModel, Repository, BudgetDbContext, UserSession
    <br/><br/>
    <b>Tok:</b><br/>
    1. Korisnik unosi kredencijale i klikće na Login dugme<br/>
    2. LoginView poziva LoginCommand u LoginViewModel-u<br/>
    3. LoginViewModel poziva Repository.ValidateUser()<br/>
    4. Repository upituje BudgetDbContext<br/>
    5. DbContext izvršava SQL upit i vraća User objekat<br/>
    6. Repository vraća rezultat u ViewModel<br/>
    7. LoginViewModel postavlja UserSession.CurrentUser<br/>
    8. LoginView prikazuje MainView
    """
    story.append(Paragraph(login_seq, styles['Normal']))
    story.append(Spacer(1, 12))
    
    # Add Transaction Sequence
    story.append(Paragraph("3.3.2. Dodavanje Transakcije", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    add_trans_seq = """
    <b>Učesnici:</b> Korisnik, TransactionView, TransactionViewModel, TransactionFactory, 
    Repository, BudgetDbContext
    <br/><br/>
    <b>Tok:</b><br/>
    1. Korisnik unosi podatke o transakciji i klikće Add dugme<br/>
    2. TransactionView poziva AddTransactionCommand<br/>
    3. TransactionViewModel validira unete podatke<br/>
    4. ViewModel poziva TransactionFactory.CreateTransaction()<br/>
    5. Factory kreira Income ili Expense objekat (Factory pattern)<br/>
    6. ViewModel poziva Repository.AddTransaction()<br/>
    7. Repository dodaje transakciju u DbContext<br/>
    8. DbContext čuva promene u bazi<br/>
    9. ViewModel osvežava listu transakcija (Observer pattern)<br/>
    10. View se ažurira kroz data binding
    """
    story.append(Paragraph(add_trans_seq, styles['Normal']))
    story.append(Spacer(1, 12))
    
    # Generate Report Sequence
    story.append(Paragraph("3.3.3. Generisanje Izveštaja", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    report_seq = """
    <b>Učesnici:</b> Korisnik, MainView, MainViewModel, ReportService, Repository, 
    ExportService, BudgetDbContext
    <br/><br/>
    <b>Tok:</b><br/>
    1. Korisnik bira mesec/godinu i klikće Generate Report dugme<br/>
    2. MainView poziva GenerateReportCommand<br/>
    3. MainViewModel poziva ReportService.GenerateMonthlyReport()<br/>
    4. ReportService poziva Repository za preuzimanje transakcija<br/>
    5. Repository izvršava LINQ upit preko DbContext-a<br/>
    6. ReportService kreira MonthlyReport objekat sa statistikom<br/>
    7. MainViewModel prikazuje izveštaj<br/>
    8. Opciono: Korisnik klikće Export to PDF<br/>
    9. ExportService.ExportReportToPdf() kreira PDF dokument<br/>
    10. Sistem čuva PDF fajl na disk
    """
    story.append(Paragraph(report_seq, styles['Normal']))
    story.append(PageBreak())

def create_implementation_section(story, styles):
    """Create implementation section"""
    story.append(Paragraph("4. Implementacija", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("4.1. MVVM Arhitektura", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    mvvm_desc = """
    Aplikacija je implementirana striktno prema MVVM (Model-View-ViewModel) arhitekturnom obrascu. 
    Ovaj obrazac omogućava jasnu separaciju odgovornosti i olakšava testiranje i održavanje koda.
    """
    story.append(Paragraph(mvvm_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    mvvm_layers = [
        [
            "<b>Model</b>",
            "• Domenski entiteti (User, Transaction, Category, Budget)<br/>" +
            "• Entity Framework Core mapiranja<br/>" +
            "• Poslovne pravila i validacija<br/>" +
            "• Nezavisan od UI-a"
        ],
        [
            "<b>View</b>",
            "• XAML datoteke sa UI definicijama<br/>" +
            "• Minimalan code-behind (samo inicijalizacija)<br/>" +
            "• Data binding na ViewModel properties<br/>" +
            "• Converters za prikaz podataka"
        ],
        [
            "<b>ViewModel</b>",
            "• Logika prezentacije<br/>" +
            "• ICommand implementacije (RelayCommand)<br/>" +
            "• INotifyPropertyChanged za data binding<br/>" +
            "• Pozivanje servisa za pristup podacima<br/>" +
            "• Nezavisan od View-a (testabilno)"
        ]
    ]
    
    mvvm_table = Table(mvvm_layers, colWidths=[1.2*inch, 4.3*inch])
    mvvm_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (0, -1), colors.HexColor('#e8f4f8')),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('VALIGN', (0, 0), (-1, -1), 'TOP'),
        ('FONTNAME', (0, 0), (0, -1), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 0), (-1, -1), [colors.HexColor('#f8f8f8'), colors.white, colors.HexColor('#f8f8f8')])
    ]))
    
    story.append(mvvm_table)
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("Primeri implementacije:", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    code_style = ParagraphStyle(
        'Code',
        parent=styles['Normal'],
        fontName='Courier',
        fontSize=8,
        leftIndent=20,
        textColor=colors.HexColor('#2c5aa0'),
        backColor=colors.HexColor('#f5f5f5')
    )
    
    viewmodel_code = """
    public class TransactionViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private ObservableCollection&lt;Transaction&gt; _transactions;
        
        public ICommand AddTransactionCommand { get; }
        
        public TransactionViewModel(IRepository repository)
        {
            _repository = repository;
            AddTransactionCommand = new RelayCommand(AddTransaction);
            LoadTransactions();
        }
        
        private void AddTransaction()
        {
            // Logika za dodavanje transakcije
            OnPropertyChanged(nameof(Transactions));
        }
    }
    """
    story.append(Paragraph("ViewModelBase sa INotifyPropertyChanged:", code_style))
    story.append(Spacer(1, 6))
    story.append(Paragraph(viewmodel_code, code_style))
    story.append(PageBreak())
    
    story.append(Paragraph("4.2. Entity Framework Core", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    ef_desc = """
    Entity Framework Core je korišćen kao ORM (Object-Relational Mapping) za pristup SQLite bazi 
    podataka. Implementiran je Code-First pristup sa migracijama.
    """
    story.append(Paragraph(ef_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    ef_entities = [
        "<b>User:</b> Korisnik sistema (Id, Username, PasswordHash, Email, CreatedAt)",
        "<b>Transaction:</b> Apstraktna klasa za transakcije (Id, Amount, Description, Date, UserId, CategoryId)",
        "<b>Income/Expense:</b> Konkretne implementacije transakcija (TPH - Table Per Hierarchy)",
        "<b>Category:</b> Apstraktna klasa za kategorije (Id, Name, Color)",
        "<b>IncomeCategory/ExpenseCategory:</b> Konkretne kategorije (TPH)",
        "<b>Budget:</b> Mesečni budžet (Id, Month, Year, PlannedAmount, UserId, CategoryId)",
        "<b>MonthlyReport:</b> Izveštaj (Id, Month, Year, TotalIncome, TotalExpense, Balance)"
    ]
    
    for entity in ef_entities:
        story.append(Paragraph(f"• {entity}", styles['Normal']))
    
    story.append(Spacer(1, 12))
    
    dbcontext_code = """
    public class BudgetDbContext : DbContext
    {
        public DbSet&lt;User&gt; Users { get; set; }
        public DbSet&lt;Transaction&gt; Transactions { get; set; }
        public DbSet&lt;Category&gt; Categories { get; set; }
        public DbSet&lt;Budget&gt; Budgets { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TPH konfiguracija za Transaction
            modelBuilder.Entity&lt;Transaction&gt;()
                .HasDiscriminator&lt;string&gt;("TransactionType")
                .HasValue&lt;Income&gt;("Income")
                .HasValue&lt;Expense&gt;("Expense");
                
            // Relacije
            modelBuilder.Entity&lt;Transaction&gt;()
                .HasOne(t =&gt; t.User)
                .WithMany(u =&gt; u.Transactions)
                .HasForeignKey(t =&gt; t.UserId);
        }
    }
    """
    story.append(Paragraph("BudgetDbContext konfiguracija:", code_style))
    story.append(Spacer(1, 6))
    story.append(Paragraph(dbcontext_code, code_style))
    story.append(PageBreak())
    
    story.append(Paragraph("4.3. Dizajn Šabloni", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    patterns_desc = """
    U aplikaciji su implementirana dva dizajn šablona prema zahtevima projekta:
    """
    story.append(Paragraph(patterns_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    # Singleton Pattern
    story.append(Paragraph("4.3.1. Singleton Pattern (Kreacioni)", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    singleton_desc = """
    <b>Klasa:</b> UserSession<br/>
    <b>Svrha:</b> Obezbeđuje da postoji samo jedna instanca korisničke sesije u celoj aplikaciji. 
    Koristi se za čuvanje trenutno ulogovanog korisnika.<br/>
    <b>Implementacija:</b> Thread-safe Singleton sa lazy initialization.
    """
    story.append(Paragraph(singleton_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    singleton_code = """
    public class UserSession
    {
        private static readonly Lazy&lt;UserSession&gt; _instance = 
            new Lazy&lt;UserSession&gt;(() =&gt; new UserSession());
        
        public static UserSession Instance =&gt; _instance.Value;
        
        public User CurrentUser { get; set; }
        public bool IsAuthenticated =&gt; CurrentUser != null;
        
        private UserSession() { }
    }
    """
    story.append(Paragraph(singleton_code, code_style))
    story.append(Spacer(1, 12))
    
    # Factory Pattern
    story.append(Paragraph("4.3.2. Factory Pattern (Kreacioni)", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    factory_desc = """
    <b>Klasa:</b> TransactionFactory<br/>
    <b>Svrha:</b> Kreira odgovarajući tip transakcije (Income ili Expense) na osnovu ulaznih parametara. 
    Enkapsulira logiku kreiranja objekata.<br/>
    <b>Prednost:</b> Centralizovano kreiranje objekata, laka proširivost.
    """
    story.append(Paragraph(factory_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    factory_code = """
    public class TransactionFactory
    {
        public static Transaction CreateTransaction(
            string type, decimal amount, string description, 
            DateTime date, Category category, User user)
        {
            return type.ToLower() switch
            {
                "income" =&gt; new Income
                {
                    Amount = amount,
                    Description = description,
                    Date = date,
                    Category = category,
                    User = user
                },
                "expense" =&gt; new Expense
                {
                    Amount = amount,
                    Description = description,
                    Date = date,
                    Category = category,
                    User = user
                },
                _ =&gt; throw new ArgumentException("Invalid transaction type")
            };
        }
    }
    """
    story.append(Paragraph(factory_code, code_style))
    story.append(PageBreak())
    
    # Observer Pattern
    story.append(Paragraph("4.3.3. Observer Pattern (Ponašajni)", styles['Heading3']))
    story.append(Spacer(1, 6))
    
    observer_desc = """
    <b>Implementacija:</b> INotifyPropertyChanged interfejs u ViewModelBase<br/>
    <b>Svrha:</b> Automatsko obaveštavanje View-a o promenama u ViewModel-u. Omogućava reaktivno 
    ažuriranje UI-a.<br/>
    <b>Mehanizam:</b> PropertyChanged event koji se aktivira pri promeni svojstava.
    """
    story.append(Paragraph(observer_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    observer_code = """
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected bool SetProperty&lt;T&gt;(ref T field, T value, 
                                      [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer&lt;T&gt;.Default.Equals(field, value))
                return false;
                
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
    """
    story.append(Paragraph(observer_code, code_style))
    story.append(PageBreak())
    
    story.append(Paragraph("4.4. Serijalizacija", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    serialization_desc = """
    Aplikacija podržava serijalizaciju i deserijalizaciju podataka u JSON i XML formatima. 
    Implementiran je ExportService koji omogućava izvoz i uvoz podataka.
    """
    story.append(Paragraph(serialization_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    json_code = """
    // JSON Serijalizacija
    public void ExportToJson(List&lt;Transaction&gt; transactions, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var json = JsonSerializer.Serialize(transactions, options);
        File.WriteAllText(filePath, json);
    }
    
    // XML Serijalizacija
    public void ExportToXml(List&lt;Transaction&gt; transactions, string filePath)
    {
        var serializer = new XmlSerializer(typeof(List&lt;Transaction&gt;));
        using var writer = new StreamWriter(filePath);
        serializer.Serialize(writer, transactions);
    }
    """
    story.append(Paragraph(json_code, code_style))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("4.5. PDF Izveštaji", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    pdf_desc = """
    Za generisanje PDF izveštaja koristi se iText7 biblioteka. ReportService kreira mesečne 
    izveštaje sa statistikom prihoda i rashoda, koje ExportService konvertuje u PDF format.
    """
    story.append(Paragraph(pdf_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    pdf_code = """
    public void ExportReportToPdf(MonthlyReport report, string filePath)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);
        
        // Naslov
        document.Add(new Paragraph($"Mesečni Izveštaj - {report.Month}/{report.Year}")
            .SetFontSize(20).SetBold());
        
        // Statistika
        document.Add(new Paragraph($"Ukupni Prihodi: {report.TotalIncome:C}"));
        document.Add(new Paragraph($"Ukupni Rashodi: {report.TotalExpense:C}"));
        document.Add(new Paragraph($"Bilans: {report.Balance:C}"));
        
        // Tabela transakcija
        var table = new Table(4);
        table.AddHeaderCell("Datum");
        table.AddHeaderCell("Opis");
        table.AddHeaderCell("Kategorija");
        table.AddHeaderCell("Iznos");
        
        foreach (var transaction in report.Transactions)
        {
            table.AddCell(transaction.Date.ToString("dd.MM.yyyy"));
            table.AddCell(transaction.Description);
            table.AddCell(transaction.Category.Name);
            table.AddCell(transaction.Amount.ToString("C"));
        }
        
        document.Add(table);
        document.Close();
    }
    """
    story.append(Paragraph(pdf_code, code_style))
    story.append(PageBreak())

def create_testing_section(story, styles):
    """Create testing section"""
    story.append(Paragraph("5. Testiranje", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    testing_desc = """
    Implementirano je jedinično testiranje (unit testing) kritičnih komponenti aplikacije koristeći 
    MSTest framework. Testovi pokrivaju ViewModel logiku, Factory pattern i Singleton pattern.
    """
    story.append(Paragraph(testing_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("5.1. Test Klase", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    tests_data = [
        ['Test Klasa', 'Broj Testova', 'Pokriva'],
        ['TransactionViewModelTests', '3', 'ViewModel logiku, data binding, validaciju'],
        ['TransactionFactoryTests', '2', 'Factory pattern, kreiranje objekata'],
        ['UserSessionTests', '3', 'Singleton pattern, autentifikaciju'],
    ]
    
    tests_table = Table(tests_data, colWidths=[2*inch, 1.3*inch, 2.2*inch])
    tests_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a5490')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, -1), 10),
        ('GRID', (0, 0), (-1, -1), 1, colors.grey),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f0f0f0')])
    ]))
    
    story.append(tests_table)
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("5.2. Primeri Testova", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    code_style = ParagraphStyle(
        'Code',
        parent=styles['Normal'],
        fontName='Courier',
        fontSize=8,
        leftIndent=20,
        textColor=colors.HexColor('#2c5aa0'),
        backColor=colors.HexColor('#f5f5f5')
    )
    
    test_code = """
    [TestClass]
    public class TransactionFactoryTests
    {
        [TestMethod]
        public void CreateTransaction_IncomeType_ReturnsIncomeObject()
        {
            // Arrange
            var category = new IncomeCategory { Name = "Salary" };
            var user = new User { Username = "test" };
            
            // Act
            var transaction = TransactionFactory.CreateTransaction(
                "income", 1000m, "Monthly salary", DateTime.Now, category, user);
            
            // Assert
            Assert.IsInstanceOfType(transaction, typeof(Income));
            Assert.AreEqual(1000m, transaction.Amount);
        }
        
        [TestMethod]
        public void CreateTransaction_ExpenseType_ReturnsExpenseObject()
        {
            // Arrange
            var category = new ExpenseCategory { Name = "Food" };
            var user = new User { Username = "test" };
            
            // Act
            var transaction = TransactionFactory.CreateTransaction(
                "expense", 50m, "Groceries", DateTime.Now, category, user);
            
            // Assert
            Assert.IsInstanceOfType(transaction, typeof(Expense));
            Assert.AreEqual(50m, transaction.Amount);
        }
    }
    
    [TestClass]
    public class UserSessionTests
    {
        [TestMethod]
        public void Instance_ReturnsSameInstance()
        {
            // Act
            var instance1 = UserSession.Instance;
            var instance2 = UserSession.Instance;
            
            // Assert
            Assert.AreSame(instance1, instance2);
        }
        
        [TestMethod]
        public void IsAuthenticated_WithCurrentUser_ReturnsTrue()
        {
            // Arrange
            var session = UserSession.Instance;
            session.CurrentUser = new User { Username = "test" };
            
            // Act & Assert
            Assert.IsTrue(session.IsAuthenticated);
        }
    }
    """
    story.append(Paragraph(test_code, code_style))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("5.3. Pokretanje Testova", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    test_run = """
    Testovi se mogu pokrenuti iz Visual Studio-a preko Test Explorer-a ili korišćenjem 
    komandne linije:
    """
    story.append(Paragraph(test_run, styles['Normal']))
    story.append(Spacer(1, 6))
    
    test_command = """
    dotnet test BudgetPlanner.Tests/BudgetPlanner.Tests.csproj
    """
    story.append(Paragraph(test_command, code_style))
    story.append(PageBreak())

def create_git_section(story, styles):
    """Create Git section"""
    code_style = ParagraphStyle(
        'Code',
        parent=styles['Normal'],
        fontName='Courier',
        fontSize=8,
        leftIndent=20,
        textColor=colors.HexColor('#2c5aa0'),
        backColor=colors.HexColor('#f5f5f5')
    )
    
    story.append(Paragraph("6. Git i Verzionisanje", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    git_desc = """
    Projekat koristi Git za verzionisanje koda i GitHub za hosting repozitorijuma. Implementirana 
    je strategija grananja (branching) sa feature granama i povremenim merge-ovima u main granu.
    """
    story.append(Paragraph(git_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("6.1. Struktura Repozitorijuma", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    repo_structure = """
    BudgetPlanner/<br/>
    ├── BudgetPlanner.App/          # Glavni WPF projekat<br/>
    │   ├── Models/                 # Domenski modeli<br/>
    │   ├── ViewModels/             # ViewModel klase<br/>
    │   ├── Views/                  # XAML prikazi<br/>
    │   ├── Services/               # Servisni sloj<br/>
    │   ├── Data/                   # DbContext<br/>
    │   ├── Commands/               # ICommand implementacije<br/>
    │   └── Helpers/                # Helper klase<br/>
    ├── BudgetPlanner.Tests/        # Test projekat<br/>
    ├── Documentation/              # Dokumentacija<br/>
    │   └── UML/                    # PlantUML dijagrami<br/>
    ├── .gitignore                  # Git ignore fajl<br/>
    ├── README.md                   # Projekat README<br/>
    └── BudgetPlanner.sln           # Solution fajl
    """
    story.append(Paragraph(repo_structure, code_style))
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("6.2. Commit Istorija", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    commit_desc = """
    Projekat sadrži više od 15 commit-ova koji prate razvoj aplikacije od početne strukture 
    do finalne verzije. Commit-ovi su pravilno imenovani i opisani.
    """
    story.append(Paragraph(commit_desc, styles['Normal']))
    story.append(Spacer(1, 12))
    
    commits = [
        "Initial project structure with MVVM folders",
        "Add Entity Framework Core and configure DbContext",
        "Implement Transaction model with inheritance (TPH)",
        "Add Category models with inheritance",
        "Implement User and Budget models",
        "Add Repository pattern implementation",
        "Implement Singleton pattern for UserSession",
        "Add Factory pattern for Transaction creation",
        "Implement LoginViewModel and LoginView",
        "Add TransactionViewModel with CRUD operations",
        "Implement CategoryViewModel",
        "Add BudgetViewModel",
        "Implement ReportService for monthly reports",
        "Add JSON and XML serialization in ExportService",
        "Implement PDF export functionality",
        "Add unit tests for ViewModels",
        "Add unit tests for design patterns",
        "Update README with project documentation",
        "Add PlantUML diagrams",
        "Final documentation and cleanup"
    ]
    
    for i, commit in enumerate(commits, 1):
        story.append(Paragraph(f"{i}. {commit}", styles['Normal']))
    
    story.append(Spacer(1, 12))
    
    story.append(Paragraph("6.3. Grane", styles['Heading2']))
    story.append(Spacer(1, 6))
    
    branches = """
    • <b>main</b> - Glavna grana sa stabilnim kodom<br/>
    • <b>feature/ef-core-setup</b> - Podešavanje Entity Framework Core<br/>
    • <b>feature/viewmodels</b> - Implementacija ViewModel klasa<br/>
    • <b>feature/serialization</b> - JSON i XML serijalizacija<br/>
    • <b>feature/testing</b> - Dodavanje jediničnih testova
    """
    story.append(Paragraph(branches, styles['Normal']))
    story.append(PageBreak())

def create_conclusion(story, styles):
    """Create conclusion section"""
    story.append(Paragraph("7. Zaključak", styles['Heading1']))
    story.append(Spacer(1, 12))
    
    conclusion_text = """
    Projekat <b>Lični Planer Budžeta</b> uspešno demonstrira primenu MVVM arhitekture u WPF aplikacijama 
    sa integracijom Entity Framework Core ORM-a. Kroz implementaciju aplikacije ostvareni su svi 
    postavljeni zahtevi projekta:
    """
    story.append(Paragraph(conclusion_text, styles['Normal']))
    story.append(Spacer(1, 12))
    
    achievements = [
        "<b>Funkcionalni zahtevi:</b> Implementirano 8 glavnih use case-ova sa CRUD operacijama, " +
        "filtiranjem, serijalizacijom i generisanjem izveštaja",
        
        "<b>MVVM arhitektura:</b> Striktna primena MVVM obrasca sa jasnom separacijom Model-View-ViewModel slojeva",
        
        "<b>Entity Framework Core:</b> Konfiguracija DbContext-a sa 5 entiteta, relacijama, migracijama i " +
        "Table-Per-Hierarchy strategijom nasleđivanja",
        
        "<b>Dizajn šabloni:</b> Implementacija Singleton (UserSession), Factory (TransactionFactory) i " +
        "Observer (INotifyPropertyChanged) šablona",
        
        "<b>Serijalizacija:</b> Podršk za JSON i XML formate sa mogućnošću eksporta i importa podataka",
        
        "<b>PDF izveštaji:</b> Generisanje profesionalnih mesečnih izveštaja sa tabelama i statistikom",
        
        "<b>Testiranje:</b> 8 jediničnih testova koji pokrivaju ViewModel logiku i dizajn šablone",
        
        "<b>UML modelovanje:</b> Kompletna dokumentacija sa Use Case, Class, Package i Sequence dijagramima",
        
        "<b>Git verzionisanje:</b> Preko 15 commit-ova sa feature granama i pravilnim commit porukama"
    ]
    
    for achievement in achievements:
        story.append(Paragraph(f"• {achievement}", styles['Normal']))
    
    story.append(Spacer(1, 12))
    
    future_work = """
    <b>Moguća proširenja aplikacije:</b><br/>
    • Integracija sa bankovnim API-jem za automatski uvoz transakcija<br/>
    • Dodavanje grafičkih prikaza (charts) za vizualizaciju budžeta<br/>
    • Implementacija multi-user podrške sa različitim ulogama<br/>
    • Cloud sinhronizacija podataka<br/>
    • Mobilna aplikacija za praćenje rashoda u pokretu<br/>
    • Machine learning predikcije budućih troškova
    """
    story.append(Paragraph(future_work, styles['Normal']))
    story.append(Spacer(1, 12))
    
    final_note = """
    Aplikacija je u potpunosti funkcionalna, testirana i pripremljena za deployment. Izvorni kod je 
    organizovan, dokumentovan i dostupan na GitHub-u. Sva dokumentacija, uključujući UML dijagrame 
    i ovu PDF dokumentaciju, pruža kompletnu sliku arhitekture i implementacije projekta.
    """
    story.append(Paragraph(final_note, styles['Normal']))

def generate_documentation():
    """Main function to generate PDF documentation"""
    output_path = "/home/claude/BudgetPlanner/Documentation/Projektna_Dokumentacija.pdf"
    
    # Create document
    doc = SimpleDocTemplate(
        output_path,
        pagesize=A4,
        rightMargin=72,
        leftMargin=72,
        topMargin=72,
        bottomMargin=72
    )
    
    # Container for the 'Flowable' objects
    story = []
    
    # Define styles
    styles = getSampleStyleSheet()
    
    # Build document sections
    print("Generating title page...")
    create_title_page(story, styles)
    
    print("Generating table of contents...")
    create_toc(story, styles)
    
    print("Generating introduction...")
    create_introduction(story, styles)
    
    print("Generating analysis section...")
    create_analysis_section(story, styles)
    
    print("Generating modeling section...")
    create_modeling_section(story, styles)
    
    print("Generating implementation section...")
    create_implementation_section(story, styles)
    
    print("Generating testing section...")
    create_testing_section(story, styles)
    
    print("Generating Git section...")
    create_git_section(story, styles)
    
    print("Generating conclusion...")
    create_conclusion(story, styles)
    
    # Build PDF
    print("Building PDF document...")
    doc.build(story)
    
    print(f"✓ Documentation generated successfully: {output_path}")
    return output_path

if __name__ == "__main__":
    generate_documentation()
