using UnityEngine;

namespace Assets.Scripts.Shared.SystemExtensions
{
    public static class FloatExtensions
    {
        private const float power2 = 2f;

        public static float Pow2(this float value)
        {
            return value.Pow(power2);
        }

        public static float Pow(this float value, float power)
        {
            return Mathf.Pow(value, power);
        }
    }
}