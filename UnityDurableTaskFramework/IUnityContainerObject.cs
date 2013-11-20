namespace UnityDurableTaskFramework
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Interface defining the behavior of an object that uses a unity container.
    /// </summary>
    public interface IUnityContainerObject
    {
        /// <summary>
        /// Gets or sets the unity container.
        /// </summary>
        IUnityContainer UnityContainer
        {
            get;
            set;
        }
    }
}
