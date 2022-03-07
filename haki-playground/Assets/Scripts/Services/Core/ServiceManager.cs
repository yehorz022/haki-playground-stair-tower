using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Services.Core
{
    public class ServiceManager
    {
        private readonly Dictionary<Type, ReflectionServiceFactory> services = new Dictionary<Type, ReflectionServiceFactory>();

        private readonly Dictionary<Type, IEnumerable<DepInjectionProcedure>> injectionProcedures =
            new Dictionary<Type, IEnumerable<DepInjectionProcedure>>();
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
                services.Add(tInterface,
                    new ReflectionServiceFactory(
                        tImplementation.GetConstructors()));
            }
        }

        [Obsolete("is obsolete, do not use")]
        public T GetDependency<T>()
        {
            return GetDependency<T>(Array.Empty<object>());
        }

        private object GetDependency(Type type, object[] injectData)
        {

            if (implementations.TryGetValue(type, out object implementation))
            {
                return implementation;
            }

            return CreateDependency(type, injectData);
        }

        private object CreateDependency(Type type, object[] injectData)
        {
            if (services.TryGetValue(type, out ReflectionServiceFactory serviceFactory) == false)
                return default;


            object[] parameters = CreateConstructorParameters(type, injectData, serviceFactory);
            object implementation = serviceFactory.Implement(parameters);

            implementations.Add(type, implementation);

            return implementation;
        }

        private object[] CreateConstructorParameters(Type type, object[] injectData, ReflectionServiceFactory serviceFactory)
        {
            IList<Type> requiredTypes = new List<Type>(serviceFactory.RequiredTypes);

            IDictionary<Type, object> injected = injectData.ToDictionary(x => x.GetType());

            object[] parameters = new object[requiredTypes.Count];
            int index = 0;

            foreach (Type requiredType in requiredTypes)
            {
                if (injected.TryGetValue(requiredType, out object dependency) == false)
                {
                    dependency = GetDependency(requiredType, Array.Empty<object>());
                }

                if (ValidateDependency(dependency, () => $"Dependency of type {requiredType} required by {type.Name} could not been found!"))
                {
                    parameters[index++] = dependency;
                }
            }

            return parameters;
        }

        private static bool ValidateDependency(object dependency, Func<string> func)
        {
            if (dependency == null)
            {
                string message = func(); ;
                Debug.LogError(message);
                throw new NullReferenceException(message);
            }

            return true;
        }

        public T GetDependency<T>(object[] injectData)
        {
            if (GetDependency(typeof(T), injectData) is T createdValue)
            {
                return createdValue;
            }

            return default;
        }

        public void InjectDependencies(object item)
        {
            if (item == null)
                return;

            Type type = item.GetType();

            if (injectionProcedures.TryGetValue(type, out IEnumerable<DepInjectionProcedure> procedures) == false)
            {
                if (Inject.CreateProcedureList(type, out procedures))
                {
                    injectionProcedures.Add(type, procedures);
                }
            }

            foreach (DepInjectionProcedure procedure in procedures)
            {
                procedure.Execute(item, GetDependency);
            }
        }



        public void RegisterServicesFromAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsInterface)
                    continue;

                if (Service.TryGetService(type, out Service att) == false)
                    continue;

                foreach (Type current in att.SupportedTypes)
                {
                    Register(current, type);
                }
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