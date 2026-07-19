namespace EntityWebApi.Controllers.Interfaces
{
    public interface IControllerConfiguration
    {
        /// <summary>
        /// Adds function to resolve generic arguments from a type argument provided when adding a controller.
        /// </summary>
        public IControllerConfiguration SetDefaultArgumentResolver(Func<string, Type, Type> resolver);

        /// <summary>
        /// Adds function to resolve generic argument with specified name from a type argument provided when adding a controller.
        /// </summary>
        public IControllerConfiguration AddArgumentResolver(string argumentName, Func<Type, Type> resolver);

        /// <summary>
        /// Adds controller from generic controller for specified type argument.
        /// </summary>
        public IControllerConfiguration AddController<TArgument>(Type controller);

        /// <summary>
        /// Adds controllers from generic controller for specified type arguments.
        /// </summary>
        public IControllerConfiguration AddControllers(Type controller, IEnumerable<Type> arguments);
    }
}