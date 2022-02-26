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
        public bool onRecycleBin;

        public static InputHandler instance;
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();
        }

        //void OnGUI()
        //{
        //    GUILayout.Label("onRecycleBin = " + onRecycleBin);
        //    GUILayout.Label("picked = " + (picked ? picked.name : "null"));
        //    GUILayout.Label("selected = " + (ScaffoldingComponent.selected ? ScaffoldingComponent.selected.name : "null"));
        //}

        //vvvvv    button listners functions for UI    vvvvv

        public void OnInputPanelDown()
        {
            Inputs.OnInputDown();
            picked = positionProvider.GetComponent();
        }

        public void OnInputPanelUp()
        {
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

        public void OnInputPanelBeginDrag()
        {
            if (picked)
            {
                if (picked == ScaffoldingComponent.selected)
                    positionProvider.PickComponent(picked);
                else
                    picked = null;
            }
        }

        public void OnInputPanelEndDrag()
        {
            if (picked && picked == ScaffoldingComponent.selected)
            {
                if (onRecycleBin)
                    OnClickRecycleButton();
                else
                    OnDropItem();
            }
        }

        public void OnEnterRecycleButton()
        {
            if (!picked)
                return;
            onRecycleBin = true;
            recycleBin.sprite = recycleBinOpened;
            picked.SetMaterial(warningMat);
        }

        public void OnExitRecycleButton()
        {
            if (!picked)
                return;
            onRecycleBin = false;
            recycleBin.sprite = recycleBinDefault;
            picked.SetMaterial(picked == ScaffoldingComponent.selected ? selectedMat : defaultMat);
        }
        //^^^^   button listners functions for UI   ^^^^

        //vvvv   helping functions for drag and drop features   vvvv
        public void OnClickRecycleButton()
        {
            onRecycleBin = false;
            recycleBin.sprite = recycleBinDefault;
            positionProvider.RecycleComponent();
            picked.SetMaterial(defaultMat);
            picked = null;
            AudioManager.instance.PlaySound(SoundID.Trash);
        }

        public void OnDropItem()
        {
            positionProvider.PlaceComponent();
            AudioManager.instance.PlaySound(SoundID.Drop);
            picked = null;
        }

        public void OnCreateItem()
        {
            picked = positionProvider.CreateAndPickComponent(PanelComponent.selectedComponentPrefab); //This script is under working...
        }
        //^^^^   helping functions for drag and drop features   ^^^^
    }
}
