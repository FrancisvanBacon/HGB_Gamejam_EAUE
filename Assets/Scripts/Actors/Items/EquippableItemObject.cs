using Actors.Player;
using Interactables;
using UnityEngine;

namespace Actors.Items {
    [RequireComponent(typeof(BoxCollider2D))]
    public class EquippableItemObject : MonoBehaviour, IEquippableItem {

        [SerializeField] private ItemType type;
        
        private InteractableFinder m_interactableFinder;
        
        private ClassType m_currentClass;

        private void Start() {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            
            boxCollider.isTrigger = true;
            ItemType = type;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            
            Debug.Log("Entered");
            
            if (other.gameObject.TryGetComponent(out CharacterStateController character) 
            && character.CurrentItemType == ItemType.None) {
                Equip(character);
            }
            
        }

        public ItemType ItemType { get; set; }

        public void Equip(CharacterStateController character) {

            m_currentClass = character.ClassType;
            transform.parent = character.transform;
            transform.localPosition = Vector3.zero;
            Debug.Log("Equipped Item");
        }

        public void Unequip() {
        
            transform.parent = null;
            m_currentClass = ClassType.None;
            Debug.Log("Unequipped Item");
        }

        public void Use() {
            
            if (m_interactableFinder.CurrentInteractable() == null) return;
            
            m_interactableFinder.CurrentInteractable().Interact(m_currentClass, type);
            Debug.Log(m_currentClass.ToString() + type.ToString());
        }
    }
}