using Assets.Services.ComponentConnection;
using UnityEngine;
using UnityEngine.UI;

public class PanelComponent : MonoBehaviour
{

    [SerializeField]
    private RawImage image;
    [SerializeField]
    private Text text;

    private PositionProvider positionProvider;

    void Start()
    {
        positionProvider = FindObjectOfType<PositionProvider>();
    }

    private ComponentConnectionService element;
    public void Spawn()
    {
        positionProvider.SetObject(element);
    }

    public void SetImage(Texture2D texture, ComponentConnectionService element)
    {
        this.element = element;
        image.texture = texture;
        text.text = element.name;
    }
}
