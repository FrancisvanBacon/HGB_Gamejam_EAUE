using System.Collections;
using UnityEngine;

namespace Actors.Enemys {
    [RequireComponent(typeof(PatrolState))]
    public class EnemyStateController : ActorStateController {

        [SerializeField] private EnemyStateType beginningState;
        
        private IState m_currentState;
        
        public Transform CurrentTarget;

        [SerializeField] private float moveSpeed = 2.0f;

        public void Start() {
            m_currentState = GetBeginningState();
            ChangeState(m_currentState);
        }

        private IState GetBeginningState() {
            switch (beginningState) {
                case EnemyStateType.Idle:
                    break;
                case EnemyStateType.Patrol:
                    return gameObject.GetComponent<PatrolState>();
                    break;
                case EnemyStateType.Sentry:
                    return gameObject.GetComponent<SentryState>();
                    break;
            }

            return new SentryState();
        }
        
        public void Lure(Transform targetTransform) {

            if (m_currentState is LuredState) {
                return;
            }
            
            ChangeState(new LuredState(targetTransform, 3, moveSpeed / 2f));
        }
        
        public void ResetState() {
            ChangeState(GetBeginningState());
        }
        
        public void MoveToTarget(Transform targetTransform) {
            CurrentTarget = targetTransform;
            StartCoroutine(MoveToTargetRoutine());
        }

        public IEnumerator MoveToTargetRoutine() {
            
            while (Vector3.Distance(transform.position, CurrentTarget.position) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, CurrentTarget.position, 0.005f * moveSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    public enum EnemyStateType {
        Idle,
        Sentry,
        Patrol,
        Lured
    }
}