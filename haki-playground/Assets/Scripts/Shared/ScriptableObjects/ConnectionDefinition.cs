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
            if (lookAtPoint == Vector3.zero)
                lookAtPoint = new Vector3(0, 0, 1);

            Gizmo.DrawArrow(worldPosition, worldPosition + lookAtPoint, Color.red);
            //Gizmos.color = color;
            //Gizmos.DrawLine(worldPosition, worldPosition + lookAtPoint);
            //Gizmo.DrawArrow(worldPosition, worldPosition + lookAtPoint, color);
            //Gizmo.DrawCube(worldPosition + lookAtPoint * 1f / 4f, lookAtPoint, new Vector3(.05f, .05f, .1f), color, new Vector3(.5f, .5f, .5f));
            //Gizmo.DrawWireCube(worldPosition + lookAtPoint, lookAtPoint, new Vector3(.2f, .2f, .2f), color, new Vector3(0.5f, 0f, 0));
            //Gizmo.DrawWireCone(worldPosition + lookAtPoint * 2f / 4f, lookAtPoint, .1f, .2f, color, new Vector3(.5f, .5f, .5f));
            //Gizmo.DrawWireCircle(worldPosition + lookAtPoint * 3f / 4f, lookAtPoint, .04f, color, new Vector3(.5f, .5f, .5f));
            //Gizmo.DrawText("Hello", worldPosition + lookAtPoint, color, 12);


            //how this scene is set up attempts to demonstrate how i want cone to behave.
            //there should be a setting that defines wheather cone points towards "Direction" with it's peak or its base (top or bottom).
            
            // the code under does't quite work (sometimes it does ) but sometimes the cones are fliped
            Gizmo.DrawWireCone(lookAtPoint + worldPosition + lookAtPoint * .15f, Quaternion.Euler(180, 0, 0) * (lookAtPoint * -1), .05f, .15f, Color.magenta, 4);
            Gizmo.DrawWireCone(lookAtPoint * 0.9f + worldPosition, lookAtPoint, .05f, .15f, Color.blue, 4);


            // up to this point ^^^^^^

            worldPosition.y += .01f;
            //this should be cube's default behaviour
            Gizmo.DrawWireCube(worldPosition, lookAtPoint, Vector3.one * .05f, Color.red, Vector3.one * .5f);
            // there should be an option to draw the cube from the side (middle or corner, need an option for it) that is facing away from the direction
            //im sure that this effect can be achieved through pivots, but we need something that can be done without much thinking or prior knowledge of the system

            //like this

            Vector3 point = Vector3.one;
            float scaleFactor = .25f; // scaleFactor is only for presentation, makes things smaller
            Gizmo.DrawArrow(point, point + Vector3.up, Color.blue); // we need arrow method using pos, direction and lenght
            Gizmo.DrawArrow(point, point + Vector3.forward, Color.red); // we need arrow method using pos, direction and lenght
            Gizmo.DrawArrow(point, point + Vector3.one, Color.magenta); // we need arrow method using pos, direction and lenght

            //current default behaviour is good
            //Gizmo.DrawWireCube(point , Vector3.up, Vector3.one * scaleFactor, Color.magenta);

            //there should be an option for this kind of behaviour
            //this option should be easly accessible and something like this
            void DrawCube(Vector3 position, Vector3 direction, Vector3 size, Color color)
            {
                Vector3 temp = size * scaleFactor;
                // create temp as size * scaleFactor, again scale factor is only for presentation
                // in those specific examples and should not be part default implementation

                // this methods has the same arguments as other Gizmo.DrawWireCube(..) methods.
                //i think 

                Gizmo.DrawWireCube(position + new Vector3(temp.x * direction.x / 2, temp.y * direction.y / 2, temp.z * direction.z / 2), direction, size * scaleFactor, color);
            }

            DrawCube(point, Vector3.up, Vector3.one, Color.blue);
            DrawCube(point, Vector3.forward, Vector3.one, Color.red);
            DrawCube(point, Vector3.one, Vector3.one, Color.magenta);

            

            //also another point to mention.
            // i think we're going to use more "Wire" renders than "Solid" renders
            //i think it would be better to rename all of DrawX methods to DrawSolidX
            // and the DrawWireX methods to DrawX methods



            //Gizmo.DrawArrow(worldPosition, worldPosition + lookAtPoint * .3f, Color.red);
            //Gizmo.DrawWireCube(new Vector3(2, 3, 0), Quaternion.Euler(250, 224, 10), new Vector3(.05f, .05f, .1f), Color.white, new Vector3(.5f, .5f, .5f));
            //Gizmo.DrawWireCone(new Vector3(3, 2, 0), Quaternion.Euler(270, 0, 0), .1f, .2f, Color.magenta, new Vector3(.5f, .5f, .5f), 7);
            //Gizmo.DrawWireCircle(new Vector3(2, 2, 0), Quaternion.Euler(50, 124, 20), .1f, Color.cyan, new Vector3(.5f, .5f, .5f));
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