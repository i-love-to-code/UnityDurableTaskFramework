namespace DurableTaskTestFramework
{
    using Microsoft.ServiceBus.DurableTask;
    using System.Threading.Tasks;

    public class TestOrchestration<TResult, TInput, TTaskType> 
        : TaskOrchestration<TResult, TInput> where TTaskType : TaskActivity
    {
        #region [ Constructors ]
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TestOrchestration"/> class.
        /// </summary>
        public TestOrchestration()
        {
        }

        #endregion

        public override async Task<TResult> RunTask(OrchestrationContext context, TInput input)
        {
            return await context.ScheduleTask<TResult>(typeof(TTaskType), input);
        }
    }
}
