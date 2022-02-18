using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyWindow : MonoBehaviour
{
    public static PropertyWindow o;
    public Image img;
    public Text title;
    public Text id;

    public Coroutine routineDrag;
    public Coroutine routineHide;
    public void Awake()
    {
        o = this;
    }
    // Start is called before the first frame update
    public void Hide() {
        if (routineDrag != null)
            DependancyInjectionManager.instance.StopCoroutine(routineDrag);
        routineDrag = Animate.Lerp(transform.position.y, Screen.height * 1.2f, .18f,
            (val) => {
                transform.position = new Vector3(transform.position.x, val, transform.position.z);
            }); // moving animation
    }

    public void Show(Sprite img, string title, string id) {
        this.img.sprite = img;
        this.title.text = "Name : " + title;
        this.id.text = "ID : " + id;
        if (routineDrag != null)
            DependancyInjectionManager.instance.StopCoroutine(routineDrag);
        if (routineHide != null)
            DependancyInjectionManager.instance.StopCoroutine(routineHide);
        routineDrag = Animate.Lerp(transform.position.y, Screen.height, .18f,
            (val) => {
                transform.position = new Vector3(transform.position.x, val, transform.position.z);
            }); // moving animation
        routineHide = Code.WaitAndCall(3, Hide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
