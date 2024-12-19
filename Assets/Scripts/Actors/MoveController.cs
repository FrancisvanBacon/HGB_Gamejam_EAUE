using System.Collections;
using UnityEngine;

namespace Actors {
    
    public class MoveController : MonoBehaviour {
        
        [HideInInspector] public float Speed = 4.0f;
        
        [SerializeField] private Rigidbody2D rigidBody2D;
        private Vector3 m_playerVelocity;
        
        private Vector2 m_movementInput = Vector2.zero;
        private Vector2 m_lookInput = Vector2.zero;

        public Vector2 LookInput => m_lookInput;
        public Vector2 MovementInput => m_movementInput;

        [SerializeField] private GridSnap gridSnap;

        [HideInInspector] public bool LockRotation;
        
        private void Start() {
            if (rigidBody2D == null) rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
            m_lookInput = gameObject.transform.up;
            if (gridSnap == null) gridSnap = GetComponent<GridSnap>();
        }
        
        private void FixedUpdate() {
                
            if (m_movementInput != Vector2.zero) {
                rigidBody2D.MovePosition(rigidBody2D.position + (m_movementInput * Speed * Time.fixedDeltaTime));
            }
            
            HandleLookInput();
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
        
        public int GetDirection() {
            int angle = (int)transform.rotation.eulerAngles.z % 360;
            
            switch (angle) {
                case 270:
                    return 1;
                case 180:
                    return 2; ;
                case 90:
                    return 3;
                default:
                    return 0;
            }
        }

        private void HandleLookInput() {

            if (LockRotation) return;

            if (m_movementInput == Vector2.zero && m_lookInput == Vector2.zero) {
                return;
            };
            
            Vector3 look = m_lookInput != Vector2.zero ? m_lookInput : m_movementInput;
            
            float angle = 0f;

            if (Mathf.Abs(look.x) > Mathf.Abs(look.y)) {
                angle = look.x > 0 ? -90f : 90f;
            }
            else {
                angle = look.y > 0 ? 0f : -180f;
            }
            
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}