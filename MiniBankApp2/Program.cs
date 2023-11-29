using MiniBankApp2.Enums;
using MiniBankApp2.Helpers;
using MiniBankApp2.Implementations;
using MiniBankApp2.Interfaces;
using MiniBankApp2.Models;

public class Program
{
    public static void Main(string[] args)
    {
        // Create an instance of IReportService to be passed in as a dependency to BankAccount.
        // Note: We are doing this manually because we are not using a dependency injection (DI) container.
        var reportService = new ReportService();

        // Load existing bank accounts
        IPersistenceService dataService = new JsonDataService();
        List<Account> availableAccounts = dataService.FetchAllAccounts();

        BankAccount activeBankAccount; 

        Console.WriteLine("----------  YOU ARE WELCOME TO THE BULB MINI BANK  -----------");

        // If there are no existing accounts found, then prompt the user to create one
        if (availableAccounts.Count == 0)
        {
            string firstName;
            string lastName;
            double initialBalance;
            AccountType accountType;

            Console.WriteLine("Please create an account for yourself......!");
            Console.Write("Input your first name: ");
            firstName = Console.ReadLine();
            Console.Write("Input your last name: ");
            lastName = Console.ReadLine();
            Console.Write("Kindly indicate the account type you would like to have using 1. savings 2. current 3. Domiciliary 4. fixed: ");
            //casting from string to integer and to enum type TODO: ensure input is between 1 to 4.
            accountType = (AccountType)int.Parse(Console.ReadLine());
            Console.Write("Kindly type in the amount you would like to start with: ");
            var balanceString = Console.ReadLine();
            double.TryParse(balanceString, out double doubleBalance);
            initialBalance = doubleBalance;

            // Instantiate the BankAccount class to open an account
            var account = new BankAccount(firstName, lastName, initialBalance, accountType, reportService);
            activeBankAccount = account;
        }
        else
        {
            int accountNumberToFetch;
            Console.WriteLine("Please enter your account number to retrieve your account information......");
            string bankAccountInput = Console.ReadLine();
            int.TryParse(bankAccountInput, out accountNumberToFetch);

            // Now, search the available accounts list for a matching account
            var matchingAccount = availableAccounts.Find(x => x.AccountNumber == accountNumberToFetch);

            if (matchingAccount == null)
            {
                Console.WriteLine("Sorry, this account number does not exist.");
                // TODO: Ask the user if they would like to search again, create a new account or exit the app
                // After 3 failed attempts, display a warning message and exit the app
                return;
            }

            activeBankAccount = new BankAccount(matchingAccount, reportService);
        }

        // Display current balance
        activeBankAccount.DisplayBalance();

        Console.WriteLine();
        Console.WriteLine();

        int userDecision;
        do
        {
            int userChoice;
            do
            {
                Console.WriteLine("Welcome! What would you like to do next?\n1. Deposit Funds 2. Withdraw cash 3. View Statement  4. Add beneficiary  5. View beneficiaries");
                bool userChoiceValid = int.TryParse(Console.ReadLine(), out userChoice);
                if (!userChoiceValid)
                {
                    Console.WriteLine("Invalid input! Please enter a valid number");
                    Console.WriteLine();
                }
            } while (!(userChoice >= 1 && userChoice <= 5));

            switch (userChoice)
            {  
                case 1:
                    {  //make deposit
                        Console.WriteLine("Kindly enter an amount to deposit");
                        //TODO: validation is required
                        var amountToDeposit = Convert.ToDouble(Console.ReadLine());
                        activeBankAccount.Deposit(amountToDeposit);
                        Console.WriteLine();
                        break;
                    }

                case 2:
                    {
                        //make a withdrawal
                        Console.WriteLine("Kindly enter an amount to withdraw");
                        //TODO: validation is required
                        var amountToWithdraw = Convert.ToDouble(Console.ReadLine());
                        activeBankAccount.Withdraw(amountToWithdraw);
                        Console.WriteLine();
                        break;
                    }

                case 3:
                    activeBankAccount.PrintHistory();
                    break;
                case 4:
                    activeBankAccount.AddBeneficiary();
                    break;
                case 5:
                    activeBankAccount.ViewBeneficiaries();
                    break;
                default:
                    Console.WriteLine("Wrong Input detected. Please enter a valid input");
                    Console.WriteLine();
                    break;
            }

            Console.WriteLine("Would you like to perform another transaction? \nEnter 1 for \"Yes\" 2 for \"No\"");
            Console.WriteLine();
            bool userDecisionSuccess = int.TryParse(Console.ReadLine(), out userDecision);
        } while (userDecision == 1);

        if (userDecision == 2)
        {
            // Ensure the active bank account used for this session is part of the final list of accounts to be persisted.

            // First, create an Account instance from the activeBankAccount object
            var accountToSave = AccountHelper.MapBankAccountToAccount(activeBankAccount);

            var accountToUpdate = availableAccounts.Find(x => x.AccountNumber == activeBankAccount.AccountNumber);
            if (accountToUpdate == null)
            {   
                availableAccounts.Add(accountToSave);
            }
            else
            {
                availableAccounts.Remove(accountToUpdate);
                availableAccounts.Add(accountToSave);
            }

            // Then, call the data service to save all the accounts
            var savedSuccessfully = dataService.SaveAllAccounts(availableAccounts);
            if (savedSuccessfully)
            {
                Console.WriteLine("Bye! Thanks for banking with us.)");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Sorry, we are unable to save your account information at this time.)");
                Console.WriteLine();
            }
        }

    }
}


/*
    Refactor the account number generation logic to use random number generator
    Define an Account record type in the Models folder
    Use this Account record type, not the BankAccount class, for serialization, storage and deserialization of bank accounts
    Have a second constructor in BankAccount class for instantiating BankAccount from an Account record argument
    Refactor the Main method to load existing accounts or selected account using the new Account record type
    When saving the accounts back to file, do a mapping/conversion from BankAccount to Account
*/



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