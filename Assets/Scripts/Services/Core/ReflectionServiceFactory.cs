using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Shared.Constants;
using UnityEngine;

namespace Assets.Scripts.Services.Core
{
    internal sealed class ReflectionServiceFactory
    {
        private readonly HashSet<Type> types;
        private readonly Func<object[], object> constructor;
        public IEnumerable<Type> RequiredTypes => types;

        internal ReflectionServiceFactory(IReadOnlyCollection<ConstructorInfo> constructors)
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
                constructor = info.Invoke;


                foreach (ParameterInfo parameterInfo in info.GetParameters())
                {
                    types.Add(parameterInfo.ParameterType);
                }

                break;
            }

            if (constructor == null)
                throw new NullReferenceException(Constants.ReflectionServiceFactoryIsNull);
            if (types == null)
                throw new NullReferenceException(Constants.ReflectionServiceFactoryTypesIsNull);
        }

        internal object Implement(object[] parameters)
        {
            if (Validate(parameters))
            {
                return constructor(parameters);
            }

            Debug.LogError(Constants.ImproperParameters);
            throw new Exception(Constants.ImproperParameters);
        }

        private bool Validate(IReadOnlyCollection<object> parameters)
        {
            if (parameters.Count != types.Count)
                return false;

            int count = parameters.Count;

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