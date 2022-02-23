using Assets.Scripts.RunMode.ComponentConnection;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PopulateGridLayout : MonoBehaviour
    {

        public PanelComponent panel;
        public GameObject floor;

        public Camera componentCamera;
        public ScaffoldingComponent[] elements;

        private ObjectCacheManager ocm;

        void Start()
        {
            ocm = FindObjectOfType<ObjectCacheManager>();

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            BeforePopulate();

            for (int i = 0; i < elements.Length; i++)
            {
                CreateInstance(i);
            }

            AfterPopulate();
        }

        private void BeforePopulate()
        {
            componentCamera.gameObject.SetActive(true);
            floor.SetActive(false);
        }
        private void AfterPopulate()
        {
            componentCamera.gameObject.SetActive(false);
            floor.SetActive(true);
            RenderTexture.active = null;
        }

        private void CreateInstance(int i)
        {
            Texture2D screenShot = CreateTexture(i);

            PanelComponent pc1 = Instantiate(panel, transform);

            pc1.SetImage(screenShot, elements[i]);
            pc1.name = elements[i].name;
        }

        private Texture2D CreateTexture(int i)
        {
            const int size = 600;
            Rect rect = new Rect(0, 0, size, size);
            RenderTexture renderTexture = new RenderTexture(size, size, 24);
            Texture2D screenShot = new Texture2D(size, size, TextureFormat.RGBA32, false);

            ScaffoldingComponent go = elements[i];


            ScaffoldingComponent temp = ocm.Instantiate(go, Quaternion.identity);
            if (temp.isActiveAndEnabled)
            {

            }
            componentCamera.targetTexture = renderTexture;
            componentCamera.Render();

            RenderTexture.active = renderTexture;

            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            temp.gameObject.SetActive(false);
            ocm.Cache(temp);

            return screenShot;
        }
    }
}