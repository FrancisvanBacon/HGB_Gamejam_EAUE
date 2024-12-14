using System.Collections.Generic;
using Actors.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interactables {
    public class InteractableObject : MonoBehaviour, IInteractable {

        [SerializeField] private List<ItemType> validItems;
        [SerializeField] private UnityEvent onValidInteraction;
        [SerializeField] private UnityEvent onInvalidInteraction;

        private void Start() {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();
            if (image != null) {
                image.color = Color.yellow;
            }
        }
        
        public void Interact(IEquippableItem item) {
            
        }

        public void Select() {
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();
            Debug.Log("Selected");

            if (image != null) {
                image.color = Color.red;
            }
        }

        public void Deselect() {
            Debug.Log("Deselected");
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();

            if (image != null) {
                image.color = Color.yellow;
            }
        }
    }
}