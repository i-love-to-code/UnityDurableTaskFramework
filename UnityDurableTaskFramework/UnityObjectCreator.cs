namespace UnityDurableTaskFramework
{
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceBus.DurableTask;
    using System;
    using System.Dynamic;
    using System.Reflection;

    /// <summary>
    /// Factory used to create durable task framework objects. If the type <typeparamref name="T"/> implements
    /// <see cref="IUnityContainerObject"/>, the property <see cref="IUnityContainerObject.UnityContainer"/> 
    /// property is set during creation.
    /// </summary>
    /// <typeparam name="T">
    ///     The object type to create. Should be either <see cref="TaskActivity"/> 
    ///     or <see cref="TaskOrchestration"/>
    /// </typeparam>
    public class UnityObjectCreator<T> : ObjectCreator<T> where T : class
    {
        #region [ Fields ]
        
        private Type prototype;
        private T instance;
        private IUnityContainer unityContainer;

        #endregion

        #region [ Constructors ]
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityObjectCreator"/> class.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="type">The object type to be created.</param>
        public UnityObjectCreator(IUnityContainer unityContainer, Type type)
        {
            this.prototype = type;
            this.Initialize((object)type);
            this.unityContainer = unityContainer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityObjectCreator"/> class.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="instance">The instance to be returned.</param>
        public UnityObjectCreator(IUnityContainer unityContainer, T instance)
        {
            this.instance = instance;
            this.Initialize(instance);
            this.unityContainer = unityContainer;
        }

        #endregion

        #region [ Public Methods ]
        
        public override T Create()
        {
            T result;

            if (this.instance == null)
            {
                result = Activator.CreateInstance(this.prototype) as T;
            }
            else
            {
                result = this.instance;
            }

            var unityOrchestration = result as IUnityContainerObject;

            if (unityOrchestration != null)
            {
                unityOrchestration.UnityContainer = this.unityContainer;
            }

            return result;
        }

        #endregion

        #region [ Private Methods ]
        
        private void Initialize(object obj)
        {
            this.Name = GetDefaultName(obj);
            this.Version = string.Empty;
        }

        private string GetDefaultName(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            string str = string.Empty;
            Type type;
            MethodInfo methodInfo;
            InvokeMemberBinder invokeMemberBinder;

            return !((type = obj as Type) != (Type)null) 
                        ? (!((methodInfo = obj as MethodInfo) != (MethodInfo)null) 
                            ? ((invokeMemberBinder = obj as InvokeMemberBinder) == null 
                                ? obj.GetType().ToString() 
                                : invokeMemberBinder.Name) 
                            : methodInfo.Name) 
                        : type.ToString();
        }

        #endregion
    }
}
