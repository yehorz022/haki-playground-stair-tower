using Assets.Scripts.Shared.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PanelComponent : MonoBehaviour
    {
        public static HakiComponent selectedComponentPrefab; // this selection is used to create object 
        [SerializeField] private Image image;
        private HakiComponent component;
        private PropertiesWindow propertiesWindow;
        
        void Start()
        {
            propertiesWindow = FindObjectOfType<PropertiesWindow>();
        }

        public void Initialize(HakiComponent component, Sprite icon)
        {
            this.component = component;
            image.sprite = icon;
        }

        public void Select()
        {
            selectedComponentPrefab = component;
        }

        public void ViewProperties()
        {
            propertiesWindow.Show(image.sprite, component.name, component.name);
            AudioManager.instance.PlaySound(SoundID.Click);
        }
    }
}