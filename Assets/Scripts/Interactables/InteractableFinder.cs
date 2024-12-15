using System;
using Actors.Player;
using Items;
using UnityEngine;

namespace Interactables {
    public class InteractableFinder : MonoBehaviour {
        
        [SerializeField] protected float searchRadius = 5f;
        private IInteractable m_currentInteractable;
        public virtual bool HasInteractable() => m_currentInteractable != null;

        protected virtual void FixedUpdate() {
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            gameObject.transform.up, 
            searchRadius, 
            LayerMask.GetMask("Interactable"));
            
            if (hit) {
                HandleRaycastHit(hit);
            }
            else if (m_currentInteractable != null) {
                m_currentInteractable.Deselect();
                m_currentInteractable = null;
            }
            
        }

        protected virtual void OnDisable() {
            m_currentInteractable = null;
        }

        public virtual void Interact(ClassType classType, ItemType itemType, string param = "") {
            if (m_currentInteractable != null) {
                m_currentInteractable.Interact(classType, itemType, param);
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