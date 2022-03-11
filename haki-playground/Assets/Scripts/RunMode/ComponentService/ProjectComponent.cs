using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ProjectComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject delete;
        int no;
        private ProjectLayout projectLayout;

        void Start()
        {
            projectLayout = FindObjectOfType<ProjectLayout>();
        }

        public void Initialize(int no, Sprite sprite)
        {
            this.no = no;
            image.sprite = sprite;
            delete.SetActive(false);
            GetComponent<Animator>().Rebind();
        }

        public void Select()
        {
            if (!delete.activeSelf)
            {
                if (no == projectLayout.scrollView.totalPanels - 1)
                {
                    PlayerPrefs.SetString("ProjectID" + no, Code.GenerateRandomString ());
                    PlayerPrefs.SetInt("ProjectsCount", PlayerPrefs.GetInt("ProjectsCount") + 1);
                    projectLayout.Initialize();
                }
                AudioManager.instance.PlaySound(SoundID.Click);
                projectLayout.LoadProject(PlayerPrefs.GetString("ProjectID" + no));
            }
            else
                Delete();
        }
        public void ShowDeleteIcon()
        {
            if (no >= PlayerPrefs.GetInt("ProjectsCount"))
                return;
            if (delete.activeSelf)
                Routine.Scale(delete.transform, Vector3.one, Vector3.zero); // Scaling animation
            else
                Routine.Scale(delete.transform, Vector3.zero, Vector3.one); // Scaling animation
            delete.SetActive(!delete.activeSelf);
        }

        public void Delete()
        {
            int projectsCount = PlayerPrefs.GetInt("ProjectsCount", 0);
            for (int i = no + 1; i < projectsCount; i++)
                PlayerPrefs.SetString("ProjectID" + (i - 1), PlayerPrefs.GetString("ProjectID" + i));
            PlayerPrefs.SetInt("ProjectsCount", PlayerPrefs.GetInt("ProjectsCount") - 1);
            Routine.WaitAndCall(.1f, () => GetComponent<Animator>().Play("Close"));// Scaling animation
            Routine.WaitAndCall(.5f, () => projectLayout.Initialize());
        }
    }

}