namespace UnityDurableTaskFramework.Tasks
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;

    public abstract class UnityContainerTaskActivity<TInput, TResult> : TaskActivity<TInput, TResult>, IUnityContainerObject
    {
        protected UnityContainerTaskActivity()
        {
        }

        public IUnityContainer UnityContainer 
        { 
            get; 
            set; 
        }
    }
}
