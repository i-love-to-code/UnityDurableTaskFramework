namespace DurableTaskTestFramework
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System.Threading;
    using UnityDurableTaskFramework;

    public class DurableTaskHelper
    {
        #region [ Fields ]
        
        private readonly string taskHubName;
        private readonly string connectionString;

        private TaskHubClient client;
        private TaskHubWorker worker;

        #endregion

        #region [ Constructors ]
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DurableTaskHelper"/> class.
        /// </summary>
        /// <param name="taskHubName">The name for the task hub.</param>
        /// <param name="connectionString">The service bus connection string.</param>
        public DurableTaskHelper(string taskHubName, string connectionString)
        {
            this.taskHubName = taskHubName;
            this.connectionString = connectionString;
        }

        #endregion

        #region [ Public Methods ]
        
        /// <summary>
        /// Creates and sets up a <see cref="TaskHubClient"/>, <see cref="TaskHubWorker"/> and a <see cref="TestOrchestration"/> to host the test activity, <paramref name="taskActivity"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the task activity being tested.</typeparam>
        /// <typeparam name="TInput">The type of the input value for the test activity.</typeparam>
        /// <typeparam name="TTaskType">The type of the task activity being tested.</typeparam>
        /// <param name="input">The input in to the orchestration and task activity being tested.</param>
        /// <param name="unityContainer">The unity container to be passed to the test orchestration and task activity.</param>
        /// <returns>A reference to the <see cref="TaskHubClient"/> instance created.</returns>
        public TaskHubClient SetupTaskHub<TResult, TInput, TTaskType>(TInput input, IUnityContainer unityContainer) where TTaskType : TaskActivity
        {
            this.client = new TaskHubClient(this.taskHubName, this.connectionString);
            this.worker = new TaskHubWorker(this.taskHubName, this.connectionString);

            this.worker.CreateHub();

            var orchestration = this.client.CreateOrchestrationInstance(typeof(TestOrchestration<TResult, TInput, TTaskType>), input);

            var orchestrationFactory = new UnityObjectCreator<TaskOrchestration>(unityContainer, typeof(TestOrchestration<TResult, TInput, TTaskType>));
            var taskFactory = new UnityObjectCreator<TaskActivity>(unityContainer, typeof(TTaskType));

            this.worker.AddTaskOrchestrations(orchestrationFactory);
            this.worker.AddTaskActivities(taskFactory);
            
            return this.client;
        }

        /// <summary>
        /// Starts the task hub worker.
        /// </summary>
        public void RunOrchestration()
        {
            this.worker.Start();

            while (this.client.GetPendingOrchestrationsCount() > 0)
            {
                Thread.Sleep(500);
            }

            this.worker.Stop(true);
        }

        #endregion
    }
}
