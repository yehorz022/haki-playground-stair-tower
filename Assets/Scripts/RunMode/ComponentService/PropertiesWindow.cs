using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    //>>> This class responsible for properties bar shown at the top when click the item from scroller
    //>>> will modify it and make it more generic later 

    public class PropertiesWindow : MonoBehaviour
    {
        public Image img;
        public Text title;
        public Text id;

        public Coroutine routineDrag;
        public Coroutine routineHide;

        public void Hide() {
            Routine.Stop(routineDrag);
            routineDrag = Routine.MovePivot(transform.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(0, 0), .18f); // moving animation
        }

        public void Show(Sprite img, string title, string id) {
            this.img.sprite = img;
            this.title.text = "Name : " + title;
            this.id.text = "ID : " + id;
            Routine.Stop(routineDrag);
            Routine.Stop(routineHide);
            routineDrag = routineDrag = Routine.MovePivot(transform.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(0, 1), .18f); // moving animation
            routineHide = Routine.WaitAndCall(3, Hide);
        }
    }
}
