using BudgetPlanner.App.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Servis za generisanje PDF izveštaja.
    /// </summary>
    public class ReportService
    {
        /// <summary>
        /// Generiše mesečni izveštaj u PDF formatu.
        /// </summary>
        public void GenerateMonthlyReport(
            MonthlyReport report,
            IEnumerable<Transaction> transactions,
            string filePath)
        {
            Document document = new Document(PageSize.A4, 50, 50, 50, 50);

            try
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Naslov - koristimo Arial sa CP1250 encoding za srpska slova
                BaseFont arialBase = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1250, BaseFont.EMBEDDED);
                BaseFont arialBoldBase = BaseFont.CreateFont("c:\\windows\\fonts\\arialbd.ttf", BaseFont.CP1250, BaseFont.EMBEDDED);

                Font titleFont = new Font(arialBoldBase, 18);
                Font headerFont = new Font(arialBoldBase, 14);
                Font normalFont = new Font(arialBase, 12);
                Font boldFont = new Font(arialBoldBase, 12);
                
                Paragraph title = new Paragraph($"Mesečni Finansijski Izveštaj", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                
                document.Add(new Paragraph(" ")); // Razmak
                
                // Informacije o izveštaju
                Paragraph period = new Paragraph($"Period: {report.GetMonthName()}", headerFont);
                document.Add(period);
                
                Paragraph generated = new Paragraph($"Generisano: {report.GeneratedAt:dd.MM.yyyy HH:mm}", normalFont);
                document.Add(generated);
                
                document.Add(new Paragraph(" ")); // Razmak
                
                // Finansijski pregled
                PdfPTable summaryTable = new PdfPTable(2);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 3f, 2f });
                
                // Header
                PdfPCell headerCell = new PdfPCell(new Phrase("Finansijski Pregled", boldFont));
                headerCell.Colspan = 2;
                headerCell.BackgroundColor = new BaseColor(200, 200, 200);
                headerCell.Padding = 5;
                summaryTable.AddCell(headerCell);
                
                // Prihodi
                summaryTable.AddCell(new Phrase("Ukupni Prihodi:", boldFont));
                summaryTable.AddCell(new Phrase($"{report.TotalIncome:C}", normalFont));
                
                // Rashodi
                summaryTable.AddCell(new Phrase("Ukupni Rashodi:", boldFont));
                summaryTable.AddCell(new Phrase($"{report.TotalExpenses:C}", normalFont));
                
                // Bilans
                PdfPCell balanceLabel = new PdfPCell(new Phrase("Bilans:", boldFont));
                balanceLabel.BackgroundColor = new BaseColor(240, 240, 240);
                summaryTable.AddCell(balanceLabel);
                
                Font balanceFont = report.IsBalancePositive()
                    ? new Font(arialBoldBase, 12, Font.NORMAL, new BaseColor(0, 128, 0))  // Green
                    : new Font(arialBoldBase, 12, Font.NORMAL, new BaseColor(255, 0, 0)); // Red
                
                PdfPCell balanceValue = new PdfPCell(new Phrase($"{report.Balance:C}", balanceFont));
                balanceValue.BackgroundColor = new BaseColor(240, 240, 240);
                summaryTable.AddCell(balanceValue);
                
                document.Add(summaryTable);
                document.Add(new Paragraph(" ")); // Razmak
                
                // Transakcije
                if (transactions.Any())
                {
                    Paragraph transactionsHeader = new Paragraph("Transakcije", headerFont);
                    document.Add(transactionsHeader);
                    document.Add(new Paragraph(" ")); // Razmak
                    
                    PdfPTable transactionsTable = new PdfPTable(4);
                    transactionsTable.WidthPercentage = 100;
                    transactionsTable.SetWidths(new float[] { 2f, 3f, 2f, 2f });
                    
                    // Header reda
                    AddTableHeader(transactionsTable, "Datum", boldFont);
                    AddTableHeader(transactionsTable, "Opis", boldFont);
                    AddTableHeader(transactionsTable, "Kategorija", boldFont);
                    AddTableHeader(transactionsTable, "Iznos", boldFont);
                    
                    // Transakcije
                    foreach (var transaction in transactions.OrderByDescending(t => t.Date))
                    {
                        transactionsTable.AddCell(new Phrase(transaction.Date.ToString("dd.MM.yyyy"), normalFont));
                        transactionsTable.AddCell(new Phrase(transaction.Description, normalFont));
                        transactionsTable.AddCell(new Phrase(transaction.Category?.Name ?? "N/A", normalFont));

                        Font amountFont = transaction is Income
                            ? new Font(arialBase, 12, Font.NORMAL, new BaseColor(0, 128, 0))  // Green
                            : new Font(arialBase, 12, Font.NORMAL, new BaseColor(255, 0, 0)); // Red
                        
                        transactionsTable.AddCell(new Phrase(transaction.FormatAmount(), amountFont));
                    }
                    
                    document.Add(transactionsTable);
                }
                
                // Footer
                document.Add(new Paragraph(" ")); // Razmak
                Font footerFont = new Font(arialBase, 10, Font.ITALIC);
                Paragraph footer = new Paragraph(
                    $"Izveštaj generisan aplikacijom Budget Planner",
                    footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);
            }
            finally
            {
                document.Close();
            }
        }
        
        private void AddTableHeader(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = new BaseColor(200, 200, 200);
            cell.Padding = 5;
            table.AddCell(cell);
        }
        
        /// <summary>
        /// Generiše izveštaj o kategorijama rashoda.
        /// </summary>
        public void GenerateCategoryReport(
            IEnumerable<Category> categories,
            Dictionary<int, decimal> categoryTotals,
            string filePath,
            DateTime startDate,
            DateTime endDate)
        {
            Document document = new Document(PageSize.A4, 50, 50, 50, 50);

            try
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Arial fontovi sa CP1250 encoding za srpska slova
                BaseFont arialBase = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1250, BaseFont.EMBEDDED);
                BaseFont arialBoldBase = BaseFont.CreateFont("c:\\windows\\fonts\\arialbd.ttf", BaseFont.CP1250, BaseFont.EMBEDDED);

                Font titleFont = new Font(arialBoldBase, 18);
                Font boldFont = new Font(arialBoldBase, 12);
                Font normalFont = new Font(arialBase, 12);
                
                Paragraph title = new Paragraph("Izveštaj po Kategorijama", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Period: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}", normalFont));
                document.Add(new Paragraph(" "));
                
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 4f, 2f, 3f });
                
                AddTableHeader(table, "Kategorija", boldFont);
                AddTableHeader(table, "Tip", boldFont);
                AddTableHeader(table, "Ukupan Iznos", boldFont);
                
                foreach (var category in categories)
                {
                    decimal total = categoryTotals.ContainsKey(category.Id) ? categoryTotals[category.Id] : 0;
                    
                    table.AddCell(new Phrase(category.Name, normalFont));
                    table.AddCell(new Phrase(category.GetCategoryType(), normalFont));
                    table.AddCell(new Phrase($"{total:C}", normalFont));
                }
                
                document.Add(table);
            }
            finally
            {
                document.Close();
            }
        }
    }
}
