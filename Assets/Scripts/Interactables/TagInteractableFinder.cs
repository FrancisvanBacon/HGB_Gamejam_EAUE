using UnityEngine;

namespace Interactables {
    public class TagInteractableFinder : InteractableFinder {

        public string InteractableTag;

        protected override void HandleRaycastHit(RaycastHit2D hit) {
            
            Debug.DrawRay(transform.position, gameObject.transform.up * hit.distance, Color.yellow);
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            
            if (interactable == null || 
            interactable == m_currentInteractable ||
            !hit.collider.gameObject.CompareTag(InteractableTag)) return;
            
            m_currentInteractable?.Deselect();

            m_currentInteractable = interactable;
            m_currentInteractable.Select();
            
            Debug.DrawRay(transform.position, gameObject.transform.up * hit.distance, Color.red);
            
        }
        
    }
}