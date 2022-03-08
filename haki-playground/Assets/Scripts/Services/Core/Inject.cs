using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Assets.Scripts.Shared.SystemExtensions;

namespace Assets.Scripts.Services.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Inject : Attribute
    {
        private class InjectPropertyData
        {
            private readonly PropertyInfo info;

            public InjectPropertyData(PropertyInfo info, object[] injectData)
            {
                if (info == null || injectData == null)
                    throw new NullReferenceException("Parameters cannot be null!");

                this.info = info;
                InjectData = injectData;
            }

            public void SetValue(object instance, object value)
            {
                info.SetValue(instance, value);
            }

            public object GetValue(object instance)
            {
                return info.GetValue(instance);
            }
            public object[] InjectData { get; }
            public Type PropertyType => info.PropertyType;
        }

        private const BindingFlags Default = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        private readonly object[] data;
        [Obsolete("While this feature is not obsolete, use it with caution, the constructor of service needs to be able to take this parameter as an argument.\n' " +
                  "Keep in mind also that parameters ")]
        public Inject(object item0, object item1) : this(new[] { item0, item1 }) { }

        [Obsolete("While this feature is not obsolete, use it with caution, the constructor of service needs to be able to take this parameter as an argument")]
        public Inject(object item) : this(new[] { item })
        {

        }

        private Inject(/*DO NOT MAKE IT "param"! don't be lazy...*/object[] data)
        {
            this.data = data;

            if (data.Select(x => x.GetType()).Distinct().Count() != data.Length)
            {
                throw new NotSupportedException("Multiple instances of the same type are not supported as Inject parameters");
            }
        }
        public Inject() : this(Array.Empty<object>())
        {

        }


        private static bool TryGetProperties(IReflect type, out IEnumerable<InjectPropertyData> properties)
        {
            List<InjectPropertyData> pps = new List<InjectPropertyData>();

            PropertyInfo[] propertyInfos = type.GetProperties(Default);

            foreach (PropertyInfo info in propertyInfos)
            {
                if (IsInjectProperty(info, out Inject inject))
                {
                    pps.Add(new InjectPropertyData(info, inject.data));
                }
            }

            properties = pps;

            return properties.Any();
        }

        private static bool IsInjectProperty(PropertyInfo info, out Inject inject)
        {
            inject = info.GetCustomAttribute<Inject>();

            return inject != null;
        }

        public static bool CreateProcedureList(IReflect type, out IEnumerable<DepInjectionProcedure> procedures)
        {
            if (Inject.TryGetProperties(type, out IEnumerable<InjectPropertyData> properties))
            {
                procedures = properties.Select(property => new DepInjectionProcedure(property.SetValue, property.GetValue, property.PropertyType, property.InjectData));
                return true;
            }

            procedures = null;
            return false;
        }
    }
}