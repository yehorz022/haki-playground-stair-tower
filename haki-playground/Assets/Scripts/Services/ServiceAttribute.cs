using System;
using System.Runtime.CompilerServices;


namespace Assets.Scripts.Services
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
