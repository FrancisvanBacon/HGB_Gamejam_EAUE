using UnityEngine;

namespace Actors.Enemys {
    public class LuredState : IState {

        private Transform m_targetTransform;
        private float m_lureDistance = 3f;
        private float m_moveSpeed = 2.0f;

        public LuredState(Transform mTargetTransform, float mLureDistance, float mMoveSpeed) {
            m_targetTransform = mTargetTransform;
            m_lureDistance = mLureDistance;
            m_moveSpeed = mMoveSpeed;
        }

        public void OnEnter(ActorStateController actor) {
            
            EnemyStateController enemy = actor as EnemyStateController;

            if (enemy == null) {
                Debug.LogWarning("No EnemyController found");
                return;
            }
        }
        
        public void FixedUpdateState(ActorStateController actor) {

            if (Vector3.Distance(actor.transform.position, m_targetTransform.position) > m_lureDistance) {
                actor.transform.position = Vector3.MoveTowards(
                actor.transform.position,
                    m_targetTransform.position,
                    0.01f * m_moveSpeed);
            }
            
        }

        public void OnAction(ActorStateController actor) {
            
        }

        public void OnHurt(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
            
        }
    }
}