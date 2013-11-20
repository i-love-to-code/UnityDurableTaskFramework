namespace UnityDurableTaskFramework.Tasks
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System;

    public class CreditAccountTask : UnityContainerTaskActivity<Transaction, bool>
    {
        protected override bool Execute(TaskContext context, Transaction input)
        {
            var bankAccount = this.UnityContainer.Resolve<IBankAccount>();

            var result = bankAccount.CreditAccount(input.Amount, input.TargetBankAccount);

            if (result)
            {
                Console.WriteLine("{0} credited to account number {1}", input.Amount, input.TargetBankAccount);
            }
            else
            {
                Console.WriteLine("Unable to credit {0} to account number {1}.", input.Amount, input.TargetBankAccount);
            }

            return result;
        }
    }
}
