using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unity.FPS.UI
{
    public class ExitApplicationButton : MonoBehaviour
    {
        void Start()
        {
            #if (UNITY_EDITOR || UNITY_STANDALONE)
                gameObject.SetActive(true);
            #elif UNITY_WEBGL
                gameObject.SetActive(false);
            #endif
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject
                && Input.GetButtonDown(GameConstants.k_ButtonNameSubmit))
            {
                ExitApplication();
            }
        }

        public void ExitApplication()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                Application.Quit();
            #endif
        }
    }
}