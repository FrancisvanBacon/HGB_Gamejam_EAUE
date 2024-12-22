using System.Collections.Generic;
using Actors.Player;
using Items;
using UnityEngine;

namespace Interactables {
    public class InteractableFinder : MonoBehaviour {
        
        [SerializeField] protected FinderType finderType;
        [SerializeField] protected float searchRadius = 5f;
        [SerializeField] private List<string> layers;
        [SerializeField] protected Vector2 boxScale;
        protected IInteractable m_currentInteractable;
        protected int m_layerMask;
        public virtual bool HasInteractable() => m_currentInteractable != null;
        
        private void Start() {
            m_layerMask = LayerMask.GetMask(layers.ToArray());
        }

        protected virtual void FixedUpdate() {

            RaycastHit2D hit = GetRaycastHit2D();
            
            if (hit.collider != null) {
                HandleRaycastHit(hit);
            }
            else if (m_currentInteractable != null) {
                m_currentInteractable.Deselect();
                m_currentInteractable = null;
            }
            
        }

        protected virtual void OnDisable() {
            m_currentInteractable = null;
        }

        public virtual void Interact(ClassType classType, ItemType itemType, string param = "") {
            if (m_currentInteractable != null) {
                m_currentInteractable.Interact(classType, itemType, param);
            }
        }
        
        protected virtual void HandleRaycastHit(RaycastHit2D hit) {
            
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            if (interactable == null && m_currentInteractable != null) {
                m_currentInteractable.Deselect();
                m_currentInteractable = null;
                return;
            }
            
            if (interactable == null || interactable == m_currentInteractable) return;
            
            m_currentInteractable?.Deselect();
            
            m_currentInteractable = interactable;
            m_currentInteractable.Select();
            
        }

        protected virtual RaycastHit2D GetRaycastHit2D() {
            
            switch (finderType) {
                    
                case FinderType.BoxFront:
                    return Physics2D.BoxCast(
                        transform.position + transform.up * searchRadius,
                        boxScale,
                        0,
                        Vector2.zero
                    );
                case FinderType.CircleRadius:
                    return Physics2D.CircleCast(
                        transform.position,
                        searchRadius,
                        Vector2.zero
                    );
                    
                default:
                    return Physics2D.Raycast(transform.position,
                gameObject.transform.up,
                    searchRadius,
                    m_layerMask);
            }
            
        }

        protected virtual void OnDrawGizmos() {

            if (this.enabled == false) return;
            
            Gizmos.color = Color.yellow;


            switch (finderType) {
                case FinderType.RayFront:
                    Gizmos.DrawLine(transform.position, transform.position + transform.up * searchRadius);
                    break;
                case FinderType.BoxFront:
                    Gizmos.DrawWireCube(transform.position + transform.up * searchRadius, boxScale);
                    break;
                case FinderType.CircleRadius:
                    Gizmos.DrawWireSphere(transform.position, searchRadius);
                    break;
            }
            
        }
        
    }

    public enum FinderType {
        RayFront,
        BoxFront,
        CircleRadius,
        None
    }
}