using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
}
