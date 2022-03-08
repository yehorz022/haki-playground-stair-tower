using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ProjectComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        int id;
        private ProjectLayout projectLayout;

        void Start()
        {
            projectLayout = FindObjectOfType<ProjectLayout>();
        }

        public void Initialize(int id, Sprite sprite)
        {
            image.sprite = sprite;
            this.id = id;
        }

        public void Select()
        {
            if (id == projectLayout.scrollView.totalPanels - 1) { 
            PlayerPrefs.SetInt("ProjectsCount", PlayerPrefs.GetInt("ProjectsCount", 1) + 1);
                projectLayout.Initialize();
            }
            projectLayout.gameObject.SetActive(false);
            AudioManager.instance.PlaySound(SoundID.Click);
            projectLayout.LoadProject(id);
        }
    }

}