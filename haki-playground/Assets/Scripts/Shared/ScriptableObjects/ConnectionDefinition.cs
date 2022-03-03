using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ConnectionDefinition), menuName = "ScaffoldingComponent/Connection/Connection Definition", order = 0)]
    public class ConnectionDefinition : ScriptableObject
    {


        //think about make different connection type classes, but on the other hand some extra settings can be applied too.
        [SerializeField] public Vector3 localPosition;
        [SerializeField] public Vector3 lookAt;

        [SerializeField] public ComponentConnectionInfo ComponentConnectionInfo;





        public void DebugDraw(Transform parent)
        {
            DebugDraw(parent, Color.red);
        }

        public Vector3 CalculateWorldPosition(Transform parent)
        {
            return parent.rotation * localPosition + parent.position;
        }

        public Vector3 CalculateHeading(Quaternion rotation)
        {
            return rotation * lookAt;
        }

        public void DebugDraw(Transform parent, Color color)
        {
            Vector3 worldPosition = CalculateWorldPosition(parent);

            Vector3 lookAtPoint = CalculateHeading(parent.localRotation);

            //Gizmos.color = color;
            //Gizmos.DrawLine(worldPosition, worldPosition + lookAtPoint);
            //Gizmo.DrawCube(worldPosition + lookAtPoint, lookAtPoint, new Vector3(.5f, .05f, .2f), color);
            //Gizmo.DrawCube(worldPosition + lookAtPoint, new Vector3(.3f, .05f, .1f), color);
            //Gizmo.DrawCone(lookAtPoint + worldPosition, lookAtPoint, .05f, .15f, Color.magenta, 4);
            Gizmo.DrawArrow(worldPosition, worldPosition + lookAtPoint, color);
            Gizmo.DrawCone(lookAtPoint + worldPosition, lookAtPoint, .05f, .15f, Color.magenta, 14, true);

            Vector3 point = Vector3.one;
            float scaleFactor = .1f;  // scaleFactor for presentation, makes things smaller
            Gizmo.DrawCube(point + Vector3.up * scaleFactor / 4, Vector3.up, Vector3.one * scaleFactor, Color.yellow);
            Gizmo.DrawCube(point + Vector3.forward * scaleFactor / 4, Vector3.forward, Vector3.one * scaleFactor, Color.green);
            Gizmo.DrawCube(point + Vector3.one * scaleFactor / 4, Vector3.one, Vector3.one * scaleFactor, Color.magenta);
            Gizmo.DrawArrow(point, Vector3.up, scaleFactor * 4, Color.yellow); // arrow method taking position, direction and length
            Gizmo.DrawArrow(point, Vector3.forward, scaleFactor * 4, Color.green); // arrow method taking position, direction and length
            Gizmo.DrawArrow(point, Vector3.one, scaleFactor * 4, Color.magenta); // arrow method taking position, direction and length
        }

        public Quaternion CalculateRotation()
        {
            switch (ComponentConnectionInfo.rotationOrientation)
            {
                case ComponentConnectionInfo.RotationOrientation.Horizontal:
                    return Quaternion.FromToRotation(Vector3.back, lookAt);
                case ComponentConnectionInfo.RotationOrientation.Vertical:
                    return Quaternion.FromToRotation(Vector3.up, lookAt);
                default:
                    return Quaternion.identity;
            }
        }
    }
}