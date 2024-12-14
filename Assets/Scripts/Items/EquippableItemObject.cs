using Actors.Player;
using Interactables;
using UnityEngine;

namespace Items {
    [RequireComponent(typeof(BoxCollider2D), typeof(InteractableFinder))]
    public class EquippableItemObject : MonoBehaviour, IEquippableItem {

        [SerializeField] private ItemType type;
        
        private InteractableFinder m_interactableFinder;
        
        private ClassType m_currentClass;

        private bool m_wasDropped;

        private void Start() {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            
            boxCollider.isTrigger = true;
            ItemType = type;
            
            m_interactableFinder = gameObject.GetComponent<InteractableFinder>();
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
            
            m_interactableFinder.enabled = false;
            transform.parent = null;
            transform.rotation = Quaternion.identity;
            m_currentClass = ClassType.None;
            m_wasDropped = true;
            Debug.Log("Unequipped Item");
        }

        public void Use() {
            
            if (m_interactableFinder.CurrentInteractable() == null) return;
            
            m_interactableFinder.CurrentInteractable().Interact(m_currentClass, type);
        }
    }
}