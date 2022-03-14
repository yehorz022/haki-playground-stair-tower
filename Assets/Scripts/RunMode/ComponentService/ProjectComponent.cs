using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ProjectComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject deleteBtn;
        [SerializeField] private Text date;
        int no;
        string projectId;
        private ProjectLayout projectLayout;

        void Awake()
        {
            projectLayout = FindObjectOfType<ProjectLayout>();
        }

        public void Initialize(int no, string projectId, bool isNewProjectBtn)
        {
            this.no = no;
            deleteBtn.SetActive(false);
            GetComponent<Animator>().Rebind();
            image.sprite = isNewProjectBtn ? projectLayout.newProjectIcon : IO.LoadImage(projectId + "-icon");
            date.text = isNewProjectBtn ? "" : "Last Edited\n" + PlayerPrefs.GetString("ProjectLastEdited" + projectId);

        }

        public void Select()
        {
            if (!deleteBtn.activeSelf)
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
            if (deleteBtn.activeSelf)
                Routine.Scale(deleteBtn.transform, Vector3.one, Vector3.zero); // Scaling animation
            else
                Routine.Scale(deleteBtn.transform, Vector3.zero, Vector3.one); // Scaling animation
            deleteBtn.SetActive(!deleteBtn.activeSelf);
        }

        public void Delete()
        {
            IO.DeleteImage(PlayerPrefs.GetString("ProjectID" + no) + "-icon");
            int projectsCount = PlayerPrefs.GetInt("ProjectsCount", 0);
            for (int i = no + 1; i < projectsCount; i++)
                PlayerPrefs.SetString("ProjectID" + (i - 1), PlayerPrefs.GetString("ProjectID" + i));
            PlayerPrefs.SetInt("ProjectsCount", PlayerPrefs.GetInt("ProjectsCount") - 1);
            Routine.WaitAndCall(.1f, () => GetComponent<Animator>().Play("Close"));// Scaling animation
            Routine.WaitAndCall(.5f, () => projectLayout.Initialize());
            AudioManager.instance.PlaySound(SoundID.Trash);
        }
    }

}