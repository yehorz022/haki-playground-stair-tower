using System;

namespace Assets.Scripts.Services.Core
{
    public class DepInjectionProcedure
    {
        private readonly Action<object, object> setValue;
        private readonly Func<object, object> getValue;
        private readonly Type propertyType;

        public DepInjectionProcedure(Action<object, object> setValue, Func<object, object> getValue, Type propertyType)
        {
            if (setValue == null || getValue == null || propertyType == null)
                throw new NullReferenceException(Environment.StackTrace);

            this.setValue = setValue;
            this.getValue = getValue;
            this.propertyType = propertyType;
        }

        public void Execute(object item, Func<Type, object> getDependency)
        {
            if (getValue(item) == null)
            {
                setValue(item, getDependency(propertyType));
            }
        }
    }
}