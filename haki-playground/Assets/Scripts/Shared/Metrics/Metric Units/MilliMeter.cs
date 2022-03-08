namespace Assets.Scripts.Shared.Metrics.Metric_Units
{
    public class MilliMeter : Unit<float>
    {
        /// <inheritdoc />
        internal MilliMeter(float value) : base(value)
        {
        }

        public static explicit operator float(MilliMeter value)
        {
            return value.Value;
        }

        public static implicit operator MilliMeter(float value)
        {
            return new MilliMeter(value);
        }

        public static Native operator /(float v, MilliMeter value)
        {
            return new Native(v / value.Value);
        }

        public static Native operator *(float v, MilliMeter value)
        {
            return new Native(v * value.Value);
        }
    }
}