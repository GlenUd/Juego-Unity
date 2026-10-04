using UnityEngine;
using AKIXA.Weapons;

namespace AKIXA.Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        [Header("Sockets")]
        [SerializeField]
        private Transform rightHandWeaponSocket;

        [Header("Prototype")]
        [SerializeField]
        private WeaponDefinition startingWeapon;

        public WeaponDefinition EquippedWeapon
        {
            get;
            private set;
        }

        public WeaponRuntime CurrentWeaponRuntime
        {
            get;
            private set;
        }

        private GameObject currentWeaponVisual;

        private void Start()
        {
            if (startingWeapon != null)
            {
                EquipWeapon(startingWeapon);
            }
        }

        public void EquipWeapon(
            WeaponDefinition weapon)
        {
            if (weapon == null)
                return;

            UnequipWeapon();

            EquippedWeapon = weapon;

            CreateWeaponVisual();
        }

        public void UnequipWeapon()
        {
            EquippedWeapon = null;
            CurrentWeaponRuntime = null;

            if (currentWeaponVisual != null)
            {
                Destroy(currentWeaponVisual);
                currentWeaponVisual = null;
            }
        }

        private void CreateWeaponVisual()
        {
            if (EquippedWeapon == null ||
                EquippedWeapon.WeaponPrefab == null ||
                rightHandWeaponSocket == null)
            {
                return;
            }

            currentWeaponVisual = Instantiate(
                EquippedWeapon.WeaponPrefab,
                rightHandWeaponSocket
            );

            currentWeaponVisual.transform.localPosition =
                Vector3.zero;

            currentWeaponVisual.transform.localRotation =
                Quaternion.identity;

            currentWeaponVisual.transform.localScale =
                Vector3.one;

            CurrentWeaponRuntime =
                currentWeaponVisual.GetComponent<WeaponRuntime>();

            if (CurrentWeaponRuntime != null)
            {
                CurrentWeaponRuntime.Initialize(
                    EquippedWeapon,
                    transform.root
                );
            }
        }
    }
}