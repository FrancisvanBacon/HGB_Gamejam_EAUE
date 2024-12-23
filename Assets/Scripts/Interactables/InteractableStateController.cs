using System;
using System.Collections.Generic;
using Actors;
using Actors.Player;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class InteractableStateController : ActorStateController, IInteractable {
        
        [SerializeField] private List<ItemReaction> itemInteractions;
        
        private Dictionary<string, UnityEvent> m_eventDictionary = new Dictionary<string, UnityEvent>();
        
        [SerializeField] private UnityEvent onSelect;
        [SerializeField] private UnityEvent onDeselect;
        
        protected override void Start() {
            base.Start();

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
                //Debug.Log(classType.ToString() + item.ToString() + param + "Succesfully interacted with Interactable");
            }
            else {
                //Debug.Log(classType.ToString() + item.ToString() + param + "Interactable did not react");
            }
            
        }

        public void Select() {

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null) return;

            spriteRenderer.material = Resources.Load<Material>("Materials/Sprite-Unlit-Outline");
            
            onSelect?.Invoke();
        }

        public void Deselect() {

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            
            if (spriteRenderer == null) return;

            spriteRenderer.material = Resources.Load<Material>("Materials/Sprite-Lit-Default");
            
            onDeselect?.Invoke();
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