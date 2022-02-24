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

            PopulateGrid();
        }

        private void  PopulateGrid()
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
            ScaffoldingComponent obj = ocm.Instantiate(elements[i]);

            Texture2D screenShot = MeshIconMaker.CreateIcon(obj.gameObject, Color.blue, componentCamera);

            ocm.Cache(obj);

            PanelComponent pc1 = Instantiate(panel, transform);

            pc1.SetImage(screenShot, elements[i]);
            pc1.name = elements[i].name;
        }

    }
}