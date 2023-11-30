using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using MiniBankApp2.Enums;
using MiniBankApp2.Models;
using MiniBankApp2.Interfaces;
using MiniBankApp2.Implementations;

namespace MiniBankApp2.Tests.Implementations
{
    public class BankAccountTests
    {

        // TODO:
        // Add other tests for Deposit(), Withdraw(), AddBeneficiary() and DisplayBalance()


        [Fact]
        public void PrintHistory_CallsReportServicePrintAccountHistoryMethod()
        {
            // Arrange
            
            // Create a mock for the dependency
            Mock<IReportService> mockReportService = new Mock<IReportService>();

            // Configure behaviours for the mocked dependency
            mockReportService.Setup(m => m.PrintAccountHistory(It.IsAny<IList<Transaction>>()));
            mockReportService.Setup(m => m.PrintAccountBeneficiaries(It.IsAny<IList<Beneficiary>>()));

            // Instantiate the class being tested
            BankAccount bankAccount = new BankAccount("Paul", "Konyefa", 50000 , AccountType.Savings, mockReportService.Object);

            // Act
            bankAccount.PrintHistory();

            // Assert
            mockReportService.Verify(m => m.PrintAccountHistory(It.IsAny<IList<Transaction>>()));
            //mockReportService.Verify(m => m.PrintAccountBeneficiaries(It.IsAny<IList<Beneficiary>>()));
        }


        [Fact]
        public void ViewBeneficiaries_CallsReportServicePrintAccountViewBeneficiariesMethod()
        {
            // Arrange

            // Create a mock for the dependency
            Mock<IReportService> mockReportService = new Mock<IReportService>();

            // Configure behaviours for the mocked dependency
            mockReportService.Setup(m => m.PrintAccountHistory(It.IsAny<IList<Transaction>>()));
            mockReportService.Setup(m => m.PrintAccountBeneficiaries(It.IsAny<IList<Beneficiary>>()));

            // Instantiate the class being tested
            BankAccount bankAccount = new BankAccount("Paul", "Konyefa", 50000, AccountType.Savings, mockReportService.Object);

            // Act
            bankAccount.ViewBeneficiaries();

            // Assert
            //mockReportService.Verify(m => m.PrintAccountHistory(It.IsAny<IList<Transaction>>()));
            mockReportService.Verify(m => m.PrintAccountBeneficiaries(It.IsAny<IList<Beneficiary>>()));
        }

        //[Fact]
        //public void ViewBeneficiaries_Scenario1_Outcome()
        //{

        //}
    }
}
