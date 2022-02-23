using System.Collections;
using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.Shared.Helpers;
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

           StartCoroutine(PopulateGrid());
        }

        private IEnumerator  PopulateGrid()
        {
            BeforePopulate();

            for (int i = 0; i < elements.Length; i++)
            {
                yield return CreateInstance(i);
               
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

        private IEnumerator CreateInstance(int i)
        {
            var obj = ocm.Instantiate(elements[i]);


            Texture2D screenShot = MeshIconMaker.CreateIcon(obj.gameObject, Color.blue, componentCamera);

            yield return new WaitForSeconds(5);

            ocm.Cache(obj);

            PanelComponent pc1 = Instantiate(panel, transform);

            pc1.SetImage(screenShot, elements[i]);
            pc1.name = elements[i].name;
        }

        //private Texture2D CreateTexture(int i)
        //{
        //    const int size = 600;
        //    //Rect rect = new Rect(0, 0, size, size);
        //    //RenderTexture renderTexture = new RenderTexture(size, size, 24);
        //    //Texture2D screenShot = new Texture2D(size, size, TextureFormat.RGBA32, false);

        //    //ScaffoldingComponent go = elements[i];


        //    //ScaffoldingComponent temp = ocm.Instantiate(go, Quaternion.identity);
        //    //if (temp.isActiveAndEnabled)
        //    //{

        //    //}
        //    //componentCamera.targetTexture = renderTexture;
        //    //componentCamera.Render();

        //    //RenderTexture.active = renderTexture;

        //    //screenShot.ReadPixels(rect, 0, 0);
        //    //screenShot.Apply();

        //    return 

        //    //temp.gameObject.SetActive(false);
        //    //ocm.Cache(temp);

        //    //return screenShot;
        //}
    }
}