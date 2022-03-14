using System;

namespace Assets.Scripts.Services.Core
{
    public class DepInjectionProcedure
    {
        private readonly Action<object, object> setValue;
        private readonly Func<object, object> getValue;
        private readonly Type propertyType;
        private readonly object[] injectData;
        public DepInjectionProcedure(Action<object, object> setValue, Func<object, object> getValue, Type propertyType,
            object[] injectData)
        {
            if (setValue == null || getValue == null || propertyType == null || injectData == null)
                throw new NullReferenceException(Environment.StackTrace);

            this.injectData = injectData;
            this.setValue = setValue;
            this.getValue = getValue;
            this.propertyType = propertyType;

        }

        public void Execute(object item, Func<Type, object[], object> getDependency)
        {
            if (getValue(item) == null)
            {
                setValue(item, getDependency(propertyType,injectData));
            }
        }
    }
}