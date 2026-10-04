using UnityEngine;
using AKIXA.GameplayInput;

namespace AKIXA.Interaction
{
    public class CharacterInteractor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputReader inputReader;

        [Header("Interaction")]
        [SerializeField] private float interactionRadius = 2f;

        public GameObject Owner => gameObject;

        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.InteractPressed += TryInteract;
            }
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.InteractPressed -= TryInteract;
            }
        }

        private void TryInteract()
        {
            Collider[] colliders =
                Physics.OverlapSphere(
                    transform.position,
                    interactionRadius,
                    ~0,
                    QueryTriggerInteraction.Collide
                );

            IInteractable closestInteractable = null;
            float closestDistance = float.MaxValue;

            foreach (Collider hit in colliders)
            {
                IInteractable interactable =
                    hit.GetComponentInParent<IInteractable>();

                if (interactable == null)
                    continue;

                float distance =
                    Vector3.Distance(
                        transform.position,
                        hit.transform.position
                    );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }

            closestInteractable?.Interact(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(
                transform.position,
                interactionRadius
            );
        }
    }
}