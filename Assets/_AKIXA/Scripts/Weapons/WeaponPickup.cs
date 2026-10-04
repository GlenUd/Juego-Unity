using UnityEngine;
using AKIXA.Interaction;
using AKIXA.Equipment;

namespace AKIXA.Weapons
{
    public class WeaponPickup : MonoBehaviour, IInteractable
    {
        [Header("Weapon")]
        [SerializeField]
        private WeaponDefinition weapon;

        public void Interact(CharacterInteractor interactor)
        {
            if (weapon == null || interactor == null)
                return;

            EquipmentManager equipment =
                interactor.Owner.GetComponent<EquipmentManager>();

            if (equipment == null)
                return;

            equipment.EquipWeapon(weapon);

            Destroy(gameObject);
        }
    }
}