using UnityEngine;
using Player;

namespace Player
{
    public class SetVROrNot : MonoBehaviour
    {
        [SerializeField] private GameObject XRRig;
        [SerializeField] private GameObject WebGLRig;

        private bool isOnXRDevice = false;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                isOnXRDevice = true;
            }

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                isOnXRDevice = false;
            }

            PlayerStats.GameOver += GameStop;
            PlayerStats.GameRestart += GameRestart;

            XRRig.SetActive(isOnXRDevice);
            WebGLRig.SetActive(!isOnXRDevice);
        }

        private void GameStop()
        {
            if (!isOnXRDevice)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            XRRig.SetActive(false);
            WebGLRig.SetActive(false);
        }

        private void GameRestart()
        {
            if (!isOnXRDevice)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            XRRig.SetActive(isOnXRDevice);
            WebGLRig.SetActive(!isOnXRDevice);
        }
    }
}
