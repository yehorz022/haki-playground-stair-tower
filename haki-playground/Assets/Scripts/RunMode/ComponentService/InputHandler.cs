using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Tools.Selector.Face;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class InputHandler : SceneMemberInjectDependencies
    {
        public Material defaultMat;
        public Material selectedMat;
        public Material warningMat;
        [SerializeField] Image trashIcon;
        [SerializeField] RectTransform trashTrans;
        [SerializeField] Sprite recycleBinDefault;
        [SerializeField] Sprite recycleBinOpened;

        [Inject] private ISelected<ScaffoldingComponent> Selected { get; set; }
        private PositionProvider.PositionProvider positionProvider;
        public HakiComponent picked;
        public bool onRecycleBin;

        public static InputHandler instance;

        void Awake() => instance = this;

        void Start() => positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();

        public void Show() => Routine.MovePivot(trashTrans, new Vector2(0, 1), new Vector2(0, 0), .18f); // opening animation

        public void Hide() => Routine.MovePivot(trashTrans, new Vector2(0, 0), new Vector2(0, 1), .18f); // closing animation

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
        }

        public void OnInputPanelUp()
        {
            Inputs.OnInputUp();
        }

        public void OnInputPanelBeginDrag()
        {
        }

        public void OnInputPanelEndDrag()
        {
            if (picked && Selected.TryGet(out var item)  && picked == item)
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
            trashIcon.sprite = recycleBinOpened;
            picked.SetMaterial(warningMat);
        }

        public void OnExitRecycleButton()
        {
            if (!picked)
                return;
            onRecycleBin = false;
            trashIcon.sprite = recycleBinDefault;
            picked.SetMaterial(defaultMat);
        }
        //^^^^   button listners functions for UI   ^^^^

        //vvvv   helping functions for drag and drop features   vvvv
        public void OnClickRecycleButton()
        {
            onRecycleBin = false;
            trashIcon.sprite = recycleBinDefault;
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
            picked = positionProvider.CreateAndPickComponent(PanelComponent.selectedComponentPrefab) ; //This script is under working...
        }
        //^^^^   helping functions for drag and drop features   ^^^^
    }
}

