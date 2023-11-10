using MiniBankApp2.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBankApp2.Interfaces
{
    internal interface IPersistenceService
    {
        public List<BankAccount> FetchAllAccounts();

        public BankAccount FindAccount(int accountNumber);

        public bool SaveAllAccounts(List<BankAccount> accounts);
    }
}

/*
The MiniBankApp2 needs to persist data. 
When the app starts, it should load existing bank accounts with all their data.
 
If there are no bank accounts saved, prompt the user to open one. 
Otherwise, prompt the user to enter their account number to retrieve existing account info.
 
Before exiting the app, ensure all bank accounts are saved.
 
Tasks:

- Define an abstraction for a persistence service, capable of 
    - fetching all accounts, 
    - searching for a given account and 
    - saving all bank accounts.
- Implement the service to use a JSON file for storage.
- Consume the service in the Main method to load and persist bank account information. 
 
 */