using UnityEngine;
using UnityEngine.InputSystem;

namespace Actors {
    [RequireComponent(typeof(CharacterController))]
    public class MoveController : MonoBehaviour {
        
        public float Speed = 4.0f;
        
        private CharacterController m_controller;
        private Vector3 m_playerVelocity;
        
        private Vector2 m_movementInput = Vector2.zero;
        private Vector2 m_lookInput = Vector2.zero;
        
        private void Start() {
            m_controller = gameObject.GetComponent<CharacterController>();
            m_lookInput = gameObject.transform.up;
        }

        public void Stop() {
            m_movementInput = Vector2.zero;
            m_lookInput = Vector2.zero;
        }

        public void Move(Vector2 targetDirection) {
            m_movementInput = targetDirection.normalized;
        }

        public void Look(Vector2 lookDirection) {
            m_lookInput = lookDirection.normalized;
        }
        

        void Update() {

            Vector3 move = new Vector3(m_movementInput.x, m_movementInput.y, 0);
            m_controller.Move(move * Time.deltaTime * Speed);
                
            if (move != Vector3.zero) {
                gameObject.transform.up = move;
            }
            
            HandleLookInput();
        }

        private void HandleLookInput() {

            if (m_lookInput == Vector2.zero && m_movementInput == Vector2.zero) return;
        
            Vector3 look = m_lookInput != Vector2.zero ? m_lookInput : m_movementInput;
            
            float angle = 0f;

            if (Mathf.Abs(look.x) > Mathf.Abs(look.y)) {
                angle = look.x > 0 ? -90f : 90f;
            }
            else {
                angle = look.y > 0 ? 0f : -180f;
            }
            
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        }
    }
}