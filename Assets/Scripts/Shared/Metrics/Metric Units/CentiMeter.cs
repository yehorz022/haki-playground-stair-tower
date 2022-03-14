namespace Assets.Scripts.Shared.Metrics.Metric_Units
{
    public class CentiMeter : Unit<float>
    {
        /// <inheritdoc />
        internal CentiMeter(float value) : base(value)
        {
        }

        public static explicit operator float(CentiMeter value)
        {
            return value.Value;
        }

        public static implicit operator CentiMeter(float value)
        {
            return new CentiMeter(value);
        }

        public static Native operator /(float v, CentiMeter value)
        {
            return new Native(v / value.Value);
        }

        public static Native operator *(float v, CentiMeter value)
        {
            return new Native(v * value.Value);
        }
    }
}