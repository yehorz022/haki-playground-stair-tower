using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Services.Core
{

    public class ServiceManager
    {
        private readonly Dictionary<Type, ServiceFactory> services = new Dictionary<Type, ServiceFactory>();
        private readonly Dictionary<Type, object> implementations = new Dictionary<Type, object>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            Type tInterface = typeof(TInterface);
            Type tImplementation = typeof(TImplementation);
            Register(tInterface, tImplementation);
        }

        private void Register(Type tInterface, Type tImplementation)
        {
            if (tInterface.IsAssignableFrom(tImplementation))
            {
                ConstructorInfo[] implementationConstructors = tImplementation.GetConstructors();

                ServiceFactory sf = new ReflectionInstanciation(implementationConstructors);

                services.Add(tInterface, sf);
            }
        }

        private object GetDependency(Type type)
        {
            if (implementations.TryGetValue(type, out object implementation))
            {
                return implementation;
            }

            return CreateDependency(type);
        }



        private object CreateDependency(Type type)
        {
            if (services.TryGetValue(type, out ServiceFactory serviceFactory))
            {
                if (serviceFactory is ReflectionInstanciation reflectionInstanciation)
                {
                    IEnumerable<Type> requiredTypes = new List<Type>(reflectionInstanciation.RequiredTypes);

                    object[] parameters = new object[requiredTypes.Count()];
                    int index = 0;
                    foreach (Type requiredType in requiredTypes)
                    {
                        parameters[index++] = GetDependency(requiredType);
                    }

                    object implementation = reflectionInstanciation.Implement(parameters);
                    implementations.Add(type, implementation);
                    return implementation;
                }
            }


            return default;
        }

        public T GetDependency<T>()
        {
            if (GetDependency(typeof(T)) is T createdValue)
            {
                return createdValue;
            }

            return default;

        }

        public void InjectDependencies(object item)
        {
            if (item == null)
            {
                return;
            }

            Type type = item.GetType();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (PropertyInfo property in props)
            {
                Inject inject = property.GetCustomAttribute<Inject>();

                if (inject == null)
                    continue;

                if (property.GetValue(item) == null)
                {
                    property.SetValue(item, GetDependency(property.PropertyType));
                }
            }
        }

        public void RegisterServicesFromAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsInterface)
                    continue;

                Service att = type.GetCustomAttribute<Service>();
                if (att == null)
                {
                    continue;
                }

                Register(att.Interface, type);
            }
        }

        /// <summary>
        /// Overrides existing implementation or creates new one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="instance"></param>
        public void DefineAs<T, T1>(T1 instance) where T1 : T
        {
            Type key = typeof(T);
            services.Remove(key);
            implementations.Remove(key);

            implementations.Add(typeof(T), instance);
        }
    }
}