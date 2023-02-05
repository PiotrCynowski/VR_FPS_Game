using UnityEngine;

public class SetVROrNot : MonoBehaviour
{
    [SerializeField] private GameObject XRRig;
    [SerializeField] private GameObject WebGLRig;
   
    private bool isOnXRDevice = false;

    private void Awake()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            isOnXRDevice = true;
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            isOnXRDevice = false;
        }

        XRRig.SetActive(isOnXRDevice);
        WebGLRig.SetActive(!isOnXRDevice);
    }
}
