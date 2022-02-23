using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;
using UnityEngine;

namespace Assets.Scripts.Services.ComponentService
{
    public interface IIntersectionService
    {
        bool CheckIntersection(Vector3 lineStart, Vector3 lineEnd, Vector3 heading, Vector3 worldPosition);
    }

    [Service(typeof(IIntersectionService))]
    public class IntersectionService : IIntersectionService
    {
        enum LineSphereIntersectionType
        {
            None,
            Tangent, // only 1 point
            Intersection
        }

        /// <summary>
        /// This method is a a great place to use compute shader
        /// </summary>
        /// <param name="start"> start point of the line</param>
        /// <param name="end">end point of the line</param>
        /// <param name="sphereCenter">center of the sphere</param>
        /// <param name="r">radius of the sphere</param>
        /// <returns></returns>
        static LineSphereIntersectionType GetIntersectionType(Vector3 start, Vector3 end, Vector3 sphereCenter, float r)
        {
            //create variables for ease of use
            float x1 = start.x;
            float y1 = start.y;
            float z1 = start.z;
            float x2 = end.x;
            float y2 = end.y;
            float z2 = end.z;

            float x3 = sphereCenter.x;
            float y3 = sphereCenter.y;
            float z3 = sphereCenter.z;

            //calculate line point relation
            float x2x1diff = x2 - x1;
            float y2y1diff = y2 - y1;
            float z2z1diff = z2 - z1;

            //calculate line start and sphere relation
            float x1x3diff = x1 - x3;
            float y1y3diff = y1 - y3;
            float z1z3diff = z1 - z3;

            // calcualte squares of line point relation
            float x2x1Sqr = x2x1diff * x2x1diff;
            float y2y1Sqr = y2y1diff * y2y1diff;
            float z2z1Sqr = z2z1diff * z2z1diff;

            //calculate a
            float a = x2x1Sqr + y2y1Sqr + z2z1Sqr;

            //calculate squares of line start and sphere relation
            float x2x1x1x3 = x2x1diff * x1x3diff;
            float y2y1y1y3 = y2y1diff * y1y3diff;
            float z2z1z1z3 = z2z1diff * z1z3diff;

            //calcualte b
            float b = 2 * (x2x1x1x3 + y2y1y1y3 + z2z1z1z3);

            //calculate squares
            float x3Sq = x3 * x3;
            float y3Sq = y3 * y3;
            float z3Sq = z3 * z3;
            float x1Sq = x1 * x1;
            float y1Sq = y1 * y1;
            float z1Sq = z1 * z1;

            //calculate c
            float c = x3Sq + y3Sq + z3Sq + x1Sq + y1Sq + z1Sq - 2 * (x3 * x1 + y3 * y1 + z3 * z1) - (r * r);

            //calculate result
            float res = b * b - 4 * a * c;

            //if res is greater than ZERO it means that there are 2 points of intersection
            if (res > 0)
                return LineSphereIntersectionType.Intersection;

            // if res is equal to ZERO it means that the point of intersection is a on the surface of the sphere meaning its a tangent ( unlikely to ever happen)
            if (res == 0)
                return LineSphereIntersectionType.Tangent;

            // in other cases, res is less than ZERO, there is no intersection
            return LineSphereIntersectionType.None;
        }

        public bool CheckIntersection(Vector3 lineStart, Vector3 lineEnd, Vector3 heading, Vector3 worldPosition)
        {
            Vector3 dir = (lineEnd - lineStart).normalized;

            float dot = Vector3.Dot(dir, heading);

            if (dot >= Constants.MinimumDotProductValueAllowedForIntersections) // check if it is facing us or away from us. 
            {
                return false;
            }

            LineSphereIntersectionType intersectionType = GetIntersectionType(lineStart, lineEnd, worldPosition, Constants.VirtualSphereRadius /*Add some sort of setting later on*/);

            if (intersectionType == LineSphereIntersectionType.Intersection)
            {
                return true;
            }

            return false;
        }
    }
}