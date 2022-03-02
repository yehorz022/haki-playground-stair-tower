using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public static class Gizmo
    {
        static GUIStyle style = new GUIStyle();

        #region Cube
        public static void DrawCube(Vector3 position, Vector3 direction, Vector3 size, Color color)
        {
            DrawCube(position, Quaternion.LookRotation(direction), size, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 size, Color color)
        {
            DrawCube(position, rotation, size, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawCube(Vector3 position, Vector3 direction, Vector3 size, Color color, Vector3 pivot)
        {
            DrawCube(position, Quaternion.LookRotation(direction), size, color, pivot);
        }
        public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position + new Vector3((pivot.x - 0.5f) * size.x, (0.5f - pivot.y) * size.y, (pivot.z - 0.5f) * size.z), rotation, color);
            Gizmos.DrawCube(Vector3.zero, size);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region Sphere
        public static void DrawSphere(Vector3 position, Vector3 direction, float radius, Color color)
        {
            DrawSphere(position, Quaternion.LookRotation(direction), radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawSphere(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawSphere(position, rotation, radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawSphere(Vector3 position, Vector3 direction, float radius, Color color, Vector3 pivot)
        {
            DrawSphere(position, Quaternion.LookRotation(direction), radius, color, pivot);
        }
        public static void DrawSphere(Vector3 position, Quaternion rotation, float radius, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position + new Vector3(pivot.x - 0.5f, 0.5f - pivot.y, pivot.z - 0.5f) * radius, rotation, color);
            Gizmos.DrawSphere(Vector3.zero, radius);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region WireCube
        public static void DrawWireCube(Vector3 position, Vector3 direction, Vector3 size, Color color)
        {
            DrawWireCube(position, Quaternion.LookRotation(direction), size, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 size, Color color)
        {
            DrawWireCube(position, rotation, size, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawWireCube(Vector3 position, Vector3 direction, Vector3 size, Color color, Vector3 pivot)
        {
            DrawWireCube(position, Quaternion.LookRotation(direction), size, color, pivot);
        }
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position + new Vector3((pivot.x - 0.5f) * size.x, (0.5f - pivot.y) * size.y, (pivot.z - 0.5f) * size.z), rotation, color);
            Gizmos.DrawWireCube(Vector3.zero, size);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region WireSphere
        public static void DrawWireSphere(Vector3 position, Vector3 direction, float radius, Color color)
        {
            DrawWireSphere(position, Quaternion.LookRotation(direction), radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawWireSphere(position, rotation, radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawWireSphere(Vector3 position, Vector3 direction, float radius, Color color, Vector3 pivot)
        {
            DrawWireSphere(position, Quaternion.LookRotation(direction), radius, color, pivot);
        }
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position + new Vector3(pivot.x - 0.5f, 0.5f - pivot.y, pivot.z - 0.5f) * radius, rotation, color);
            Gizmos.DrawWireSphere(Vector3.zero, radius);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region WireCircle
        public static void DrawWireCircle(Vector3 position, Vector3 direction, float radius, Color color)
        {
            DrawWireCircle(position, Quaternion.LookRotation(direction), radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawWireCircle(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawWireCircle(position, rotation, radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawWireCircle(Vector3 position, Vector3 direction, float radius, Color color, Vector3 pivot)
        {
            DrawWireCircle(position, Quaternion.LookRotation(direction), radius, color, pivot);
        }
        public static void DrawWireCircle(Vector3 position, Quaternion rotation, float radius, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position + new Vector3(pivot.x - 0.5f, 0.5f - pivot.y, pivot.z - 0.5f) * radius, rotation, color);
            DrawCircle(radius);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region Cone
        public static void DrawWireCone(Vector3 position, Vector3 direction, float radius, float height, Color color, int noOfRings = 10)
        {
            DrawWireCone(position, Quaternion.LookRotation(direction), radius, height, color, new Vector3(0.5f, 0.5f, 0.5f), noOfRings);
        }
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float radius, float height, Color color, int noOfRings = 10)
        {
            DrawWireCone(position, rotation, radius, height, color, new Vector3(0.5f, 0.5f, 0.5f), noOfRings);
        }
        public static void DrawWireCone(Vector3 position, Vector3 direction, float radius, float height, Color color, Vector3 pivot, int noOfRings = 10)
        {
            DrawWireCone(position, Quaternion.LookRotation(direction), radius, height, color, pivot, noOfRings);
        }
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float radius, float height, Color color, Vector3 pivot, int noOfRings = 10)
        {
            Vector3 forwardVector = rotation * Vector3.forward;
            float radiusDiv = 1f * radius / noOfRings;
            float heightDiv = 1f * height / noOfRings;
            for (int i = 1; i < noOfRings; i++)
            {
                Matrix4x4 oldGizmosMatrix = SetEnvironment(position + new Vector3((pivot.x - 0.5f) * radius, (0.5f - pivot.y) * height, (pivot.z - 0.5f) * radius) + heightDiv * i * forwardVector, rotation, color);
                DrawCircle(radius - radiusDiv * i);
                Gizmos.matrix = oldGizmosMatrix;
            }
        }
        #endregion

        #region Arrow
        public static void DrawArrow(Vector3 position1, Vector3 position2, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(position1, position2);
            DrawWireCone(position2 - (position2 - position1).normalized * .05f, Quaternion.LookRotation(position2 - position1), .02f, .05f, color);
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

        static Matrix4x4 SetEnvironment(Vector3 position, Quaternion rotation, Color color)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, Vector3.one);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;
            Gizmos.color = color;
            return oldGizmosMatrix;
        }

        public static void DrawCircle(float radius)
        {
            Vector3 point = Vector3.zero, lastPoint = Vector3.zero;
            float angle = 15;
            float angleDivs = 360 / angle;
            for (int i = 0; i < angleDivs + 1; i++)
            {
                int k = i;
                if (i == angleDivs)
                    k = 0;
                point = radius * new Vector3(Mathf.Cos(k * angle * Mathf.Deg2Rad), Mathf.Sin(k * angle * Mathf.Deg2Rad), 0);
                if (i > 0)
                    Gizmos.DrawLine(lastPoint, point);
                lastPoint = point;
            }
        }
    }
}
