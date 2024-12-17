using System.Collections.Generic;
using UnityEngine;

namespace Interactables {
    public class TagInteractableFinder : InteractableFinder {

        public List<string> InteractableTags;

        protected override void HandleRaycastHit(RaycastHit2D hit) {
            
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            
            if (interactable == null || 
            interactable == m_currentInteractable ||
            !InteractableTags.Contains(hit.collider.tag)) return;
            
            m_currentInteractable?.Deselect();

            m_currentInteractable = interactable;
            m_currentInteractable.Select();
            
        }
        
    }
}