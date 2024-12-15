
using UnityEngine;

namespace Actors.Enemys {
    public class PushedState : IState {

        private GridSnap m_gridSnap;
        private Vector3 m_direction;
        private bool m_isMoving;

        private float m_elapsedTime;
        private const float MAXELAPSEDTIME = 2f;

        public PushedState(Vector3 direction) {
            m_direction = direction;
        }
        public void OnEnter(ActorStateController actor) {
            m_gridSnap = actor.gameObject.GetComponent<GridSnap>();
            m_gridSnap.SnapToAdjacentCell(m_direction);
            m_isMoving = true;
            
            var rigidbody = actor.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void FixedUpdateState(ActorStateController actor) {
            
            m_elapsedTime += Time.fixedDeltaTime;
            
            if ((m_isMoving && m_gridSnap.IsSnapped) || m_elapsedTime > MAXELAPSEDTIME) {
                m_isMoving = false;
                actor.ResetState();
            }
            
        }

        public void OnAction(ActorStateController actor) {
            throw new System.NotImplementedException();
        }

        public void OnHurt(ActorStateController actor) {
            throw new System.NotImplementedException();
        }

        public void OnExit(ActorStateController actor) {
            var rigidbody = actor.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            
        }
    }
}