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
            
            m_characterController.Animator.ResetTrigger("Whip_Stop");
            m_characterController.Animator.SetTrigger("Whip_Start");
        }

        public void FixedUpdateState(ActorStateController actor) {
            var gameObject = actor.gameObject;
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position,
                gameObject.transform.up,
                m_gridSnap.CellSize * 0.1f,
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
            
            m_characterController.Animator.SetTrigger("Whip_Stop");
        }
    }
}