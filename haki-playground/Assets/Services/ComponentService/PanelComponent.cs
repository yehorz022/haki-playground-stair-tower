using Assets.Services.ComponentConnection;
using UnityEngine;
using UnityEngine.UI;

public class PanelComponent : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private ScaffoldingComponent element;
    public static ScaffoldingComponent selectedScaffoldingComponent;

    public void Initialize(ScaffoldingComponent element)
    {
        this.element = element;
        image.sprite = element.icon;
    }

    public void Select()
    {
        selectedScaffoldingComponent = element;
    }

    public void ViewProperties()
    {
        PropertyWindow.instance.Show(element.icon, element.name, element.name);
    }
}
