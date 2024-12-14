using UnityEngine;
using UnityEngine.InputSystem;

namespace Actors {
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveController : MonoBehaviour {
        
        public float Speed = 4.0f;
        
        private Rigidbody2D m_rigidbody2D;
        private Vector3 m_playerVelocity;
        
        private Vector2 m_movementInput = Vector2.zero;
        private Vector2 m_lookInput = Vector2.zero;
        
        private void Start() {
            m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
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
        
        void FixedUpdate() {
            Vector3 move = new Vector3(m_movementInput.x, m_movementInput.y, 0);
            
            m_rigidbody2D.MovePosition(m_rigidbody2D.position + (m_movementInput * Speed * Time.fixedDeltaTime));
            
            HandleLookInput();
            if (move != Vector3.zero) {
                gameObject.transform.up = move;
            }
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