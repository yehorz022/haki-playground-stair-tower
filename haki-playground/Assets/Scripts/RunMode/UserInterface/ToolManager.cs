using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.UserInterface
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private GameObject assembly;
        [SerializeField] private GameObject component;

        
        public void OnToolChanged(Dropdown dropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    assembly.SetActive(true);
                    component.SetActive(false);

                    break;
                case 1:
                    component.SetActive(true);
                    assembly.SetActive(false);
                    break;

                default:
                    break;
            }
        }
    }
}