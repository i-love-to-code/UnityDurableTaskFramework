namespace UnityDurableTaskFrameworkTests
{
    using DurableTaskTestFramework;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;
    using UnityDurableTaskFramework;
    using UnityDurableTaskFramework.Tasks;

    [TestClass]
    public class CreditAccountTaskTests
    {
        private readonly string connectionString;

        public CreditAccountTaskTests()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["Microsoft.ServiceBus.ConnectionString"].ConnectionString;
        }

        [TestMethod]
        public void CreditAccountSuccessful()
        {
            var unityContainer = new UnityContainer();
            var bankAccount = new BankAccount();
            
            unityContainer.RegisterInstance<IBankAccount>(bankAccount, new ContainerControlledLifetimeManager());

            var input = new Transaction() 
            { 
                SourceBankAccount = "123456", 
                TargetBankAccount = "654321", 
                Amount = 100M 
            };

            var durableTaskHelper = new DurableTaskHelper("CreditAccountTaskTest", this.connectionString);
            var taskHubClient = durableTaskHelper.SetupTaskHub<bool, Transaction, CreditAccountTask>(input, unityContainer);

            // Just to show you what the balances are before we run the orchestration.
            Assert.AreEqual(100000M, bankAccount.GetBalance("123456"));
            Assert.AreEqual(0M, bankAccount.GetBalance("654321"));

            durableTaskHelper.RunOrchestration();

            // We didn't debit anything, so the first account should be untouched...
            Assert.AreEqual(100000M, bankAccount.GetBalance("123456"));
            // Check that the transaction amount was credited...
            Assert.AreEqual(input.Amount, bankAccount.GetBalance("654321"));
        }
    }
}
