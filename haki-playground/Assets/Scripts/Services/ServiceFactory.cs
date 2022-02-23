using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Shared.Constants;
using UnityEngine;

namespace Assets.Scripts.Services
{
    internal class ServiceFactory
    {
        private readonly HashSet<Type> types;
        private readonly Func<object[], object> factory;

        public IEnumerable<Type> RequiredTypes => types;

        internal ServiceFactory(IReadOnlyCollection<ConstructorInfo> constructors)
        {

            if (constructors.Count > 2)
            {
                Debug.LogError(Constants.TooManyConstructorsException);
                throw new Exception(Constants.TooManyConstructorsException);
            }
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] parameters = info.GetParameters();

                if (parameters.Length == 0 && constructors.Count > 1)
                    continue;

                types = new HashSet<Type>();
                factory = info.Invoke;


                foreach (ParameterInfo parameterInfo in info.GetParameters())
                {
                    types.Add(parameterInfo.ParameterType);
                }

                break;
            }
        }

        internal object Implement(object[] parameters)
        {
            if (Validate(parameters))
            {
                return factory.Invoke(parameters);
            }

            Debug.LogError(Constants.ImproperParameters);
            throw new Exception(Constants.ImproperParameters);
        }

        private bool Validate(object[] parameters)
        {
            if (parameters.Length != types.Count)
                return false;

            int count = parameters.Length;

            foreach (object parameter in parameters)
            {
                Type objType = parameter.GetType();
                if (types.Any(type => type.IsAssignableFrom(objType)))
                {
                    count--;
                }
            }

            return count == 0;
        }
    }
}