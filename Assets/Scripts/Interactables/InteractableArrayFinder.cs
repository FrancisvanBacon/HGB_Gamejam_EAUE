using System.Collections.Generic;
using System.Numerics;
using Actors.Player;
using Items;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Interactables {
    [RequireComponent(typeof(Collider2D))]
    public class InteractableArrayFinder : InteractableFinder {
        
        private List<IInteractable> m_interactables = new List<IInteractable>();
        public override bool HasInteractable() => m_interactables.Count > 0;

        [SerializeField] private bool adressStopGlobally;

        private void Start() {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }

        protected override void FixedUpdate() {

            foreach (var interactable in m_interactables) {
                interactable.Deselect();
            }
            
            m_interactables.Clear();
            
            var colliders = Physics2D.CircleCastAll(
            gameObject.transform.position, 
            searchRadius, 
            Vector2.zero, 
            LayerMask.GetMask("Interactable"));

            foreach (var collider in colliders) {
                
                if (collider.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable)) {
                    m_interactables.Add(interactable);
                    interactable.Select();
                }

            }

        }

        private void OnCollisionExit2D(Collision2D collision) {
        
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            if (interactable != null && m_interactables.Contains(interactable)) {
                m_interactables.Remove(interactable);
            }

        }
        
        protected override void OnDisable() {
            m_interactables.Clear();
        }
        
        public override void Interact(ClassType classType, ItemType itemType, string param = "") {

            if (adressStopGlobally && param.Equals("Stop")) {
                var interactables = GameObject.FindObjectsByType<InteractableObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                foreach (var interactable in interactables) {
                    interactable.Interact(classType, itemType, param);
                }

            }
            
            foreach (IInteractable interactable in m_interactables) {
                interactable.Interact(classType, itemType, param);
            }
        }
        
    }
}