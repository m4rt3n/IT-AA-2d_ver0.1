using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.UI.Menus
{
    public class StartMenuController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuManager menuManager;

        [Header("State")]
        [SerializeField] private bool openOnStart;

        private void Start()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }

            if (openOnStart && menuManager != null)
            {
                menuManager.ShowStartMenu();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void OpenLoadGame()
        {
            if (menuManager != null)
            {
                menuManager.ShowLoadGamePanel();
            }
        }

        public void CloseMenu()
        {
            if (menuManager != null)
            {
                menuManager.HideAll();
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}