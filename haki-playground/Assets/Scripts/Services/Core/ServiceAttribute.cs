using System;

namespace Assets.Scripts.Services.Core
{
    public class Service : Attribute
    {
        public Service(Type @interface)
        {
            Interface = @interface;
        }

        public Type Interface { get; }
    }
}
