using System;
using System.Collections.Generic;
using Actors.Player;
using Interactables;
using UnityEngine;
using UnityEngine.Events;

namespace Items {
    [RequireComponent(typeof(BoxCollider2D), typeof(FinderSwitcher), typeof(GridSnap))]
    public class EquippableItemObject : MonoBehaviour, IEquippableItem {

        [SerializeField] protected ItemType type;
        [SerializeField] private List<ClassType> optionalParamInteraction;
        
        private bool m_toggle;
        
        private FinderSwitcher m_FinderSwitcherr;
        private InteractableFinder m_interactableFinder;
        
        private CharacterStateController m_currentClass;

        private bool m_wasDropped;
        
        private GridSnap m_gridSnap;
        
        [SerializeField] private UnityEvent<ClassType, ItemType, string> onEquipped;
        [SerializeField] private UnityEvent<ClassType, ItemType, string> onUnequipped;

        private void Start() {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            
            boxCollider.isTrigger = true;
            ItemType = type;
            
            m_FinderSwitcherr = gameObject.GetComponent<FinderSwitcher>();
            
            m_gridSnap = gameObject.GetComponent<GridSnap>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            
            if (other.gameObject.TryGetComponent(out CharacterStateController character) 
            && character.CurrentItemType == ItemType.None) {
                Equip(character);
            }
            
        }

        private void OnTriggerExit2D(Collider2D other) {
            
            if (other.gameObject.TryGetComponent(out CharacterStateController character)) {
                m_wasDropped = false;
            }
        }

        public ItemType ItemType { get; set; }

        public void Equip(CharacterStateController character) {
            
            if (m_wasDropped) return;
            
            m_currentClass = character;
            transform.parent = character.transform;
            transform.rotation = character.transform.rotation;
            transform.localPosition = Vector3.zero;
            character.PlayerEquipItem(this);
            gameObject.GetComponent<Collider2D>().enabled = false;
            
            m_FinderSwitcherr.SwitchFinders(character.ClassType);
            m_interactableFinder = m_FinderSwitcherr.CurrentFinder;
            
            if (optionalParamInteraction.Contains(m_currentClass.ClassType)) {
                m_toggle = false;
            }
            
            onEquipped?.Invoke(m_currentClass.ClassType, type, "Equip");
        }

        public void Unequip() {
            
            if (optionalParamInteraction.Contains(m_currentClass.ClassType)) {
                m_interactableFinder.Interact(m_currentClass.ClassType, type, "Stop");
                m_currentClass.InterpretToggleInteraction(m_currentClass.ClassType, type, "Stop");
                m_toggle = true;
            }
            m_interactableFinder.enabled = false;
            transform.parent = null;
            transform.rotation = Quaternion.identity;
            
            onUnequipped?.Invoke(m_currentClass.ClassType, type, "Unequip");
            
            m_currentClass = null;
            m_wasDropped = true;
            gameObject.GetComponent<Collider2D>().enabled = true;
            StartCoroutine(m_gridSnap.SnapCoroutine());
        }

        public virtual void Use() {
            
            if (!m_interactableFinder.HasInteractable()) return;
            
            if (optionalParamInteraction.Contains(m_currentClass.ClassType)) {
                m_toggle = !m_toggle;
                m_interactableFinder.Interact(m_currentClass.ClassType, type, m_toggle ? "Start" : "Stop");
                m_currentClass.InterpretToggleInteraction(m_currentClass.ClassType, type, m_toggle ? "Start" : "Stop");
                return;
            }
            m_currentClass.InterpretToggleInteraction(m_currentClass.ClassType, type, "");
            m_interactableFinder.Interact(m_currentClass.ClassType, type);
        }

        public void StopInteraction() {
            m_interactableFinder.Interact(m_currentClass.ClassType, type, "Stop");
        }
    }

    [Serializable]
    public class ClassStringWrapper {
        public ClassType ClassType;
        public string value;
    }
}