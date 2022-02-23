namespace Assets.Scripts.Extensions.SystemExtensions
{
    public static class IntExtensions
    {
        private const bool DefaultMinInclusive = true;
        private const bool DefaultMaxInclusive = false;


        /// <summary>
        /// returns the value of value.IsValueBetween(min, max, true, maxInclusive);
        /// </summary>
        /// <param name="maxInclusive">Determines wheather max value should be considered as part of range or not.</param>
        /// <returns></returns>
        public static bool IsValueBetween(this int value, int min, int max, bool maxInclusive)
        {
            return value.IsValueBetween(min, max, DefaultMinInclusive, maxInclusive);
        }
        /// <summary>
        /// returns the value of value.IsValueBetween(min, max, true, false);
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsValueBetween(this int value, int min, int max)
        {
            return value.IsValueBetween(min, max, DefaultMinInclusive, DefaultMaxInclusive);
        }

        public static bool IsValueBetween(this int value, int min, int max, bool minInclusive, bool maxInclusive)
        {
            if (value < max && value > min)
                return true;

            if (max == value && maxInclusive)
                return true;

            if (min == value && minInclusive)
                return true;

            return false;
        }
    }
}