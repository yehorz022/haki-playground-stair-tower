using UnityEngine;

namespace Assets.Scripts.Shared.UnityExtensions
{
    public static class RayExtensions
    {

        public static bool IsVectorInFront(this Ray ray, Vector3 point)
        {
            return Vector3.Dot(ray.direction, ray.origin.GetDirectionTo(point, true)) > 0;
        }

        public static bool IsVectorBehind(this Ray ray, Vector3 point)
        {
            return ray.IsVectorInFront(point) == false;
        }

        public static float DotProduct(this Ray ray, Vector3 otherDirection)
        {
            return ray.direction.DotProduct(otherDirection);
        }

        public static bool IsDotProductGreaterOrEqual(this Ray ray, Vector3 other, float threshold)
        {
            return ray.DotProduct(other) >= threshold;
        }
    }
}