using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.Shared.Helpers;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class InputHandler : MonoBehaviour
    {
        public Material defaultMat;
        public Material selectedMat;
        public Material warningMat;
        [SerializeField] Image recycleBin;
        [SerializeField] Sprite recycleBinDefault;
        [SerializeField] Sprite recycleBinOpened;

        public ScaffoldingComponent picked;
        public PositionProvider.PositionProvider positionProvider;
        bool isDown;
        int touchCount;
        public bool onRecycleBin;

        public static InputHandler instance;
        void Awake()
        {
            instance = this;
            Routine.Initialize(this);
            Inputs.Initialize();
        }

        void Start()
        {
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();
        }

        void OnGUI()
        {
            GUILayout.Label("onRecycleBin = " + onRecycleBin);
            GUILayout.Label("picked = " + (picked ? picked.name : "null"));
            GUILayout.Label("selected = " + (ScaffoldingComponent.selected ? ScaffoldingComponent.selected.name : "null"));
        }

        void Update()
        {
            
            touchCount = Input.touchCount;
    #if UNITY_EDITOR
            if (isDown)
                touchCount = 1;
            else
                touchCount = 0;
    #endif
            //Debug.Log ("picked = " + (picked ? picked.name : "null"));
        }

        public void OnDown()
        {
            isDown = true;
            Inputs.OnInputDown();
            picked = positionProvider.PickComponent();
            Debug.Log("picked = " + (picked ? picked.name : "null"));
        }

        public void OpenRecycleBin()
        {
            if (!picked)
                return;
            onRecycleBin = true;
            recycleBin.sprite = recycleBinOpened;
            picked.SetMaterial(warningMat);
            //print("OpenRecycleBin " + picked);
        }

        public void DropToRecycleBin()
        {
            if (!picked)
                return;
            onRecycleBin = false;
            recycleBin.sprite = recycleBinDefault;
            picked.SetMaterial(defaultMat);
            //print("DropToRecycleBin " + picked);
        }

        public void RecycleComponent()
        {
            onRecycleBin = false;
            recycleBin.sprite = recycleBinDefault;
            positionProvider.RecycleComponent();
        }

        public void OnBeginDrag()
        {
            if (picked && picked == ScaffoldingComponent.selected)
                positionProvider.SetObjectActive(picked);
        }

        public void OnEndDrag()
        {
            //print("onRecycleBin" + onRecycleBin);
            if (picked && picked == ScaffoldingComponent.selected)
            {
                if (onRecycleBin)
                    RecycleComponent();
                else
                    positionProvider.SetObjectInactive();
            }
        }

        public void OnUp()
        {
            //print("OnUp");
            isDown = false;
            Inputs.OnInputUp();
            if (Inputs.InputType() == Click.Tap && picked)
            {
                bool same = picked == ScaffoldingComponent.selected;
                if (ScaffoldingComponent.selected)
                    ScaffoldingComponent.selected.Deselect();
                if (!same)
                    picked.Select();
            }
        }
    }
}
