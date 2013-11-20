namespace UnityDurableTaskFramework
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System;
    using System.Configuration;
    using UnityDurableTaskFramework.Orchestrations;
    using UnityDurableTaskFramework.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new unity container and register our types.
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IBankAccount, BankAccount>(new ContainerControlledLifetimeManager());

            var isValidAmount = false;
            var amount = 0M;

            while (!isValidAmount)
            {
                Console.Write("Please enter an amount to transfer or Q to quit: ");
                var input = Console.ReadLine();

                if (input == "Q")
                {
                    return;
                }

                isValidAmount = decimal.TryParse(input, out amount);
            }

            Console.WriteLine("Processing...");

            // Now setup our orchestration
            var connectionString = ConfigurationManager.ConnectionStrings["Microsoft.ServiceBus.ConnectionString"].ConnectionString;
            var taskHubName = ConfigurationManager.AppSettings["taskHubName"];

            var taskHubClient = new TaskHubClient(taskHubName, connectionString);
            var taskHub = new TaskHubWorker(taskHubName, connectionString);

            taskHub.CreateHub();

            var transaction = new Transaction() { Amount = amount, SourceBankAccount = "123456", TargetBankAccount = "654321" };

            var orchestration = taskHubClient.CreateOrchestrationInstance(typeof(BankingOrchestration), transaction);

            taskHub.AddTaskOrchestrations(new UnityObjectCreator<TaskOrchestration>(unityContainer, typeof(BankingOrchestration)));

            taskHub.AddTaskActivities(new UnityObjectCreator<TaskActivity>(unityContainer, typeof(CreditAccountTask)),
                new UnityObjectCreator<TaskActivity>(unityContainer, typeof(DebitAccountTask)),
                new UnityObjectCreator<TaskActivity>(unityContainer, typeof(PrintAccountBalancesTask)));

            taskHub.Start();

            Console.ReadLine();

            taskHub.Stop(true);
        }
    }
}
