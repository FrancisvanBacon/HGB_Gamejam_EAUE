using System;
using System.Collections.Generic;
using Actors.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input {
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputRouter : MonoBehaviour {

        [SerializeField] private List<CharacterStateController> characterControllers;
        private CharacterStateController m_currentCharacterController;
        
        private int m_currentIndex;
        private int m_lastIndex;

        [SerializeField] private UnityEvent onCharacterSwitch;

        private void Awake() {
            m_currentCharacterController = characterControllers[0];
            m_currentCharacterController.EnablePlayerControl();
        }

        public void MoveCharacter(InputAction.CallbackContext context) {

            if (m_currentCharacterController == null) return;
            m_currentCharacterController.PlayerMove(context);
        }

        public void LookCharacter(InputAction.CallbackContext context) {
        
            if (m_currentCharacterController == null) return;
            m_currentCharacterController.PlayerLook(context);
        }

        public void UseItem(InputAction.CallbackContext context) {

            if (context.performed && m_currentCharacterController != null) {
                m_currentCharacterController.PlayerUseItem();
            }
        }

        public void DropItem(InputAction.CallbackContext context) {
            
            if (context.performed && m_currentCharacterController != null) {
                m_currentCharacterController.PlayerDropItem();
            }
        }

        public void Interact(InputAction.CallbackContext context) {
            if (context.performed && m_currentCharacterController != null) {
                m_currentCharacterController.PlayerInteract();
            }
        }

        public void SwitchNextCharacter(InputAction.CallbackContext context) {
            if (context.performed) SwitchToNextCharacter();
        }
        
        public void SwitchPrevCharacter(InputAction.CallbackContext context) {
            if (context.performed) SwitchToPrevCharacter();
        }
        
        public void SwitchLastCharacter(InputAction.CallbackContext context) {
            if (context.performed) SwitchToLastCharacter();
        }
        
        public void SwitchToWarrior(InputAction.CallbackContext context) {
            if (context.performed) SwitchByIndex(1);
        }
        
        public void SwitchToJester(InputAction.CallbackContext context) {
            if (context.performed) SwitchByIndex(0);
        }
        
        public void SwitchToHunter(InputAction.CallbackContext context) {
            if (context.performed) SwitchByIndex(2);
        }

#region CharacterSwitching

        private void SwitchToNextCharacter() {
            m_currentCharacterController.DisablePlayerControl();
            m_lastIndex = m_currentIndex;
            m_currentIndex = Math.Abs((m_currentIndex + 1) % characterControllers.Count);
            m_currentCharacterController = characterControllers[m_currentIndex];
            
            if (m_currentCharacterController.IsPlayerControlled) SwitchToNextCharacter();
            
            m_currentCharacterController.EnablePlayerControl();
            
            onCharacterSwitch?.Invoke();
        }
        
        private void SwitchToPrevCharacter() {
            m_currentCharacterController.DisablePlayerControl();
            m_lastIndex = m_currentIndex;

            if (m_currentIndex == 0) {
                m_currentIndex = characterControllers.Count - 1;
            }
            else {
                m_currentIndex -= 1;
            }
            m_currentCharacterController = characterControllers[m_currentIndex];
            
            if (m_currentCharacterController.IsPlayerControlled) SwitchToPrevCharacter();
            
            m_currentCharacterController.EnablePlayerControl();
            
            onCharacterSwitch?.Invoke();
        }
        
        private void SwitchByIndex(int index) {

            if (characterControllers[index].IsPlayerControlled) return;
        
            m_currentCharacterController.DisablePlayerControl();
            m_lastIndex = m_currentIndex;
            m_currentCharacterController = characterControllers[index];
            m_currentIndex = index;
            m_currentCharacterController.EnablePlayerControl();
            
            onCharacterSwitch?.Invoke();
        }

        private void SwitchToLastCharacter() {
        
            if (characterControllers[m_lastIndex].IsPlayerControlled) return;
        
            m_currentCharacterController.DisablePlayerControl();
            int index = m_lastIndex;
            m_lastIndex = m_currentIndex;
            m_currentIndex = index;
            m_currentCharacterController = characterControllers[index];
            m_currentCharacterController.EnablePlayerControl();
            
            onCharacterSwitch?.Invoke();
        }

#endregion
        

    }
}