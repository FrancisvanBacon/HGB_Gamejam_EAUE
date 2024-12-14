using Actors.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actors.Player {
    [RequireComponent(typeof(MoveController))]
    public class CharacterStateController : ActorStateController {
        
        [SerializeField] private ClassType classType;
        public ClassType ClassType => classType;
        
        private bool m_isPLayerControlled = false;
        public bool IsPlayerControlled => m_isPLayerControlled;
        
        private MoveController m_moveController;
        private IEquippableItem m_equippedItem;
        public ItemType CurrentItemType {
            get {
                if (m_equippedItem == null) return ItemType.None;
                return m_equippedItem.ItemType;
            }
        }

        protected override void Update() {
            base.Update();
        }

        private void Start() {
            m_moveController = gameObject.GetComponent<MoveController>();
        }

        public void EnablePlayerControl() {
            m_isPLayerControlled = true;
        }

        public void DisablePlayerControl() {
            m_moveController.Stop();
            m_isPLayerControlled = false;
        }
        
        public void PlayerMove(InputAction.CallbackContext context) {
            m_moveController.Move(context.ReadValue<Vector2>());
        }

        public void PlayerLook(InputAction.CallbackContext context) {
            m_moveController.Look(context.ReadValue<Vector2>());
        }

        public void PlayerUseItem() {
            
            if (m_equippedItem == null) return;
            m_equippedItem.Use();
        }

        public void PlayerDropItem() {
            if (m_equippedItem == null) return;
            
            m_equippedItem.Unequipt();
            m_equippedItem = null;
        }

        public void PlayerEquipItem(IEquippableItem equippedItem) {
            if (m_equippedItem != null) {
                m_equippedItem.Unequipt();
            }
            
            m_equippedItem = equippedItem;
        }
        
    }
}