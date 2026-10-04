using UnityEngine;

namespace AKIXA.Combat
{
    [CreateAssetMenu(
        fileName = "AttackProfile_New",
        menuName = "AKIXA/Combat/Attack Profile"
    )]
    public class AttackProfile : ScriptableObject
    {
        [Header("Timing")]
        public float duration = 0.55f;
        public float hitboxStart = 0.12f;
        public float hitboxDuration = 0.18f;

        [Header("Visual Rotation")]
        public Vector3 startEuler;
        public Vector3 impactEuler;
        public Vector3 endEuler;

        [Header("Visual Position")]
        public Vector3 startPosition;
        public Vector3 impactPosition;
        public Vector3 endPosition;
    }
}