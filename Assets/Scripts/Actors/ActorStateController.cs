using System;
using System.Collections;
using Actors.Enemys;
using Actors.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Actors {
    public class ActorStateController : MonoBehaviour {

        protected IState m_currentState;
        
        [SerializeField] protected GridSnap gridSnap;
        
        [SerializeField] protected StateType defaultState;
        public StateType DefaultState => defaultState;
        
        [SerializeField] protected float speed = 4.0f;
        public float Speed => speed;
        [SerializeField] protected Transform targetTransform;
        [FormerlySerializedAs("onAnyStateExit")] [FormerlySerializedAs("onAnyStateEnter")] [SerializeField] private UnityEvent onAnyStateChange;
        
        private void Awake() {
            m_currentState = StateTypeToIState(defaultState);
            if (gridSnap == null) gridSnap = GetComponent<GridSnap>();
        }

        protected virtual void Start() {
            m_currentState = StateTypeToIState(defaultState);
            ChangeState(m_currentState, false);
        }

        protected virtual void FixedUpdate() {
            m_currentState.FixedUpdateState(this);
        }

        protected void ChangeState(IState newState, bool snap = false) {
            if (snap) {
                StartCoroutine(StateChange(newState));
            }
            else {
                m_currentState.OnExit(this);
                m_currentState = newState;
                onAnyStateChange?.Invoke();
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
            ChangeState(new PushedState(originTransform.transform.up * gridSnap.CellSize));

        }
        
        public void SetDefaultState(StateType state) {
            defaultState = state;
        }
        
        public void SetDefaultState(string state) {
            if (Enum.TryParse<StateType>(state, out StateType stateType)) {
                SetDefaultState(stateType);
            }
        }
        
        protected IState StateTypeToIState(StateType type) {
            
            switch (type) {
                case StateType.Default:
                    break;
                case StateType.Patrol:
                    return gameObject.GetComponent<PatrolState>();
                case StateType.Sentry:
                    return new SentryState();
                case StateType.Lured:
                    return new LuredState(targetTransform);
                case StateType.Blocking:
                    return new GuardState(StateType.Blocking);
                case StateType.AimingBow:
                    return new GuardState(StateType.AimingBow);
                case StateType.AimingWhip:
                    return new GuardState(StateType.AimingWhip);
                case StateType.AimingGrapple:
                    return new GuardState(StateType.AimingGrapple);
            }

            return new DefaultState();
        }
        
        public void ResetState() {
            ChangeState(StateTypeToIState(defaultState));
        }
        
        protected virtual IEnumerator StateChange(IState newState) {
            yield return gridSnap.SnapCoroutine();
            m_currentState.OnExit(this);
            onAnyStateChange?.Invoke();
            m_currentState = newState;
            m_currentState.OnEnter(this);
       }

    }
}