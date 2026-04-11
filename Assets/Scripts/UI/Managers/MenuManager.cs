using ITAA.UI.Menus;
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private LoadGamePanel loadGamePanel;
        [SerializeField] private GameObject backgroundDim;

        private void Start()
        {
            HideAll();
        }

        public void ShowStartMenu()
        {
            SetDim(true);
            SetActive(startMenuController, true);
            SetActive(loadGamePanel, false);
        }

        public void ShowLoadGamePanel()
        {
            SetDim(true);
            SetActive(startMenuController, false);
            SetActive(loadGamePanel, true);

            if (loadGamePanel != null)
            {
                loadGamePanel.Refresh();
            }
        }

        public void HideAll()
        {
            SetDim(false);
            SetActive(startMenuController, false);
            SetActive(loadGamePanel, false);
        }

        private void SetDim(bool visible)
        {
            if (backgroundDim != null)
            {
                backgroundDim.SetActive(visible);
            }
        }

        private void SetActive(MonoBehaviour target, bool visible)
        {
            if (target != null)
            {
                target.gameObject.SetActive(visible);
            }
        }
    }
}