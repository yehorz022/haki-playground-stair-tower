using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.Helpers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.UserInterface
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] Dropdown dropdown;
        private InputHandler inputHandler;
        private ProjectLayout projectLayout;
        private AssemblyFactory assemblyFactory;
        private ComponentsLayout componentsLayout;

        void Start()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            projectLayout = FindObjectOfType<ProjectLayout>();
            assemblyFactory = FindObjectOfType<AssemblyFactory>();
            componentsLayout = FindObjectOfType<ComponentsLayout>();
        }

        public void Show() {
            dropdown.value = 0;
            Routine.MovePivot(transform.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), .18f); // opening animation
        }
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
                case 2:
                    Hide();
                    inputHandler.Hide();
                    assemblyFactory.Hide();
                    componentsLayout.Hide();
                    projectLayout.Show();
                    break;

                default:
                    break;
            }
        }
    }
}