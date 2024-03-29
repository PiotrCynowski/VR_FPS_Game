using UnityEngine;
using Google.XR.Cardboard;

namespace Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private ParticleSystem gunpowderEffect;
        [SerializeField] private ParticleSystem onHitEffect;
        [SerializeField] private LayerMask bulletTargetLayer;

        private const float maxDistance = 20;
        private Transform raycastStartPos;

        private void Start()
        {
            raycastStartPos = transform.GetChild(0);
        }

        private void Update()
        {
            if (Api.IsTriggerPressed || Input.GetMouseButtonUp(0))
            {
                gunpowderEffect.Play();

                if (Physics.Raycast(raycastStartPos.position, raycastStartPos.forward, out RaycastHit hit, maxDistance, bulletTargetLayer))
                {
                    if (hit.transform.GetComponent<IHaveEnemyData>() != null)
                    {
                        hit.transform.GetComponent<IHaveEnemyData>().GetDamage();

                        OnHitEffect(hit.point);
                    }
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
}