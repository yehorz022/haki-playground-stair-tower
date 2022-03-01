using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public enum Direction
    {
        center,
        right,
        left,
        up,
        down,
        forward,
        backward
    }

    public static class Gizmo
    {
        enum Shape
        {
            Cube,
            Sphere,
            WireCube,
            WireSphere
        };

        static readonly Vector3[] directions = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };

        static GUIStyle style = new GUIStyle();

        #region Cube
        public static void DrawCube(Vector3 position, Vector3 size, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.Cube, position, size, color, allignment);
        }
        #endregion

        #region Sphere
        public static void DrawSphere(Vector3 position, float radius, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.Sphere, position, new Vector3(radius, 0, 0), color, allignment);
        }
        #endregion

        #region WireCube
        public static void DrawWireCube(Vector3 position, Vector3 size, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.WireCube, position, size, color, allignment);
        }
        #endregion

        #region WireSphere
        public static void DrawWireSphere(Vector3 position, float radius, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.WireSphere, position, new Vector3(radius, 0, 0), color, allignment);
        }
        #endregion

        #region WireCircle

        public static void DrawWireCircle(Vector3 position, float radius, Color color, Vector3 direction, Direction allignment = Direction.center)
        {
            DrawCircle(position + directions[(int)allignment] * radius, radius, color, direction, allignment);
        }
        #endregion

        #region Cone
        public static void DrawWireCone(Vector3 position, float radius, Color color, Vector3 direction, Direction allignment = Direction.center)
        {

            Matrix4x4 cubeTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;
            float radiusDiv = radius / 10f;
            for (int i = 1; i < 10f; i++)
                DrawCircle(position + directions[(int)allignment] * radius + .01f * i * direction, radius - radiusDiv * i, color, direction, allignment);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region Text
        public static void DrawText(string label, Vector3 position, Color color, int fontsize = 10)
        {
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = color;
            style.fontSize = fontsize;
            Handles.Label(position, label, style);
        }
        #endregion

        static void Draw(Shape shape, Vector3 position, Vector3 size, Color color, Direction allignment)
        {
            Gizmos.color = color;
            switch (shape)
            {
                case Shape.Cube:
                    Gizmos.DrawCube(position + new Vector3(directions[(int)allignment].x * size.x / 2f, directions[(int)allignment].y * size.y / 2f, directions[(int)allignment].z * size.z / 2f), size);
                    break;
                case Shape.Sphere:
                    Gizmos.DrawSphere(position + directions[(int)allignment] * size.x, size.x);
                    break;
                case Shape.WireCube:
                    Gizmos.DrawWireCube(position + new Vector3(directions[(int)allignment].x * size.x / 2f, directions[(int)allignment].y * size.y / 2f, directions[(int)allignment].z * size.z / 2f), size);
                    break;
                case Shape.WireSphere:
                    Gizmos.DrawWireSphere(position + directions[(int)allignment] * size.x, size.x);
                    break;
            }
        }

        static void DrawCircle2(Vector3 position, float radius, Color color, Direction axis, Direction allignment = Direction.center)
        {
            Gizmos.color = color;
            Vector3 point = Vector3.zero, lastPoint = Vector3.zero;
            for (int i = 0; i < 24; i++)
            {
                if (axis == Direction.up || axis == Direction.down)
                {
                    point = new Vector3(radius * Mathf.Cos(i * 15 * Mathf.Deg2Rad), 0, radius * Mathf.Sin(i * 15 * Mathf.Deg2Rad));
                }
                else if (axis == Direction.right || axis == Direction.left)
                {
                    point = new Vector3(0, radius * Mathf.Cos(i * 15 * Mathf.Deg2Rad), radius * Mathf.Sin(i * 15 * Mathf.Deg2Rad));
                }
                else
                {
                    point = new Vector3(radius * Mathf.Cos(i * 15 * Mathf.Deg2Rad), radius * Mathf.Sin(i * 15 * Mathf.Deg2Rad), 0);
                }
                if (i > 0)
                    Gizmos.DrawLine(position + lastPoint, position + point);
                lastPoint = point;
            }
        }
        public static void DrawCircle(Vector3 position, float radius, Color color, Vector3 direction, Direction allignment = Direction.center)
        {
            Gizmos.color = color;
            Vector3 point = Vector3.zero, lastPoint = Vector3.zero;
            for (int i = 0; i < 24; i++)
            {
                point = radius * new Vector3(Mathf.Cos(i * 15 * Mathf.Deg2Rad), Mathf.Sin(i * 15 * Mathf.Deg2Rad), 0);
                if (i > 0)
                    Gizmos.DrawLine(position + lastPoint, position + point);
                lastPoint = point;
            }
        }
    }
}

//float val = 90 * Mathf.Deg2Rad;
//float angle = 15 * Mathf.Deg2Rad;
//Vector3 point = Vector3.zero, lastPoint = Vector3.zero;
//point.y = point.y* Mathf.Cos(val* (1 - dir.x)) + point.z* Mathf.Sin(val* (1 - dir.x));
//                point.z = point.z* Mathf.Cos(val* (1 - dir.z)) - point_y* Mathf.Sin(val* (1 - dir.z));


//float cos = Mathf.Cos(i * angle);
//float sin = Mathf.Sin(i * angle);
//point.x = radius* (cos* Mathf.Cos(val* direction.x));
//                point.y = radius* (cos* Mathf.Sin(val* direction.x) + sin* Mathf.Sin(val* direction.z));
//                point.z = radius* (sin* Mathf.Cos(val* direction.z));

//float point_t = point.y;
//point.y = point.y * Mathf.Cos(val * direction.y) + point.z * Mathf.Sin(val * direction.y);
//point.z = point.z * Mathf.Cos(val * direction.y) - point_t * Mathf.Sin(val * direction.y);
//point_t = point.x;
//point.x = point.x * Mathf.Cos(val * direction.x) + point.z * Mathf.Sin(val * direction.x);
//point.z = point.z * Mathf.Cos(val * direction.x) - point_t * Mathf.Sin(val * direction.x);
//point_t = point.x;
//point.x = point.x * Mathf.Cos(val * direction.z) + point.y * Mathf.Sin(val * direction.z);
//point.y = point.y * Mathf.Cos(val * direction.z) - point_t * Mathf.Sin(val * direction.z);