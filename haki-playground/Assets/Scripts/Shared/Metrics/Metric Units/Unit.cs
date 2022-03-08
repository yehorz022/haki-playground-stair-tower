using System.Diagnostics;

namespace Assets.Scripts.Shared.Metrics.Metric_Units
{
    [DebuggerDisplay("{Value}")]
    public class Unit<T>
    {
        protected T Value { get; set; }

        protected Unit(T value)
        {
            Value = value;
        }


        public static implicit operator T(Unit<T> v)
        {
            return v.Value;
        }

        public static implicit operator Unit<T>(T value)
        {
            return new Unit<T>(value);
        }
    }
}