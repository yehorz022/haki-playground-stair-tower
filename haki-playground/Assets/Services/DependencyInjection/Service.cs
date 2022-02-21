using System;

namespace Assets.Services.DependencyInjection
{
    public sealed class Service : Attribute
    {
        public Service(Type @interface)
        {
            Interface = @interface;
        }

        public Type Interface { get; set; }
    }
}