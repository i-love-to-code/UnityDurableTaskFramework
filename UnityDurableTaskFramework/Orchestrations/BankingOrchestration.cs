namespace UnityDurableTaskFramework.Orchestrations
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System.Threading.Tasks;
    using UnityDurableTaskFramework.Tasks;

    public class BankingOrchestration : TaskOrchestration<bool, Transaction>, IUnityContainerObject
    {
        #region [ Constructors ]
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BankingOrchestration"/> class.
        /// </summary>
        public BankingOrchestration()
        {
        }

        #endregion

        #region [ Public Properties ]
        
        /// <summary>
        /// Gets or sets the unity container.
        /// </summary>
        public IUnityContainer UnityContainer
        {
            get;
            set;
        }

        #endregion

        public override async Task<bool> RunTask(OrchestrationContext context, Transaction input)
        {
            var result = await context.ScheduleTask<bool>(typeof(DebitAccountTask), input);

            if (!result)
            {
                return false;
            }

            await context.ScheduleTask<bool>(typeof(CreditAccountTask), input);

            await context.ScheduleTask<bool>(typeof(PrintAccountBalancesTask), input);

            return true;
        }
    }
}
