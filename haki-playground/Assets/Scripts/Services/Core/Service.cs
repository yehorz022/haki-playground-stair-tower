using System;
using System.ComponentModel;
using System.Reflection;

namespace Assets.Scripts.Services.Core
{
    [AttributeUsage(AttributeTargets.Class)]

    public sealed class Service : Attribute
    {

        public Type[] SupportedTypes { get; }

        public Service(Type type)
        {
            SupportedTypes = new Type[] { type };
        }

        public Service(Type type0, Type type1)
        {
            SupportedTypes = new Type[]
            {
                type0,
                type1
            };
        }

        public Service(Type type0, Type type1, Type type2)
        {
            SupportedTypes = new Type[]
            {
                type0,
                type1,
                type2,
            };
        }

        [Obsolete("No constructor with this number of type parameters was found! Please implement one in " + nameof(Service) + " class.", true)]
        public Service(params Type[] types)
        {
            SupportedTypes = types;
        }

        public static bool TryGetService(Type type, out Service service)
        {
            service = type.GetCustomAttribute<Service>();
            return service != null;
        }
    }
}
