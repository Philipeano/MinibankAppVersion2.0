using MiniBankApp2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniBankApp2.Implementations
{
    internal class JsonDataService : IPersistenceService
    {
        private const string _jsonFilePath = "C:\\CSharpDemo\\JsonFiles\\BankAccounts.json";
        private List<BankAccount> _bankAccounts;

        // Define options to customize how we want the object serialized/deserialized
        private JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            IncludeFields = true
        };

        public JsonDataService()
        {
            _bankAccounts = new List<BankAccount>();
        }

        public List<BankAccount> FetchAllAccounts()
        {
            // First, read all text from the JSON file
            string jsonContent = File.ReadAllText(_jsonFilePath);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Console.WriteLine("Unable to complete the fetch operation. JSON file has no content.");
                return new List<BankAccount> { };
            }

            // Attempt to deserialize the JSON text to obtain bank accounts
            List<BankAccount>? deserializedAccounts = JsonSerializer.Deserialize<List<BankAccount>>(jsonContent, _options);
            if (deserializedAccounts == null)
            {
                Console.WriteLine("Unable to complete the fetch operation. Deserialization failed.");
                return new List<BankAccount> { };
            }

            // Return the bank accounts, if any, or return the empty list 
            _bankAccounts = deserializedAccounts;
            Console.WriteLine("Fetch operation was successful.");
            return _bankAccounts;
        }

        public BankAccount FindAccount(int accountNumber)
        {
            // Search the bank accounts list for an account with the given account number
            var result = _bankAccounts.Find(x => x.AccountNumber == accountNumber);
            return result;            
        }

        public bool SaveAllAccounts(List<BankAccount> accounts)
        {
            // Check if the input list has any items
            if (!accounts.Any())
            {
                Console.WriteLine("Invalid operation! Cannot save an empty list of accounts.");
                return false;
            }
            
            // Serialize the given list into a JSON string
            var jsonContent = JsonSerializer.Serialize(accounts, _options);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Console.WriteLine("Unable to complete the save operation. Serialization failed.");
                return false;
            }

            // Write the JSON contant to the file
            File.WriteAllText(_jsonFilePath, jsonContent);
            Console.WriteLine("Save operation was successful.");
            return true;
        }
    }
}
