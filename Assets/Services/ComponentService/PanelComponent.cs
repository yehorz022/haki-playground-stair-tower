using System.Threading.Tasks;
using Assets.Services.ComponentConnection;
using Assets.Services.ComponentService;
using UnityEngine;
using UnityEngine.UI;

public class PanelComponent : MonoBehaviour
{
    [SerializeField]
    private Image img;

    private PositionProvider positionProvider;
    public static PanelComponent selected;

    void Start()
    {
        positionProvider = FindObjectOfType<PositionProvider>();
    }

    public void Select()
    {
        selected = this;
    }

    private ComponentConnectionService element;
    public static void Spawn()
    {
        selected.positionProvider.SetObject(selected.element);
    }

    public void SetImage(Texture2D texture, ComponentConnectionService element)
    {
        this.element = element;
        img.sprite = Graphik.TextureToSprite(texture);
    }

    public void ViewProperties()
    {
        PropertyWindow.o.Show(img.sprite, element.name, element.name);
    }
}
