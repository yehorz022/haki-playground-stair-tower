using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.Helpers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.UserInterface
{
    public class ToolManager : MonoBehaviour
    {
        private InputHandler inputHandler;
        private AssemblyFactory assemblyFactory;
        private ComponentsLayout componentsLayout;

        void Start()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            assemblyFactory = FindObjectOfType<AssemblyFactory>();
            componentsLayout = FindObjectOfType<ComponentsLayout>();
        }

        public void Show() => Routine.MovePivot(transform.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), .18f); // opening animation

        public void Hide() => Routine.MovePivot(transform.GetComponent<RectTransform>(), new Vector2(1, 1), new Vector2(0, 1), .18f); // closing animation

        public void OnToolChanged(Dropdown dropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    assemblyFactory.Show();
                    componentsLayout.Hide();
                    inputHandler.Hide();

                    break;
                case 1:
                    assemblyFactory.Hide();
                    componentsLayout.Show();
                    inputHandler.Show();
                    break;

                default:
                    break;
            }
        }
    }
}