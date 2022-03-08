using Assets.Scripts.Shared.UnityExtensions;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public static class Intersections
    {
        /// <summary>
        /// This function is sensitive to the order of points inserted. points a,b,c and d should
        /// inserted clock-wise along the face normal vector.
        /// </summary>
        /// <param name="point">Point that is suspected to be on the quad.</param>
        /// <param name="quad">Quad containing 4 points, points A, B, C and D should inserted clock-wise along the normal vector of this quad.</param>
        /// <returns>returns true if point is inside the quad ( or on the edge) false if it is outside of it.</returns>
        public static bool CheckIfPointIsOnQuad(Vector3 point, Quad quad)
        {
            return CheckIfPointIsOnQuad(point, quad.A, quad.B, quad.C, quad.D);
        }

        /// <summary>
        /// This function is sensitive to the order of points inserted. points a,b,c and d should
        /// inserted clock-wise along the face normal vector.
        /// </summary>
        /// <param name="point">Point that is suspected to be on the quad.</param>
        /// <param name="a">First point</param>
        /// <param name="b">Second point</param>
        /// <param name="c">Third point</param>
        /// <param name="d">Forth point</param>
        /// <returns>returns true if point is inside the quad ( or on the edge) false if it is outside of it.</returns>
        public static bool CheckIfPointIsOnQuad(Vector3 point, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 ba = b - a;
            if (ba.DotProduct(point - a) < 0)
                return false;

            Vector3 cb = c - b;
            if (cb.DotProduct(point - b) < 0)
                return false;

            Vector3 dc = d - c;
            if (dc.DotProduct(point - c) < 0)
                return false;

            Vector3 ad = a - d;
            if (ad.DotProduct(point - d) < 0)
                return false;

            return true;
        }

        public static bool RayPlaneIntersection(Ray ray, Vector3 planeNormal, Vector3 planePoint, out Vector3 pointOfIntersection)
        {
            Vector3 diff = ray.origin - planePoint;
            float prod1 = diff.DotProduct(planeNormal);// Vector3.Dot(diff, planeNormal);
            float prod2 = ray.DotProduct(planeNormal);
            float prod3 = prod1 / prod2;
            pointOfIntersection = ray.origin - ray.direction * prod3;
            return ray.IsVectorInFrontRay(pointOfIntersection);
        }

        public static bool RayBoxIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 a, Vector3 b, Vector3 c)
        {
            // 1.
            Vector3 diff_b_a = a.CalculateDirectionTo(b, false); //b - a;
            Vector3 diff_c_a = a.CalculateDirectionTo(c, false); //c - a;
            Vector3 normal = Vector3.Cross(diff_b_a, diff_c_a);

            // 2.
            Vector3 dR = rayDirection.CalculateDirectionTo(rayOrigin, false); //rayOrigin - rayDirection;

            float normalDotDr = normal.DotProduct(dR.normalized); //Vector3.Dot(normal, dR.normalized);

            if (Mathf.Abs(normalDotDr) < 1e-6f)
            { // Choose your tolerance
                return false;
            }

            float t = -normal.DotProduct(a.CalculateDirectionTo(rayOrigin, false)) / normalDotDr;
            Vector3 M = rayOrigin + dR * t;

            // 3.
            Vector3 diff_M_a = a.CalculateDirectionTo(M, false);
            float u = diff_M_a.DotProduct(diff_b_a);
            float v = diff_M_a.DotProduct(diff_c_a);

            // 4.
            return u >= 0.0f && u <= diff_b_a.DotProduct(diff_b_a) && v >= 0.0f && v <= diff_c_a.DotProduct(diff_c_a);
        }
    }
}