using UnityEngine;
using Google.XR.Cardboard;

public class PlayerWeapon : MonoBehaviour
{
    private const float _maxDistance = 20;
    private GameObject _gazedAtObject = null;

    [SerializeField] private ParticleSystem gunpowderReference;
    [SerializeField] private ParticleSystem onHitEffect;

    private Transform raycastStartPos;

    private void Start()
    {
        raycastStartPos = transform.GetChild(0);
    }

    void Update()
    {
        if (Api.IsTriggerPressed)
        {
            gunpowderReference.Play();

            RaycastHit hit;
            if (Physics.Raycast(raycastStartPos.position, raycastStartPos.forward, out hit, _maxDistance))
            {
                if (_gazedAtObject != hit.transform.gameObject && hit.transform.gameObject.layer == 9)
                {
                    _gazedAtObject = hit.transform.gameObject;
                }

                _gazedAtObject?.SendMessage("OnPointerClick");
                OnHitEffect(hit.point);
            }
            else
            {
                _gazedAtObject = null;
            }         
        }
    }

    void OnHitEffect(Vector3 pos)
    {
        onHitEffect.transform.position = pos;
        onHitEffect.transform.rotation = transform.rotation;
        onHitEffect.Play();
    }
}