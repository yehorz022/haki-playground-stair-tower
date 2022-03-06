using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Assets.Scripts.Services.Factories;
using Assets.Scripts.Services.Instanciation;

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
            if (services.TryGetValue(type, out ReflectionServiceFactory serviceFactory) == false) 
                return default;


            IEnumerable<Type> requiredTypes = new List<Type>(serviceFactory.RequiredTypes);

            object[] parameters = new object[requiredTypes.Count()];
            int index = 0;
            foreach (Type requiredType in requiredTypes)
            {
                parameters[index++] = GetDependency(requiredType);
            }

            object implementation = serviceFactory.Implement(parameters);
            implementations.Add(type, implementation);
            return implementation;
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