using Assets.Scripts.RunMode.ComponentConnection;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PanelComponent : MonoBehaviour
    {

        [SerializeField]
        private RawImage image;
        [SerializeField]
        private Text text;

        private PositionProvider.PositionProvider positionProvider;

        void Start()
        {
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();
        }

        private ScaffoldingComponent element;
        public void Spawn()
        {
            positionProvider.SetObject(element);
        }

        public void SetImage(Texture2D texture, ScaffoldingComponent element)        {
            this.element = element;
            image.texture = texture;
            text.text = element.name;
        }
    }
}