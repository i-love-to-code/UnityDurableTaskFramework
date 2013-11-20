namespace UnityDurableTaskFramework.Tasks
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System;

    public class DebitAccountTask : UnityContainerTaskActivity<Transaction, bool>
    {
        protected override bool Execute(TaskContext context, Transaction input)
        {
            var bankAccount = this.UnityContainer.Resolve<IBankAccount>();

            var result = bankAccount.DebitAccount(input.Amount, input.SourceBankAccount);

            if (result)
            {
                Console.WriteLine("{0} debited from account number {1}", input.Amount, input.SourceBankAccount);
            }
            else
            {
                Console.WriteLine("Insufficient funds in account number {0} to debit {1}.", input.SourceBankAccount, input.Amount);
            }

            return result;
        }
    }
}
