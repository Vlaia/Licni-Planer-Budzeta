using BudgetPlanner.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Servis za export i import podataka u JSON/XML format.
    /// </summary>
    public class ExportService
    {
        private readonly JsonSerializerOptions _jsonOptions;
        
        public ExportService()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }
        
        #region JSON Export/Import
        
        /// <summary>
        /// Exportuje transakcije u JSON fajl.
        /// </summary>
        public void ExportTransactionsToJson(IEnumerable<Transaction> transactions, string filePath)
        {
            var json = JsonSerializer.Serialize(transactions, _jsonOptions);
            File.WriteAllText(filePath, json);
        }
        
        /// <summary>
        /// Importuje transakcije iz JSON fajla.
        /// </summary>
        public List<Transaction> ImportTransactionsFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(json, _jsonOptions);
            return transactions ?? new List<Transaction>();
        }
        
        /// <summary>
        /// Exportuje kategorije u JSON fajl.
        /// </summary>
        public void ExportCategoriesToJson(IEnumerable<Category> categories, string filePath)
        {
            var json = JsonSerializer.Serialize(categories, _jsonOptions);
            File.WriteAllText(filePath, json);
        }
        
        /// <summary>
        /// Importuje kategorije iz JSON fajla.
        /// </summary>
        public List<Category> ImportCategoriesFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var categories = JsonSerializer.Deserialize<List<Category>>(json, _jsonOptions);
            return categories ?? new List<Category>();
        }
        
        #endregion
        
        #region XML Export/Import
        
        /// <summary>
        /// Exportuje transakcije u XML fajl.
        /// </summary>
        public void ExportTransactionsToXml(IEnumerable<Transaction> transactions, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Transaction>), 
                new Type[] { typeof(Income), typeof(Expense) });
            
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, new List<Transaction>(transactions));
        }
        
        /// <summary>
        /// Importuje transakcije iz XML fajla.
        /// </summary>
        public List<Transaction> ImportTransactionsFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Transaction>),
                new Type[] { typeof(Income), typeof(Expense) });
            
            using var reader = new StreamReader(filePath);
            var transactions = (List<Transaction>?)serializer.Deserialize(reader);
            return transactions ?? new List<Transaction>();
        }
        
        /// <summary>
        /// Exportuje kategorije u XML fajl.
        /// </summary>
        public void ExportCategoriesToXml(IEnumerable<Category> categories, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Category>),
                new Type[] { typeof(IncomeCategory), typeof(ExpenseCategory) });
            
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, new List<Category>(categories));
        }
        
        /// <summary>
        /// Importuje kategorije iz XML fajla.
        /// </summary>
        public List<Category> ImportCategoriesFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Category>),
                new Type[] { typeof(IncomeCategory), typeof(ExpenseCategory) });
            
            using var reader = new StreamReader(filePath);
            var categories = (List<Category>?)serializer.Deserialize(reader);
            return categories ?? new List<Category>();
        }
        
        #endregion
        
        /// <summary>
        /// Vraća filter string za SaveFileDialog.
        /// </summary>
        public string GetExportFileFilter()
        {
            return "JSON fajlovi (*.json)|*.json|XML fajlovi (*.xml)|*.xml|Svi fajlovi (*.*)|*.*";
        }
        
        /// <summary>
        /// Vraća filter string za OpenFileDialog.
        /// </summary>
        public string GetImportFileFilter()
        {
            return "JSON/XML fajlovi (*.json;*.xml)|*.json;*.xml|Svi fajlovi (*.*)|*.*";
        }
    }
}
