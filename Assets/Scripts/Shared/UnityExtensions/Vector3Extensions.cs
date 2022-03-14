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

        public static float CalculateSquareDistanceTo(this Vector3 origin, Vector3 target)
        {
            return origin.CalculateDirectionTo(target, false).sqrMagnitude;
        }

        public static bool TryGetVectorClosestToVector(this Vector3 origin, Vector3 first, Vector3 second, out Vector3 result)
        {
            float f = origin.CalculateSquareDistanceTo(first);
            float s = origin.CalculateSquareDistanceTo(second);

            if (f > s)
            {
                result = second;
                return true;
            }
            result = first;
            return false;
        }

        public static bool IsVectorCloserThan(this Vector3 origin, Vector3 candidate, Vector3 current)
        {
            return origin.TryGetVectorClosestToVector(candidate, current, out Vector3 ignore);
        }

        public static Vector3 GetVectorClosestToVector(this Vector3 origin, Vector3 first, Vector3 second)
        {
            origin.TryGetVectorClosestToVector(first, second, out Vector3 res);
            return res;
        }
    }
}