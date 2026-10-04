using System.Collections.Generic;
using UnityEngine;
using AKIXA.Combat;

namespace AKIXA.Weapons
{
    public class WeaponHitbox : MonoBehaviour
    {
        [SerializeField] private Collider hitboxCollider;

        private float damage;
        private Transform ownerRoot;

        private readonly HashSet<Damageable>
            damagedTargets = new HashSet<Damageable>();

        private void Awake()
        {
            if (hitboxCollider == null)
            {
                hitboxCollider = GetComponent<Collider>();
            }

            DisableHitbox();
        }

        public void Configure(
            Transform owner,
            float weaponDamage)
        {
            ownerRoot = owner;
            damage = weaponDamage;
        }

        public void EnableHitbox()
        {
            damagedTargets.Clear();

            if (hitboxCollider != null)
            {
                hitboxCollider.enabled = true;
            }
        }

        public void DisableHitbox()
        {
            if (hitboxCollider != null)
            {
                hitboxCollider.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ownerRoot != null &&
                other.transform.root == ownerRoot)
            {
                return;
            }

            Damageable damageable =
                other.GetComponentInParent<Damageable>();

            if (damageable == null)
                return;

            if (damagedTargets.Contains(damageable))
                return;

            damagedTargets.Add(damageable);

            damageable.TakeDamage(damage);
        }
    }
}