using System;
using StarterAssets;
using UnityEngine;

namespace DefaultNamespace
{
    public class RepairNearbyWalls : MonoBehaviour
    {
        [SerializeField]
        private StarterAssetsInputs _input;


        private void Start()
        {
            _input.OnRepair.AddListener(Repair);
        }

        private void Repair()
        {
            var colliders = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("DestructableNonExplosive"));

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<Damageable>(out var damageable))
                {
                    if (damageable.MaxHealth < damageable.CurrentHealth + 20)
                    {
                        damageable.CurrentHealth = damageable.MaxHealth;
                    }
                    damageable.CurrentHealth += 20;
                }
            }
        }
    }
}