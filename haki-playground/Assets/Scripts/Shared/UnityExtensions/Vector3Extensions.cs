using UnityEngine;

namespace Assets.Scripts.Shared.UnityExtensions
{
    public static class Vector3Extensions 
    {

        public static Vector3 GetDirectionTo(this Vector3 origin, Vector3 end)
        {
            return origin.GetDirectionTo(end, false);
        }

        public static Vector3 GetDirectionTo(this Vector3 origin, Vector3 end, bool normalize)
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
    }
}