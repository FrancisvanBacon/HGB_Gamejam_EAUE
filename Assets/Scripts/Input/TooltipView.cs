using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class TooltipView : MonoBehaviour {

        public bool showTooltips = true;
        private PlayerInputRouter m_currentInputDevice;
        private TooltipViewConfig m_config;
        private SpriteRenderer m_spriteRenderer;
        private Animator m_animator;

        private InputType m_currentInputType;

        private void OnEnable() {
            m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            m_animator = gameObject.GetComponent<Animator>();
            m_currentInputDevice = GameObject.FindFirstObjectByType<PlayerInputRouter>();
            m_spriteRenderer.enabled = false;
            m_animator.enabled = false;
            m_config = Resources.Load<TooltipViewConfig>("TooltipSettings");

            var playerInput = GameObject.FindFirstObjectByType<PlayerInput>();
            if (playerInput != null) playerInput.controlsChangedEvent.AddListener(OnInputSwitch);
        }

        private void OnDisable() {
            var playerInput = GameObject.FindFirstObjectByType<PlayerInput>();
            if (playerInput != null) playerInput.controlsChangedEvent.RemoveListener(OnInputSwitch);
        }

        private void OnInputSwitch(PlayerInput input) {

            if (m_currentInputType != InputType.None) {
                ShowTooltip(m_currentInputType);
            }
        }

        public void ShowInteractTooltip() {
            ShowTooltip(InputType.Interact);
        }

        public void ShowUseItemTooltip() {
            ShowTooltip(InputType.UseItem);
        }

        public void ShowMoveTooltip() {
            ShowTooltip(InputType.Move);
        }

        public void ShowLookTooltip() {
            ShowTooltip(InputType.Look);
        }

        public void ShowSwitchCharacterTooltip() {
            ShowTooltip(InputType.SwitchCharacter);
        }
        
        public void ShowDropItemTooltip() {
            ShowTooltip(InputType.DropItem);
        }

        private void ShowTooltip(InputType inputType) {

            if (!showTooltips) return;

            if (inputType == InputType.Interact) {
                inputType = CheckForDistantInteraction();
            }

            var sprite = GetSpriteByType(inputType);

            if (sprite != null) {
                m_spriteRenderer.sprite = sprite;
                m_spriteRenderer.enabled = true;
                m_animator.enabled = true;

                m_currentInputType = inputType;
            }

        }

        public void HideTooltip() {
            m_spriteRenderer.enabled = false;
            m_animator.enabled = false;
            m_currentInputType = InputType.None;
        }

        private InputType CheckForDistantInteraction() {

            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero);
            InputType inputType = InputType.UseItem;

            foreach (var hit in hits) {
                if (hit.collider == null) {
                    inputType = InputType.UseItem;
                }

                if (hit.collider.gameObject.CompareTag("Player")) {
                    return InputType.Interact;
                }
            }

            return inputType;
        }

        private Sprite GetSpriteByType(InputType type) {

            var list = m_config.Bindings;

            Sprite sprite = null;
            
            foreach (var obj in list) {
                if (obj.InputType.Equals(type)) {
                    switch (m_currentInputDevice.CurrentControlScheme) {
                
                        case "Keyboard":
                            return obj.KeyboardSprite;
                        case "Gamepad":
                            return obj.XBoxControllerSprite;
                    }
                    
                }
            }

            return sprite;
        }

    }
}