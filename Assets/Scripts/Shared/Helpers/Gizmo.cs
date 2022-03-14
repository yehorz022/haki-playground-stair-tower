using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public static class Gizmo
    {

        #region Circle
        public static void DrawCircle(Vector3 position, Vector3 direction, float radius, Color color)
        {
            DrawCircle(position, Quaternion.LookRotation(direction), radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawCircle(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawCircle(position, rotation, radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawCircle(Vector3 position, Vector3 direction, float radius, Color color, Vector3 pivot)
        {
            DrawCircle(position, Quaternion.LookRotation(direction), radius, color, pivot);
        }
        public static void DrawCircle(Vector3 position, Quaternion rotation, float radius, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position, rotation, color);
            DrawCircle(new Vector3(pivot.x - 0.5f, 0.5f - pivot.y, pivot.z - 0.5f) * radius, radius);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

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
        public static void DrawCube(Vector3 startPosition, Vector3 size, Color color)
        {
            DrawCube(startPosition, Vector3.forward, size, color, new Vector3(0, 0, 0));
        }
        public static void DrawCubeFromBottomCenter(Vector3 startPosition, Vector3 size, Color color)
        {
            DrawCube(startPosition, Vector3.forward, size, color, new Vector3(0.5f, 0, 0));
        }
        public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position, rotation, color);
            Gizmos.DrawWireCube(new Vector3((pivot.x - 0.5f) * size.x, (0.5f - pivot.y) * size.y, (pivot.z - 0.5f) * size.z), size);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region SolidCube
        public static void DrawSolidCube(Vector3 position, Vector3 direction, Vector3 size, Color color)
        {
            DrawSolidCube(position, Quaternion.LookRotation(direction), size, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawSolidCube(Vector3 position, Quaternion rotation, Vector3 size, Color color)
        {
            DrawSolidCube(position, rotation, size, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawSolidCube(Vector3 position, Vector3 direction, Vector3 size, Color color, Vector3 pivot)
        {
            DrawSolidCube(position, Quaternion.LookRotation(direction), size, color, pivot);
        }
        public static void DrawSolidCube(Vector3 startPosition, Vector3 size, Color color)
        {
            DrawSolidCube(startPosition, Vector3.forward, size, color, new Vector3(0, 0, 0));
        }
        public static void DrawSolidCubeFromBottomCenter(Vector3 startPosition, Vector3 size, Color color)
        {
            DrawSolidCube(startPosition, Vector3.forward, size, color, new Vector3(0.5f, 0, 0));
        }
        public static void DrawSolidCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position, rotation, color);
            Gizmos.DrawCube(new Vector3((pivot.x - 0.5f) * size.x, (0.5f - pivot.y) * size.y, (pivot.z - 0.5f) * size.z), size);
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
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position, rotation, color);
            Gizmos.DrawWireSphere(new Vector3(pivot.x - 0.5f, 0.5f - pivot.y, pivot.z - 0.5f) * radius, radius);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region SolidSphere
        public static void DrawSolidSphere(Vector3 position, Vector3 direction, float radius, Color color)
        {
            DrawSolidSphere(position, Quaternion.LookRotation(direction), radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawSolidSphere(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawSolidSphere(position, rotation, radius, color, new Vector3(0.5f, 0.5f, 0.5f));
        }
        public static void DrawSolidSphere(Vector3 position, Vector3 direction, float radius, Color color, Vector3 pivot)
        {
            DrawSolidSphere(position, Quaternion.LookRotation(direction), radius, color, pivot);
        }
        public static void DrawSolidSphere(Vector3 position, Quaternion rotation, float radius, Color color, Vector3 pivot)
        {
            Matrix4x4 oldGizmosMatrix = SetEnvironment(position, rotation, color);
            Gizmos.DrawSphere(new Vector3(pivot.x - 0.5f, 0.5f - pivot.y, pivot.z - 0.5f) * radius, radius);
            Gizmos.matrix = oldGizmosMatrix;
        }
        #endregion

        #region Cone
        public static void DrawCone(Vector3 position, Vector3 direction, float radius, float height, Color color, int noOfRings = 10, bool isRevesed = false)
        {
            DrawCone(position, Quaternion.LookRotation(direction), radius, height, color, new Vector3(0.5f, 0.5f, 0.5f), noOfRings, isRevesed);
        }
        public static void DrawCone(Vector3 position, Quaternion rotation, float radius, float height, Color color, int noOfRings = 10, bool isRevesed = false)
        {
            DrawCone(position, rotation, radius, height, color, new Vector3(0.5f, 0.5f, 0.5f), noOfRings, isRevesed);
        }
        public static void DrawCone(Vector3 position, Vector3 direction, float radius, float height, Color color, Vector3 pivot, int noOfRings = 10, bool isRevesed = false)
        {
            DrawCone(position, Quaternion.LookRotation(direction), radius, height, color, pivot, noOfRings, isRevesed);
        }
        public static void DrawCone(Vector3 position, Quaternion rotation, float radius, float height, Color color, Vector3 pivot, int noOfRings = 10, bool isRevesed = false)
        {
            Vector3 forwardVector = rotation * Vector3.forward;
            float radiusDiv = 1f * radius / noOfRings;
            float heightDiv = 1f * height / noOfRings;
            for (int i = 0; i < noOfRings; i++)
            {
                Matrix4x4 oldGizmosMatrix = SetEnvironment(position + heightDiv * i * forwardVector - height / 2f * forwardVector, rotation, color); // - height / 2f * forwardVector to start it from center
                DrawCircle(new Vector3((pivot.x - 0.5f) * radius, (0.5f - pivot.y) * radius, (pivot.z - 0.5f) * height), isRevesed ? radiusDiv * i : radius - radiusDiv * i);
                Gizmos.matrix = oldGizmosMatrix;
            }
        }
        #endregion

        #region Arrow
        public static void DrawArrow(Vector3 position, Vector3 direction, float length, Color color)
        {
            DrawArrow(position, position + direction.normalized * length, color);
        }
        public static void DrawArrow(Vector3 position1, Vector3 position2, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(position1, position2);
            DrawCone(position2 - (position2 - position1).normalized * .05f, Quaternion.LookRotation(position2 - position1), .05f, .1f, color, new Vector3(0.5f, 0.5f, 1), 4);
        }
        #endregion

        #region Text
        static GUIStyle style = new GUIStyle();

        public static void DrawText(string label, Vector3 position, Color color, int fontsize = 10)
        {
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = color;
            style.fontSize = fontsize;
            Handles.Label(position, label, style);
        }
        #endregion

        #region GeneralFuntions
        static Matrix4x4 SetEnvironment(Vector3 position, Quaternion rotation, Color color)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, Vector3.one);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;
            Gizmos.color = color;
            return oldGizmosMatrix;
        } 
        public static void DrawCircle(Vector3 position, float radius)
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
                    Gizmos.DrawLine(position + lastPoint, position + point);
                lastPoint = point;
            }
        }
        #endregion
    }
}
