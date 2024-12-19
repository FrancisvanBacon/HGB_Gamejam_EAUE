using UnityEngine;
using Debug = FMOD.Debug;

namespace Actors.Player {
    public class SurfingState : IState {
    
        private GridSnap m_gridSnap;
        private bool m_surfing;
        private CharacterStateController m_characterController;
        private Collider2D m_collider;
        public void OnEnter(ActorStateController actor) {
            m_characterController = actor as CharacterStateController;
            m_gridSnap = actor.transform.parent.GetComponent<GridSnap>();
            
            m_characterController.LockInput = true;
            
            m_gridSnap.SnapToAdjacentCell(actor.gameObject.transform.up);
            m_characterController.transform.parent.GetComponent<Collider2D>().enabled = false;
            
            m_characterController.Animator.SetBool("IsSurfing", true);

            m_collider = actor.transform.parent.GetComponent<Collider2D>();
        }

        public void FixedUpdateState(ActorStateController actor) {

            if (!m_surfing && m_gridSnap.IsSnapped) {
                m_characterController.SetSpeed(m_characterController.Speed * 2);
                m_characterController.gameObject.layer = LayerMask.NameToLayer("SurfingPlayerActors");
                m_collider.enabled = true;
                m_surfing = true;
                
            } else if (m_surfing) {
                
                m_characterController.LockInput = false;
                
                RaycastHit2D hit = Physics2D.Raycast(actor.gameObject.transform.position, 
                actor.gameObject.transform.forward, 
                Mathf.Infinity, 
                LayerMask.GetMask("Pier"));

                if (hit.collider != null) {
                    m_characterController.Surf(false);
                }
                
            }
        }
        
        public void OnExit(ActorStateController actor) {
            CharacterStateController character = actor as CharacterStateController;
            
            m_characterController.LockInput = false;
            
            character.SetSpeed(character.Speed / 2);
            character.gameObject.layer = LayerMask.NameToLayer("PlayerActors");
            
            m_characterController.Animator.SetBool("IsSurfing", false);   
        }
    }
}