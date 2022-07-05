using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.FPS.UI
{
    public class OpenLinkButton : MonoBehaviour
    {
        public string linkUrl = "";

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject
                && Input.GetButtonDown(GameConstants.k_ButtonNameSubmit))
            {
                LoadTargetUrl();
            }
        }

        public void LoadTargetUrl()
        {
            Application.OpenURL(linkUrl);
        }
    }
}