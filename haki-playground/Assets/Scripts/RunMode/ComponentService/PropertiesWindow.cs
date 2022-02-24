using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PropertiesWindow : MonoBehaviour
    {
        public Image img;
        public Text title;
        public Text id;

        public Coroutine routineDrag;
        public Coroutine routineHide;

        public void Hide() {
            Routine.Stop(routineDrag);
            routineDrag = Routine.Lerp(transform.position.y, Screen.height * 1.2f, .18f,
                (val) => {
                    transform.position = new Vector3(transform.position.x, val, transform.position.z);
                }); // moving animation
        }

        public void Show(Sprite img, string title, string id) {
            this.img.sprite = img;
            this.title.text = "Name : " + title;
            this.id.text = "ID : " + id;
            Routine.Stop(routineDrag);
            Routine.Stop(routineHide);
            routineDrag = Routine.Lerp(transform.position.y, Screen.height, .18f,
                (val) => {
                    transform.position = new Vector3(transform.position.x, val, transform.position.z);
                }); // moving animation
            routineHide = Routine.WaitAndCall(3, Hide);
        }
    }
}
