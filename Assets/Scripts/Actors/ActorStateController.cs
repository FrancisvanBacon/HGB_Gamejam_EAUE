using System.Collections;
using Actors.Enemys;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors {
    [RequireComponent(typeof(GridSnap))]
    public class ActorStateController : MonoBehaviour {

        protected IState m_currentState;
        
        protected GridSnap m_gridSnap;
        
        [SerializeField] private StateType beginningState;
        
        [SerializeField] protected float speed = 4.0f;
        public float Speed => speed;
        [SerializeField] protected Transform targetTransform;
        

        [SerializeField] private int m_direction;
        public int CurrentDirection => m_direction;
        
        private void Awake() {
            m_currentState = GetBeginningState();
            m_gridSnap = GetComponent<GridSnap>();
        }

        protected virtual void Start() {
            m_currentState = GetBeginningState();
            ChangeState(m_currentState, false);
        }

        protected virtual void FixedUpdate() {
            m_currentState.FixedUpdateState(this);
            CalculateDirection();
        }

        public void ChangeState(IState newState, bool snap = true) {
            if (snap) {
                StartCoroutine(StateChange(newState));
            }
            else {
                m_currentState.OnExit(this);
                m_currentState = newState;
                m_currentState.OnEnter(this);
            }
        }
        
        public void Lure(Transform _targetTransform) {

            targetTransform = _targetTransform;
            
            if (m_currentState is LuredState) {
                return;
            }
            
            ChangeState(new LuredState(targetTransform));
        }

        public void Push(Transform originTransform) {
            
            if (m_currentState is PushedState) return;
            ChangeState(new PushedState(originTransform.transform.up * m_gridSnap.CellSize));

        }
        
        private void CalculateDirection() {
            int angle = (int)transform.rotation.eulerAngles.z % 360;
            
            switch (angle) {
                case 270:
                    m_direction = 1;
                    break;
                case 180:
                    m_direction = 2;
                    break;
                case 90:
                    m_direction = 3;
                    break;
                case 0:
                    m_direction = 0;
                    break;
            }
        }
        
        private IState GetBeginningState() {
            switch (beginningState) {
                case StateType.Default:
                    break;
                case StateType.Patrol:
                    return gameObject.GetComponent<PatrolState>();
                
                case StateType.Sentry:
                    return new SentryState();
                case StateType.Lured:
                    return new LuredState(targetTransform);
            }

            return new DefaultState();
        }
        
        public void ResetState() {
            ChangeState(GetBeginningState());
        }
        
        protected virtual IEnumerator StateChange(IState newState) {
        
            yield return m_gridSnap.SnapCoroutine();
            m_currentState.OnExit(this);
            m_currentState = newState;
            m_currentState.OnEnter(this);
       }
       
       public enum StateType {
        Default,
        Sentry,
        Patrol,
        Lured
    }

    }
}