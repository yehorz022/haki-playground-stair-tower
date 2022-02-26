using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public enum GizmoAllignment
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

        static readonly Vector3[] allignments = new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1)
            };

        static GUIStyle style = new GUIStyle();

        public static void DrawCube(Vector3 position, float size, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Cube, "", position, size, Color.white, Color.white, allignment);
        }

        public static void DrawCube(Vector3 position, float size, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Cube, "", position, size, color, Color.white, allignment);
        }

        public static void DrawCube(string label, Vector3 position, float size, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Cube, label, position, size, color, color, allignment);
        }

        public static void DrawCube(string label, Vector3 position, float size, Color color, Color fontColor, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Cube, label, position, size, color, fontColor, allignment);
        }

        public static void DrawSphere(Vector3 position, float size, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Sphere, "", position, size, Color.white, Color.white, allignment);
        }

        public static void DrawSphere(Vector3 position, float radius, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Sphere, "", position, radius, color, Color.white, allignment);
        }

        public static void DrawSphere(string label, Vector3 position, float size, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Sphere, label, position, size, color, color, allignment);
        }

        public static void DrawSphere(string label, Vector3 position, float size, Color color, Color fontColor, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.Sphere, label, position, size, color, fontColor, allignment);
        }

        public static void DrawWireCube(Vector3 position, float size, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireCube, "", position, size, Color.white, Color.white, allignment);
        }

        public static void DrawWireCube(Vector3 position, float size, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireCube, "", position, size, color, Color.white, allignment);
        }

        public static void DrawWireCube(string label, Vector3 position, float size, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireCube, label, position, size, color, color, allignment);
        }

        public static void DrawWireCube(string label, Vector3 position, float size, Color color, Color fontColor, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireCube, label, position, size, color, fontColor, allignment);
        }

        public static void DrawWireSphere(Vector3 position, float radius, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireSphere, "", position, radius, Color.white, Color.white, allignment);
        }

        public static void DrawWireSphere(string label, Vector3 position, float radius, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireSphere, label, position, radius, color, color, allignment);
        }

        public static void DrawWireSphere(string label, Vector3 position, float radius, Color color, Color fontColor, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireSphere, label, position, radius, color, fontColor, allignment);
        }

        public static void DrawWireSphere(Vector3 position, float size, Color color, GizmoAllignment allignment = GizmoAllignment.center)
        {
            Draw(Shape.WireSphere, "", position, size, color, Color.white, allignment);
        }

        static void Draw(Shape shape, string label, Vector3 position, float size, Color color, Color fontColor, GizmoAllignment allignment)
        {
            Gizmos.color = color;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = fontColor;
            Handles.Label(position + allignments[(int)allignment], label, style);

            switch (shape)
            {
                case Shape.Cube:
                    Gizmos.DrawCube(position + allignments[(int)allignment] * size / 2f, Vector3.one * size);
                    break;
                case Shape.Sphere:
                    Gizmos.DrawSphere(position + allignments[(int)allignment] * size, size);
                    break;
                case Shape.WireCube:
                    Gizmos.DrawWireCube(position + allignments[(int)allignment] * size / 2f, Vector3.one * size);
                    break;
                case Shape.WireSphere:
                    Gizmos.DrawWireSphere(position + allignments[(int)allignment] * size, size);
                    break;
            }
        }
    }
}
