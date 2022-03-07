using Assets.Scripts.Shared.SystemExtensions;
using UnityEngine;

namespace Assets.Scripts.Shared.UnityExtensions
{
    public static class Vector3Extensions
    {

        public static Vector3 CalculateNormalizedDirectionTo(this Vector3 origin, Vector3 end)
        {
            return origin.CalculateDirectionTo(end, true);
        }

        public static Vector3 CalculateDirectionTo(this Vector3 origin, Vector3 end, bool normalize)
        {
            Vector3 res = end - origin;
            if (normalize)
                res.Normalize();

            return res;
        }

        public static float DotProduct(this Vector3 item, Vector3 other)
        {
            return Vector3.Dot(item, other);
        }

        public static Vector3 CrossProduct(this Vector3 item, Vector3 other)
        {
            return Vector3.Cross(item, other);
        }

        public static Vector3 Max()
        {
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        public static float MagnitudeSquared(this Vector3 vec)
        {
            return vec.x.Pow2() + vec.y.Pow2() + vec.z.Pow2();
        }

        public static bool IsVectorCloser(this Vector3 origin, Vector3 first, Vector3 second, out Vector3 result)
        {
            float f = origin.CalculateNormalizedDirectionTo(first).sqrMagnitude;
            float s = origin.CalculateNormalizedDirectionTo(second).sqrMagnitude;

            if (f > s)
            {
                result = second;
                return true;
            }
            result = first;
            return false;
        }

        public static Vector3 GetCloserVector(this Vector3 origin, Vector3 first, Vector3 second)
        {
            float f = origin.CalculateDirectionTo(first, false).MagnitudeSquared();
            float s = origin.CalculateDirectionTo(second, false).MagnitudeSquared();

            return s < f ? second : first;
        }
    }
}