using System;
using UnityEngine;

namespace Interactables {
    public class InteractableFinder : MonoBehaviour {
        
        
        [SerializeField] private float searchRadius = 5f;
        private IInteractable m_currentInteractable;

        void FixedUpdate() {
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, gameObject.transform.up, searchRadius);
            
            if (hit) {
                HandleRaycastHit(hit);
            }
            else if (m_currentInteractable != null) {
                m_currentInteractable.Deselect();
                m_currentInteractable = null;
            }
            
        }

        private void HandleRaycastHit(RaycastHit2D hit) {
            
            Debug.DrawRay(transform.position, gameObject.transform.up * hit.distance, Color.yellow);
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            
            if (interactable == null || interactable == m_currentInteractable) return;

            m_currentInteractable?.Deselect();

            m_currentInteractable = interactable;
            m_currentInteractable.Select();
            
            Debug.DrawRay(transform.position, gameObject.transform.up * hit.distance, Color.red);
        }

    }
}