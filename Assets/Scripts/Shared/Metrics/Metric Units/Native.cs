namespace Assets.Scripts.Shared.Metrics.Metric_Units
{
    public class Native : Unit<float>
    {
        internal Native(float value) : base(value)
        {
        }

        public static explicit operator float(Native value)
        {
            return value.Value;
        }

        public static implicit operator Native(float value)
        {
            return new Native(value);
        }

        public static MilliMeter operator /(float v, Native value)
        {
            return new MilliMeter(v / value.Value);
        }

        public static MilliMeter operator *(float v, Native value)
        {
            return new MilliMeter(v * value.Value);
        }

        //public static CentiMeter operator /(float v, Native value)
        //{
        //    return new CentiMeter(v / value.Value);
        //}
        //public static CentiMeter operator *(float v, Native value)
        //{
        //    return new CentiMeter(v * value.Value);
        //}
    }
}