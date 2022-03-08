using UnityEngine;

namespace Assets.Scripts.Shared.UnityExtensions
{
    public static class RayExtensions
    {
        public static bool IsVectorInFrontRay(this Ray ray, Vector3 point)
        {
            return Vector3.Dot(ray.direction, ray.origin.CalculateDirectionTo(point, true)) > 0;
        }

        public static bool IsTransformInFrontRay(this Ray ray, Transform transform)
        {
            return ray.IsVectorInFrontRay(transform.position);
        }

        public static bool IsVectorBehindRay(this Ray ray, Vector3 point)
        {
            return ray.IsVectorInFrontRay(point) == false;
        }

        public static bool IsTransformBehindRay(this Ray ray, Transform transform)
        {
            return ray.IsVectorBehindRay(transform.position);
        }

        public static float DotProduct(this Ray ray, Vector3 otherDirection)
        {
            return ray.direction.DotProduct(otherDirection);
        }

        public static Vector3 GetVectorClosestToRay(this Ray ray, Vector3 first, Vector3 second)
        {
            return ray.origin.GetVectorClosestToVector(first, second);
        }

        public static bool TryGetVectorClosestToRay(this Ray ray, Vector3 first, Vector3 second, out Vector3 result)
        {
            return ray.origin.TryGetVectorClosestToVector(first, second, out result);
        }

        public static bool IsDotProductGreaterOrEqual(this Ray ray, Vector3 other, float threshold)
        {
            return ray.DotProduct(other) >= threshold;
        }

        
    }
}