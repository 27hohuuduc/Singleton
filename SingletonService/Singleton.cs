using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test")]

namespace SingletonService
{
    internal class Singleton
    {
        private static readonly Dictionary<Type, object> _instances = [];

        public static T GetInstance<T>()
        {
            var type = typeof(T);
            return (T)GetInstance(type);
        }

        public static object GetInstance(Type type)
        {
            var instance = _instances.GetValueOrDefault(type);
            if (instance == null)
            {
                try
                {
                    instance = CreateInstance(type);
                }
                catch (NullableException)
                {
                    throw new Exception("The type must not be nullable");
                }
                _instances.Add(type, instance);
            }

            return instance;
        }

        /// <summary>
        /// Register an instance for the class; if it has already been registered, it will automatically override the previous instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <exception cref="NullReferenceException"></exception>
        public static void Register<T>(T obj)
        {
            if (obj != null)
                _instances[typeof(T)] = obj;
            else
                throw new NullReferenceException("The parameter must not to be null");
        }

        private static object CreateInstance(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length == 0)
                throw new ZeroIndexException();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    if (parameters.Length == 0)
                    {
                        var instance = Activator.CreateInstance(type);
                        if (instance != null)
                            return instance;
                        else
                            throw new NullableException();
                    }
                    else
                    {
                        var instances = new object[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            instances[i] = CreateInstance(parameters[0].ParameterType);
                        }
                        var instance = Activator.CreateInstance(type, instances);
                        if (instance != null)
                            return instance;
                        else
                            throw new NullableException();
                    }
                }
                catch (ZeroIndexException)
                {
                    continue;
                }
            }
            throw new MissingMethodException();
        }
    }

    class ZeroIndexException : Exception { }
    class NullableException : Exception { }
}