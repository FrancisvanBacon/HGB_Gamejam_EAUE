using System;
using System.Collections.Generic;
using Actors;
using Actors.Player;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class InteractableObject : ActorStateController, IInteractable {
        
        [SerializeField] private List<ItemReaction> itemInteractions;
        
        private Dictionary<string, UnityEvent> m_eventDictionary = new Dictionary<string, UnityEvent>();
        
        protected override void Start() {
            base.Start();
            
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();
            if (image != null) {
                image.color = Color.yellow;
            }
            
            foreach (var itemReaction in itemInteractions) {
                m_eventDictionary.Add(itemReaction.ClassType.ToString() + 
                itemReaction.ItemType.ToString() + 
                itemReaction.optionalparam, 
                itemReaction.OnReaction);
            }
        }
        
        public void Interact(ClassType classType, ItemType item, string param = "") {
            if (m_eventDictionary.TryGetValue(classType.ToString() + item.ToString() + param, out UnityEvent reaction)) {
                reaction?.Invoke();
                Debug.Log(classType.ToString() + item.ToString() + param + "Succesfully interacted with Interactable");
            }
            else {
                Debug.Log(classType.ToString() + item.ToString() + param + "Interactable did not react");
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
        public string optionalparam;
        public UnityEvent OnReaction;
    } 
}