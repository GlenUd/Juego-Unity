using UnityEngine;

namespace AKIXA.Weapons
{
    public class WeaponRuntime : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WeaponHitbox hitbox;

        public WeaponDefinition Definition { get; private set; }

        public void Initialize(
            WeaponDefinition definition,
            Transform owner)
        {
            Definition = definition;

            if (hitbox != null &&
                Definition != null)
            {
                hitbox.Configure(
                    owner,
                    Definition.Damage
                );

                hitbox.DisableHitbox();
            }
        }

        public void EnableHitbox()
        {
            if (hitbox != null)
            {
                hitbox.EnableHitbox();
            }
        }

        public void DisableHitbox()
        {
            if (hitbox != null)
            {
                hitbox.DisableHitbox();
            }
        }
    }
}