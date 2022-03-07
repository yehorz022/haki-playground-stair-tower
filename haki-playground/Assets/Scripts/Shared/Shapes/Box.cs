using System.Collections.Generic;
using Assets.Scripts.Shared.Helpers;
using Assets.Scripts.Shared.UnityExtensions;
using log4net.Appender;
using UnityEngine;

namespace Assets.Scripts.Shared.Shapes
{
    public abstract class Shape<T> where T : Shape<T>
    {
        protected abstract T Rotate(Transform transform);
    }

    public class Box : Shape<Box>
    {
        private const int NumberOfSides = 6;
        private const int NumberOfVerticesPerSide = 4;
        private Vector3 localPosition;
        private Vector3[] points;
        private Vector3[] normals;
        private Vector3 offset;
        private Vector3 min;
        private Vector3 max;

        Color color = Color.blue;

        public Vector3[] Normals => normals;

        private Box(Vector3 localPosition, Vector3 offset, (Vector3[] points, Vector3[] normals, Vector3 min, Vector3 max) structure)
        {
            this.offset = offset;
            this.localPosition = localPosition;
            points = structure.points;
            normals = structure.normals;
            min = structure.min;
            max = structure.max;
        }
        public Box(Vector3 size, Vector3 offset, Transform transform) : this(size, offset, CreatePoints(size))
        {
            //Rotate(transform);
        }


        private static Vector3[] CreateNormals()
        {
            return new Vector3[]
            {
                Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.right, Vector3.left,
            };
        }

        private static (Vector3[] points, Vector3[] normals, Vector3 min, Vector3 max) CreatePoints(Vector3 size)
        {
            Vector3[] points = new Vector3[NumberOfSides * NumberOfVerticesPerSide];
            Vector3[] normals = CreateNormals();
            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;
            int i = 0;

            float x = size.x;
            float y = size.y;
            float z = size.z;

            void MakePoint(float posX, float poxY, float poxZ)
            {
                Vector3 point = new Vector3(posX, poxY, poxZ);
                points[i++] = point;
                min = Vector3.Min(min, point);
                max = Vector3.Max(max, point);
            }

            //top
            MakePoint(-x, +y, +z);
            MakePoint(+x, +y, +z);
            MakePoint(+x, +y, -z);
            MakePoint(-x, +y, -z);
            //bottom
            MakePoint(-x, -y, +z);
            MakePoint(+x, -y, +z);
            MakePoint(+x, -y, -z);
            MakePoint(-x, -y, -z);
            //forward
            MakePoint(-x, +y, +z);
            MakePoint(+x, +y, +z);
            MakePoint(+x, -y, +z);
            MakePoint(-x, -y, +z);
            //back
            MakePoint(-x, +y, -z);
            MakePoint(+x, +y, -z);
            MakePoint(+x, -y, -z);
            MakePoint(-x, -y, -z);
            //right
            MakePoint(+x, +y, +z);
            MakePoint(+x, -y, +z);
            MakePoint(+x, -y, -z);
            MakePoint(+x, +y, -z);
            //left
            MakePoint(-x, -y, +z);
            MakePoint(-x, +y, +z);
            MakePoint(-x, +y, -z);
            MakePoint(-x, -y, -z);

            return (points, normals, min, max);
        }


        protected sealed override Box Rotate(Transform transform)
        {

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.rotation * points[i];
            }

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = transform.rotation * normals[i];
            }

            localPosition = transform.rotation * localPosition;
            offset = transform.rotation * offset;

            return this;
        }

        public void DebugDraw(Transform transform)
        {
            Vector3 world = GetWorldPos(transform);

            void d(int i1, int i2)
            {
                if (i2 >= points.Length || i1 >= points.Length)
                    return;
                Debug.DrawLine(world + transform.rotation * points[i1], world + transform.rotation * points[i2], color);
            }
            for (int iSide = 0; iSide < NumberOfSides; iSide++)
            {
                int main = iSide * NumberOfVerticesPerSide;
                d(main + 0, main + 1);
                d(main + 1, main + 2);
                d(main + 2, main + 3);
                d(main + 3, main + 0);
            }
        }


        public bool IntersectRay(Ray ray, Transform transform, out Vector3 pointOfContact)
        {
            Vector3 world = GetWorldPos(transform);
            color = Color.blue;

            for (int sideIndex = 0; sideIndex < NumberOfSides; sideIndex++)
            {
                Vector3 normal = transform.rotation * normals[sideIndex];
                Vector3 a = world + transform.rotation * points[sideIndex * NumberOfVerticesPerSide];

                if (Intersections.RayPlaneIntersection(ray, normal, a, out pointOfContact))
                {
                    Vector3 b = world + transform.rotation * points[sideIndex * NumberOfVerticesPerSide + 1];
                    Vector3 c = world + transform.rotation * points[sideIndex * NumberOfVerticesPerSide + 2];
                    Vector3 d = world + transform.rotation * points[sideIndex * NumberOfVerticesPerSide + 3];

                    if (Intersections.CheckIfPointIsOnQuad(pointOfContact, a, b, c, d))
                    {
                        color = Color.red;
                        return true;
                    }
                }

            }
            pointOfContact = Vector3.zero;
            return false;
        }

        private Vector3 GetWorldPos(Transform transform)
        {
            return transform.position + transform.rotation * (localPosition + offset);
        }
    }
}