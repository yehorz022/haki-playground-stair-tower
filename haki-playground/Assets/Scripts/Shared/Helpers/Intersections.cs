using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Shared.UnityExtensions;
using UnityEngine;

public static class Intersections
{
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
        if (ray.IsDotProductGreaterOrEqual(planeNormal, 0))
        {
            pointOfIntersection = Vector2.zero;
            return false;
        }

        Vector3 diff = ray.origin - planePoint;
        float prod1 = Vector3.Dot(diff, planeNormal);
        float prod2 = Vector3.Dot(ray.direction, planeNormal);
        float prod3 = prod1 / prod2;
        pointOfIntersection = ray.origin - ray.direction * prod3;
        return ray.IsVectorInFront(pointOfIntersection);
    }

    public static bool RaySquareIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 a, Vector3 b, Vector3 c)
    {
        // 1.
        Vector3 diff_b_a = b - a;
        Vector3 diff_c_a = c - a;
        Vector3 normal = Vector3.Cross(diff_b_a, diff_c_a);

        // 2.
        Vector3 dR = rayOrigin - rayDirection;

        float normalDotDr = Vector3.Dot(normal, dR.normalized);

        if (Mathf.Abs(normalDotDr) < 1e-6f)
        { // Choose your tolerance
            return false;
        }

        float t = -normal.DotProduct(rayOrigin - a) / normalDotDr;
        Vector3 M = rayOrigin + dR * t;

        // 3.
        Vector3 diff_M_a = M - a;
        float u = diff_M_a.DotProduct(diff_b_a);
        float v = diff_M_a.DotProduct(diff_c_a);

        // 4.
        return u >= 0.0f && u <= diff_b_a.DotProduct(diff_b_a) && v >= 0.0f && v <= diff_c_a.DotProduct(diff_c_a);
    }
}
