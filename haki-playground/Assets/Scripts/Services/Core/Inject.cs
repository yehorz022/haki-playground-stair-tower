using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Shared.SystemExtensions;

namespace Assets.Scripts.Services.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Inject : Attribute
    {
        private const BindingFlags Default = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        private static bool TryGetProperties(IReflect type, out IEnumerable<PropertyInfo> properties)
        {
            properties = type.GetProperties(Default).Where(IsInjectProperty);

            return properties.Any();
        }

        private static bool IsInjectProperty(PropertyInfo info)
        {
            return info.GetCustomAttribute<Inject>() != null;
        }

        public static bool CreateProcedureList(IReflect type, out IEnumerable<DepInjectionProcedure> procedures)
        {
            if (Inject.TryGetProperties(type, out IEnumerable<PropertyInfo> properties))
            {
                procedures = properties.Select(property => new DepInjectionProcedure(property.SetValue, property.GetValue, property.PropertyType));
                return true;
            }

            procedures = null;
            return false;
        }
    }
}