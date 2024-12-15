using System.Collections.Generic;
using Actors.Player;
using Interactables;
using UnityEngine;

namespace Items {
    [RequireComponent(typeof(BoxCollider2D), typeof(InteractableFinder), typeof(GridSnap))]
    public class EquippableItemObject : MonoBehaviour, IEquippableItem {

        [SerializeField] protected ItemType type;
        [SerializeField] private List<ClassType> toggleableInteraction;
        private bool m_toggle;
        
        private InteractableFinder m_interactableFinder;
        
        private ClassType m_currentClass;

        private bool m_wasDropped;
        
        private GridSnap m_gridSnap;

        private void Start() {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            
            boxCollider.isTrigger = true;
            ItemType = type;
            
            m_interactableFinder = gameObject.GetComponent<InteractableFinder>();
            
            m_gridSnap = gameObject.GetComponent<GridSnap>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            
            Debug.Log("Entered");
            
            if (other.gameObject.TryGetComponent(out CharacterStateController character) 
            && character.CurrentItemType == ItemType.None) {
                Equip(character);
            }
            
        }

        private void OnTriggerExit2D(Collider2D other) {
        
            Debug.Log("Exited");
            
            if (other.gameObject.TryGetComponent(out CharacterStateController character)) {
                m_wasDropped = false;
            }
        }

        public ItemType ItemType { get; set; }

        public void Equip(CharacterStateController character) {
            
            if (m_wasDropped) return;

            m_interactableFinder.enabled = true;
            m_currentClass = character.ClassType;
            transform.parent = character.transform;
            transform.rotation = character.transform.rotation;
            transform.localPosition = Vector3.zero;
            character.PlayerEquipItem(this);
            Debug.Log("Equipped Item");
        }

        public void Unequip() {
            
            if (toggleableInteraction.Contains(m_currentClass)) {
                m_interactableFinder.Interact(m_currentClass, type, "Stop");
                return;
            }
            m_interactableFinder.enabled = false;
            transform.parent = null;
            transform.rotation = Quaternion.identity;
            m_currentClass = ClassType.None;
            m_wasDropped = true;
            StartCoroutine(m_gridSnap.SnapCoroutine());
            Debug.Log("Unequipped Item");
        }

        public virtual void Use() {
            
            if (!m_interactableFinder.HasInteractable()) return;

            if (toggleableInteraction.Contains(m_currentClass)) {
                m_toggle = !m_toggle;
                Debug.Log(m_toggle ? "Start" : "Stop");
                m_interactableFinder.Interact(m_currentClass, type, m_toggle ? "Start" : "Stop");
                return;
            }
            
            m_interactableFinder.Interact(m_currentClass, type);
        }
    }
}