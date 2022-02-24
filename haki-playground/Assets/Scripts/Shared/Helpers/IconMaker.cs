using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public static class IconMaker
    {
        // Camera Properties
        static Camera iconCamera;
        static GameObject camerObj;
        static RenderTexture renderTexture;

        // Icon Image Properties
        static int iconSize = 300;
        static Rect screenRect;

        //3D Model Properities (save model state to reset it after making icon)
        static int modelOldLayer;
        static Vector3 modelOldPosition;
        static MeshEdges extremeEdges; //to find boundary points of models
        static Quaternion modelOldRotation;

        public static void Initialize()
        {
            iconSize = iconSize < Screen.width ? iconSize : Screen.width; // checking our size with screen size width
            iconSize = iconSize < Screen.height ? iconSize : Screen.height; // checking our size with screen size height
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
            renderTexture = new RenderTexture(iconSize, iconSize, 24);

            camerObj = new GameObject();
            camerObj.name = "MeshIconMaker";
            iconCamera = camerObj.AddComponent<Camera>();
            iconCamera.clearFlags = CameraClearFlags.SolidColor;
            iconCamera.nearClipPlane = 0.01f;
            iconCamera.cullingMask = LayerMask.GetMask("UI");
        }

        public static Texture2D CreateIcon(GameObject model, Color backgroundColor)
        {
            if (iconCamera == null)
                Initialize(); // as this code only needs to run once at start
            BeforeCreation(model);
            MeshEdges edges = GetExtremeEdges(model);
            int smallerAxis = edges.GetSmallerAxis(); //print ("smaller axis : " + smallerAxis + " ,, "+ edges.size);
            model.transform.rotation = Quaternion.Euler(smallerAxis == 1 ? 90 : 0, smallerAxis != 1 ? 90 : 0, 0);
            edges = GetExtremeEdges(model); //Debug.DrawLine(edges.min, edges.max, Color.green, 5000);
            edges.CalculateSize(); // setting its size values 
            camerObj.transform.position = new Vector3(edges.Center().x, edges.Center().y, -edges.GetBiggerAxisSize() * 1.1f); //move camera in middle and front of obj to take snap

            iconCamera.backgroundColor = backgroundColor;
            iconCamera.targetTexture = renderTexture;
            iconCamera.Render();
            RenderTexture.active = renderTexture;

            Texture2D screenShot = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);
            screenShot.ReadPixels(screenRect, 0, 0);
            screenShot.Apply();
            RenderTexture.active = null;

            AfterCreation(model);
            return screenShot;
        }

        static void BeforeCreation(GameObject model)
        {
            iconCamera.enabled = true;
            modelOldLayer = model.layer;
            modelOldPosition = model.transform.position;
            modelOldRotation = model.transform.rotation;
            model.transform.position = Vector3.zero;
            model.transform.rotation = Quaternion.identity;
            SetLayerRecursively(model, LayerMask.NameToLayer("UI")); // set layer uI
        }

        static void AfterCreation(GameObject model)
        {
            iconCamera.enabled = false;
            model.transform.position = modelOldPosition;
            model.transform.rotation = modelOldRotation;
            SetLayerRecursively(model, modelOldLayer); // reset old layer
        }

        static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;

            for (int i = 0; i < obj.transform.childCount; i++)
                SetLayerRecursively(obj.transform.GetChild(i).gameObject, newLayer);
        }

        static MeshEdges GetExtremeEdges(GameObject obj)
        {
            extremeEdges = new MeshEdges();
            return GetExtremeEdgesRecursively(obj);
        }

        static MeshEdges GetExtremeEdgesRecursively(GameObject obj)
        {
            if (obj.transform.GetComponent<MeshRenderer>() != null)
            {
                Bounds bounds = obj.transform.GetComponent<MeshRenderer>().bounds;  //Debug.DrawLine(bounds.min, bounds.max, Color.magenta, 50);
                extremeEdges = new MeshEdges(bounds.min, bounds.max).CompareAndGetExtremeEdges(extremeEdges); //print(obj.name + " ("+bounds.min + "," + bounds.max + ") , " + extremeEdges);
            }
            for (int i = 0; i < obj.transform.childCount; i++)
                GetExtremeEdgesRecursively(obj.transform.GetChild(i).gameObject);
            return extremeEdges;
        }

    }

    public class MeshEdges
    {
        public Vector3 min;
        public Vector3 max;
        public Vector3 size;


        public MeshEdges()
        {
            min = new Vector3(+999, +999, +999);
            max = new Vector3(-999, -999, -999);
        }

        public MeshEdges(Vector3 _min, Vector3 _max)
        {
            min = _min;
            max = _max;
        }

        public Vector3 Center()
        {
            return new Vector3((min.x + max.x) / 2f, (min.y + max.y) / 2f, (min.z + max.z) / 2f);
        }

        public void CalculateSize()
        {
            size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
        }

        public int GetSmallerAxis()
        {
            CalculateSize();
            return size.x < size.y ? (size.x < size.z ? 0 : 2) : (size.y < size.z ? 1 : 2);
        }

        public float GetBiggerAxisSize()
        {
            CalculateSize();
            return size.x > size.y ? (size.x > size.z ? size.x : size.z) : (size.y > size.z ? size.y : size.z);
        }

        public MeshEdges CompareAndGetExtremeEdges(MeshEdges toCompare)
        {
            toCompare.min.x = min.x < toCompare.min.x ? min.x : toCompare.min.x;
            toCompare.min.y = min.y < toCompare.min.y ? min.y : toCompare.min.y;
            toCompare.min.z = min.z < toCompare.min.z ? min.z : toCompare.min.z;
            toCompare.max.x = max.x > toCompare.max.x ? max.x : toCompare.max.x;
            toCompare.max.y = max.y > toCompare.max.y ? max.y : toCompare.max.y;
            toCompare.max.z = max.z > toCompare.max.z ? max.z : toCompare.max.z;
            return toCompare;
        }

        public override string ToString()
        {
            return "min:" + min + ",max:" + max;
        }
    }

}
