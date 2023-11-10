using MiniBankApp2.Enums;
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
        List<BankAccount> availableAccounts = dataService.FetchAllAccounts();
        BankAccount activeBankAccount = new BankAccount(string.Empty, string.Empty, 0, AccountType.Unknown, reportService); 

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
                return;
            }

            activeBankAccount = matchingAccount;
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
                Console.WriteLine("Welcome! What would you like to do?\n1. Deposit Funds 2. Withdraw cash 3. View Statement  4. Add beneficiary  5. View beneficiaries");
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
            var accountToUpdate = availableAccounts.Find(x => x.AccountNumber == activeBankAccount.AccountNumber);
            if (accountToUpdate == null)
            {
                availableAccounts.Add(activeBankAccount);
            }
            else
            {
                availableAccounts.Remove(accountToUpdate);
                availableAccounts.Add(activeBankAccount);
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