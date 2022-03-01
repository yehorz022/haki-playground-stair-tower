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
            WireCircle,
            WireSphere,
            WireCone,
            Arrow
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
        public static void DrawCube(Vector3 position, Vector3 direction, Vector3 size, Color color, Direction allignment = Direction.center)
        {
            DrawCube(position, Quaternion.LookRotation(direction), size, color, allignment);
        }
        public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.Cube, position + new Vector3(directions[(int)allignment].x * size.x / 2f, directions[(int)allignment].y * size.y / 2f, directions[(int)allignment].z * size.z / 2f), rotation, size, color, allignment);
        }
        #endregion

        #region Sphere
        public static void DrawSphere(Vector3 position, Vector3 direction, float radius, Color color, Direction allignment = Direction.center)
        {
            DrawSphere(position, Quaternion.LookRotation(direction), radius, color, allignment);
        }
        public static void DrawSphere(Vector3 position, Quaternion rotation, float radius, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.Sphere, position + directions[(int)allignment] * radius, rotation, new Vector3(radius * 2f, 0, 0), color, allignment);
        }
        #endregion

        #region WireCube
        public static void DrawWireCube(Vector3 position, Vector3 direction, Vector3 size, Color color, Direction allignment = Direction.center)
        {
            DrawWireCube(position, Quaternion.LookRotation(direction), size, color, allignment);
        }
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.WireCube, position + new Vector3(directions[(int)allignment].x * size.x / 2f, directions[(int)allignment].y * size.y / 2f, directions[(int)allignment].z * size.z / 2f), rotation, size, color, allignment);
        }
        #endregion

        #region WireSphere
        public static void DrawWireSphere(Vector3 position, Vector3 direction, float radius, Color color, Direction allignment = Direction.center)
        {
            DrawWireSphere(position, Quaternion.LookRotation(direction), radius, color, allignment);
        }
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.WireSphere, position + directions[(int)allignment] * radius, rotation, new Vector3(radius * 2f, 0, 0), color, allignment);
        }
        #endregion

        #region WireCircle
        public static void DrawWireCircle(Vector3 position, Vector3 direction, float radius, Color color, Direction allignment = Direction.center)
        {
            DrawWireCircle(position, Quaternion.LookRotation(direction), radius, color, allignment);
        }

        public static void DrawWireCircle(Vector3 position, Quaternion rotation, float radius, Color color, Direction allignment = Direction.center)
        {
            Draw(Shape.WireCircle, position + directions[(int)allignment] * radius, rotation, new Vector3(radius * 2f, 0, 0), color, allignment);
        }
        #endregion

        #region Cone
        public static void DrawWireCone(Vector3 position, Vector3 direction, float radius, Color color, Direction allignment = Direction.center)
        {
            DrawWireCone(position, Quaternion.LookRotation(direction), radius, color, allignment);
        }

        public static void DrawWireCone(Vector3 position, Quaternion rotation, float radius, Color color, Direction allignment = Direction.center)
        {
            Vector3 forwardVector = rotation * Vector3.forward;
            float radiusDiv = radius / 10f;
            for (int i = 1; i < 10f; i++)
                Draw(Shape.WireCircle, position + directions[(int)allignment] * radius + radiusDiv * 2 * i * forwardVector, rotation, new Vector3(radius - radiusDiv * i, 0, 0) * 2f, color, allignment);
        }
        #endregion

        #region Arrow
        public static void DrawArrow(Vector3 position1, Vector3 position2, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(position1, position2);
            DrawWireCone(position2, Quaternion.LookRotation(position2 - position1), .03f, color, Direction.center);
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

        static void Draw(Shape shape, Vector3 position, Quaternion rotation, Vector3 size, Color color, Direction allignment)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, Vector3.one);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;

            Gizmos.color = color;
            switch (shape)
            {
                case Shape.Cube:
                    Gizmos.DrawCube(Vector3.zero, size);
                    break;
                case Shape.Sphere:
                    Gizmos.DrawSphere(Vector3.zero, size.x / 2f);
                    break;
                case Shape.WireCube:
                    Gizmos.DrawWireCube(Vector3.zero, size);
                    break;
                case Shape.WireCircle:
                    DrawCircle(size.x / 2f);
                    break;
                case Shape.WireSphere:
                    Gizmos.DrawWireSphere(Vector3.zero, size.x / 2f);
                    break;
            }
            Gizmos.matrix = oldGizmosMatrix;
        }

        public static void DrawCircle(float radius)
        {
            Vector3 point = Vector3.zero, lastPoint = Vector3.zero;
            for (int i = 0; i < 25; i++)
            {
                int k = i;
                if (i == 24)
                    k = 0;
                point = radius * new Vector3(Mathf.Cos(k * 15 * Mathf.Deg2Rad), Mathf.Sin(k * 15 * Mathf.Deg2Rad), 0);
                if (i > 0)
                    Gizmos.DrawLine(lastPoint, point);
                lastPoint = point;
            }
        }
    }
}