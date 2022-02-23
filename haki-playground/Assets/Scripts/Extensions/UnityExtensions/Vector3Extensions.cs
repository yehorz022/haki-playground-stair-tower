using UnityEngine;

namespace Assets.Scripts.Extensions.UnityExtensions
{
    public static class Vector3Extensions 
    {

        public static Vector3 GetDirectionTo(this Vector3 origin, Vector3 end)
        {
            return origin.GetDirectionTo(end, false);
        }

        public static Vector3 GetDirectionTo(this Vector3 origin, Vector3 end, bool normalize)
        {
            var res = end - origin;
            if (normalize)
                res.Normalize();

            return res;
        }
    }
}