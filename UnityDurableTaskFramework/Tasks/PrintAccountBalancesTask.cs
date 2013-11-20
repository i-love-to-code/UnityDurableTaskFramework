namespace UnityDurableTaskFramework.Tasks
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System;

    public class PrintAccountBalancesTask : UnityContainerTaskActivity<Transaction, bool>
    {
        protected override bool Execute(TaskContext context, Transaction input)
        {
            var bankAccount = this.UnityContainer.Resolve<IBankAccount>();

            Console.WriteLine();

            Console.WriteLine("Account {0} balance: {1}", input.SourceBankAccount, bankAccount.GetBalance(input.SourceBankAccount));
            Console.WriteLine("Account {0} balance: {1}", input.TargetBankAccount, bankAccount.GetBalance(input.TargetBankAccount));

            Console.WriteLine();

            Console.WriteLine("Press any key to quit.");

            return true;
        }
    }
}
