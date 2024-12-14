using System;
using System.Collections.Generic;
using Actors.Items;
using Actors.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class InteractableObject : MonoBehaviour, IInteractable {

        [SerializeField] private List<ItemReaction> itemInteractions;
        
        private Dictionary<string, UnityEvent> m_eventDictionary = new Dictionary<string, UnityEvent>();
        
        private void Start() {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();
            if (image != null) {
                image.color = Color.yellow;
            }

            foreach (var itemReaction in itemInteractions) {
                m_eventDictionary.Add(itemReaction.ClassType.ToString() + itemReaction.ItemType.ToString(), itemReaction.OnReaction);
            }
        }
        
        public void Interact(ClassType classType, ItemType item) {
            if (m_eventDictionary.TryGetValue(classType.ToString() + item.ToString(), out UnityEvent reaction)) {
                reaction?.Invoke();
            }
        }

        public void Select() {
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();

            if (image != null) {
                image.color = Color.red;
            }
        }

        public void Deselect() {
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();

            if (image != null) {
                image.color = Color.yellow;
            }
        }
    }

    [Serializable]
    public class ItemReaction {
        public ClassType ClassType;
        public ItemType ItemType;
        public UnityEvent OnReaction;
    } 
}