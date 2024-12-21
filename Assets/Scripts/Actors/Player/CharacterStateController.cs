using System.Collections;
using System.Collections.Generic;
using Actors.Enemys;
using Interactables;
using Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Actors.Player {
    [RequireComponent(typeof(MoveController), typeof(InteractableFinder), typeof(Respawner))]
    public class CharacterStateController : ActorStateController {
        
        [SerializeField] private ClassType classType;

        [SerializeField] private List<ItemReaction> itemReactions;
        private Dictionary<string, ItemReaction> m_eventDictionary = new Dictionary<string, ItemReaction>();
        
        [SerializeField] protected InteractableFinder standardInteractFinder;

        [SerializeField] private UnityEvent<ClassType, ItemType> onItemAction;

        public ClassType ClassType => classType;
        
        private bool m_isPLayerControlled = false;
        public bool IsPlayerControlled => m_isPLayerControlled;

        private MoveController m_moveController;
        public void SetSpeed(float value) {
            speed = value;
            m_moveController.Speed = value;
        }
        
        private IEquippableItem m_equippedItem;
        
        public int CurrentDirection => m_moveController.GetDirection();

        public bool LockMovement;
        public bool LockRotation {
            get {
                return m_moveController.LockRotation;
            }
            set {
                m_moveController.LockRotation = value;
            }
        }

        public Vector2 LookInput => m_moveController.LookInput;
        public Vector2 MoveInput => m_moveController.MovementInput;
        
        public bool LockInput;
        public int MovmentConstraint = -1;
        public ItemType CurrentItemType {
            get {
                if (m_equippedItem == null) return ItemType.None;
                return m_equippedItem.ItemType;
            }
        }
        
        public Animator Animator;

        protected override void Start() {
            m_moveController = gameObject.GetComponent<MoveController>();
            m_moveController.Speed = Speed;
            foreach (var reaction in itemReactions) {
                m_eventDictionary.Add(reaction.ClassType.ToString() + 
                reaction.ItemType.ToString() + 
                reaction.optionalparam, 
                reaction);
            }
            if (Animator == null) Animator = gameObject.GetComponent<Animator>();
        }

        private void Update() {
            Animator.SetFloat("Dir_X", Mathf.Round(transform.up.x));
            Animator.SetFloat("Dir_Y", Mathf.Round(transform.up.y));
        }

        public void EnablePlayerControl() => m_isPLayerControlled = true;

        public void DisablePlayerControl() {
            m_moveController.Stop();
            m_isPLayerControlled = false;
        }

        public void StopPlayerMovement() {
            m_moveController.Stop();
        }

        public void PlayerMove(InputAction.CallbackContext context) {
        
            Vector2 input = context.ReadValue<Vector2>();
        
            if (MovmentConstraint != -1) {
            
                switch (MovmentConstraint) {
                    
                    case 0:
                        input = new Vector2(0, 1) * Mathf.Clamp01(context.ReadValue<Vector2>().y);
                        break;
                    case 1:
                        input = new Vector2(1, 0) * Mathf.Clamp01(context.ReadValue<Vector2>().x);
                        break;
                    case 2:
                        input = new Vector2(0, 1) * (Mathf.Clamp(context.ReadValue<Vector2>().y, -1, 0));
                        break;
                    case 3:
                        input = new Vector2(1, 0) * (Mathf.Clamp(context.ReadValue<Vector2>().y, -1, 0));
                        break;
                }
            }
        
            if (!LockMovement && !LockInput) {
                m_moveController.Move(input);
            }
            
        }

        public void PlayerLook(InputAction.CallbackContext context) {
        
            if (LockMovement || LockInput || LockRotation) return;
            
            Vector2 lookInput = context.ReadValue<Vector2>();

            m_moveController.Look(lookInput);
        }

        public void PlayerMouseLook(Vector2 mouseWorldPosition) {
            m_moveController.Look((Vector3)mouseWorldPosition - transform.position);
        }

        public void ResetLookInput() {
            m_moveController.Look(Vector2.zero);
        }

        public void PlayerUseItem() {
            
            if (m_equippedItem == null || LockInput) return;
            //StartCoroutine(ItemUseRoutine());
            onItemAction?.Invoke(classType, m_equippedItem.ItemType);
            m_equippedItem.Use();
        }

        public void PlayerDropItem() {
            if (m_equippedItem == null || LockInput) return;

            if (TradeItemsWithPlayer()) return;
            
            m_equippedItem.Unequip();
            m_equippedItem = null;
        }

        private EquippableItemObject DropItem() {
            if (m_equippedItem == null) return null;
            var item = m_equippedItem as EquippableItemObject;
            m_equippedItem.Unequip();
            m_equippedItem = null;
            return item;
        }
        
        public void PlayerEquipItem(EquippableItemObject equippedItem) {
            if (m_equippedItem != null) {
                m_equippedItem.Unequip();
            }
            m_equippedItem = equippedItem;
            
            string key = classType.ToString() + equippedItem.ItemType.ToString();
            
        }

        public void PlayerInteract() {
            standardInteractFinder.Interact(ClassType.None, ItemType.None);
        }

        public void PlayerRespawn() {
            ChangeState(StateTypeToIState(defaultState));

            if (m_equippedItem != null && m_equippedItem is EquippableItemObject) {
                ((EquippableItemObject)m_equippedItem).StopInteraction();
            }
            
            gameObject.GetComponent<Respawner>().Respawn();
        }

        public void InterpretToggleInteraction(ClassType _classType, ItemType itemType, string param) {

            if (m_eventDictionary.TryGetValue(_classType.ToString() + itemType.ToString() + param, out ItemReaction reaction)) {
                reaction.OnReaction?.Invoke();
            }
            
        }

        public void Drag(bool enable) {
            if (enable && !(m_currentState is DraggingState)) {
                ChangeState(new DraggingState());
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }
        
        public void Hide(bool enable) {
            if (enable && !(m_currentState is HidingState)) {
                ChangeState(new HidingState());
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }

        public void Action(float actionDuration) {
            ChangeState(new ActionState(actionDuration));
        }

        public void Grapple(bool enable) {
            if (enable && !(m_currentState is GrapplingState)) {
                ChangeState(new GrapplingState());
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
            
        }

        public void Lure(bool enable) {
            if (enable && !(m_currentState is LuringState)) {
                ChangeState(new LuringState(), false);
            }
            else {
                ChangeState(StateTypeToIState(defaultState), false);
            }
        }

        public void Surf(bool enable) {
            if (enable && !(m_currentState is SurfingState)) {
                ChangeState(new SurfingState());
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }

        public void Block(bool enable) {
            if (enable && !(m_currentState is GuardState)) {
                ChangeState(new GuardState(StateType.Blocking));
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }
        
        public void AimBow(bool enable) {
            if (enable && !(m_currentState is GuardState)) {
                ChangeState(new GuardState(StateType.AimingBow));
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }

        public void AimWhip(bool enable) {
            if (enable && !(m_currentState is GuardState)) {
                ChangeState(new GuardState(StateType.AimingWhip));
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }
        
        public void AimGrapple(bool enable) {
            if (enable && !(m_currentState is GuardState)) {
                ChangeState(new GuardState(StateType.AimingGrapple));
            }
            else {
                ChangeState(StateTypeToIState(defaultState));
            }
        }
        
        protected override IEnumerator StateChange(IState newState) {
            
            LockInput = true;
            yield return gridSnap.SnapCoroutine();
            LockInput = false;
            m_currentState.OnExit(this);
            m_currentState = newState;
            m_currentState.OnEnter(this);
       }

        private bool TradeItemsWithPlayer() {
            if (m_equippedItem != null) {
                
                CharacterStateController partnerCharacter = null;

                RaycastHit2D[] hits = Physics2D.CircleCastAll(
                transform.position, 
                gridSnap.CellSize, 
                Vector2.zero,
                gridSnap.CellSize,
                LayerMask.GetMask(new []{"PlayerActors", "HidingPlayerActors"}));

                foreach (var hit in hits) {
                    if (hit.collider.gameObject.TryGetComponent(out CharacterStateController character)) {
                        if (character == this) continue;
                        partnerCharacter = character;
                        break;
                    }
                }

                if (partnerCharacter == null) return false;
                
                var partnerItem = partnerCharacter.DropItem();
                var oldItem = DropItem();
                if (partnerItem != null) partnerItem.Equip(this);
                oldItem.Equip(partnerCharacter);
                return true;
            }

            return false;
        }
    }
}