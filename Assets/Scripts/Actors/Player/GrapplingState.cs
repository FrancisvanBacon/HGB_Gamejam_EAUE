using UnityEngine;

namespace Actors.Player {
    public class GrapplingState : IState {
    
        private GridSnap m_gridSnap;
        private CharacterStateController m_characterController;

        private int m_layerMask;
        
        public void OnEnter(ActorStateController actor) {
            m_characterController = actor as CharacterStateController;
            m_gridSnap = actor.transform.parent.GetComponent<GridSnap>();
            
            m_characterController.LockInput = true;
            
            m_characterController.transform.parent.GetComponent<Collider2D>().enabled = false;
            m_gridSnap.SnapToAdjacentCell(actor.gameObject.transform.up * 100f, 5f);

            m_layerMask = LayerMask.GetMask(new string[] { "Interactable", "Walls", "PassthroughWalls" });
            
            m_characterController.Animator.SetBool("IsPullingSelf", true);
        }

        public void FixedUpdateState(ActorStateController actor) {
            
            RaycastHit2D hit = Physics2D.Raycast(actor.gameObject.transform.position,
                actor.gameObject.transform.up,
                m_gridSnap.CellSize / 2,
                m_layerMask);

            if (hit.collider != null) {
                m_characterController.LockInput = false;
                m_characterController.Grapple(false);
            }

        }

        public void OnExit(ActorStateController actor) {
            
            m_characterController.LockInput = false;
            m_characterController.transform.parent.GetComponent<Collider2D>().enabled = true;
            m_gridSnap.SnapToAdjacentCell(Vector3.zero);
            
            m_characterController.Animator.SetBool("IsPullingSelf", false);
        }
    }
}