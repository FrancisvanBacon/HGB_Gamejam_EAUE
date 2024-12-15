using UnityEngine;

namespace Actors.Player {
    public class GrapplingState : IState {
    
        private GridSnap m_gridSnap;
        private bool m_grappling;
        private CharacterStateController m_characterController;

        private int m_layerMask;
        
        public void OnEnter(ActorStateController actor) {
            m_characterController = actor as CharacterStateController;
            m_gridSnap = actor.GetComponent<GridSnap>();
            
            m_characterController.LockInput = true;
            
            m_characterController.GetComponent<Collider2D>().enabled = false;
            m_gridSnap.SnapToAdjacentCell(actor.gameObject.transform.up * 100f, 5f);
            
            m_layerMask = LayerMask.GetMask(new string[] { "Interactable", "Walls", "PassthroughWalls" });
        }

        public void FixedUpdateState(ActorStateController actor) {
            
            RaycastHit2D hit = Physics2D.Raycast(actor.gameObject.transform.position,
                actor.gameObject.transform.up,
                m_gridSnap.CellSize,
                m_layerMask);

            if (hit.collider != null) {
                m_characterController.LockInput = false;
                m_characterController.Grapple(false);
            }

        }

        public void OnAction(ActorStateController actor) {
            
        }

        public void OnHurt(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
        
            m_characterController.LockInput = false;
        
            m_characterController.GetComponent<Collider2D>().enabled = true;
            m_grappling = false;
        }
    }
}