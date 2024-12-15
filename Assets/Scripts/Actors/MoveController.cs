using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actors {
    [RequireComponent(typeof(Rigidbody2D), typeof(GridSnap))]
    public class MoveController : MonoBehaviour {
        
        [HideInInspector] public float Speed = 4.0f;
        
        private Rigidbody2D m_rigidbody2D;
        private Vector3 m_playerVelocity;
        
        private Vector2 m_movementInput = Vector2.zero;
        private Vector2 m_lookInput = Vector2.zero;

        private GridSnap m_gridSnap;

        public bool LockRotation;
        
        private void Start() {
            m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            m_lookInput = gameObject.transform.up;
            m_gridSnap = GetComponent<GridSnap>();
        }
        
        private void FixedUpdate() {
                
            if (m_movementInput != Vector2.zero) {
                m_rigidbody2D.MovePosition(m_rigidbody2D.position + (m_movementInput * Speed * Time.fixedDeltaTime));
            }
            
            HandleLookInput();
        }

        public IEnumerator Snap() {
            m_movementInput = Vector2.zero;
            yield return m_gridSnap.SnapCoroutine();
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
            
            m_rigidbody2D.SetRotation(angle);
        }
    }
}