using AKIXA.Combat;
using UnityEngine;

namespace AKIXA.Weapons
{
    [CreateAssetMenu(
        fileName = "Weapon_New",
        menuName = "AKIXA/Weapons/Weapon Definition"
    )]
    public class WeaponDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string weaponId;
        [SerializeField] private string displayName;
        [SerializeField] private WeaponType weaponType = WeaponType.Sword;

        [Header("Combat")]
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float staminaCost = 10f;
        [SerializeField] private float range = 1.5f;

        [Header("Attack Timing")]
        [SerializeField] private float attackDuration = 0.55f;
        [SerializeField] private float hitboxStartTime = 0.12f;
        [SerializeField] private float hitboxDuration = 0.18f;

        [Header("Visual")]
        [SerializeField] private GameObject weaponPrefab;

        [Header("Attack Profiles")]
        [SerializeField] private AttackProfile[] lightAttackCombo;
        public AttackProfile[] LightAttackCombo => lightAttackCombo;


        public string WeaponId => weaponId;
        public string DisplayName => displayName;
        public WeaponType WeaponType => weaponType;

        public float Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float StaminaCost => staminaCost;
        public float Range => range;

        public float AttackDuration => attackDuration;
        public float HitboxStartTime => hitboxStartTime;
        public float HitboxDuration => hitboxDuration;

        public GameObject WeaponPrefab => weaponPrefab;
    }
}