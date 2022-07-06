using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.FPS.UI
{
    public class OpenLinkButton : MonoBehaviour
    {
        public string linkUrl = "";

        public void LoadTargetUrl()
        {
            Application.OpenURL(linkUrl);
        }
    }
}