using UnityEngine;

namespace Actors {
    public class ActorStateController : MonoBehaviour {

        protected IState m_currentState;

        private void Awake() {
            m_currentState = new DefaultState();
        }

        protected virtual void FixedUpdate() {
            m_currentState.FixedUpdateState(this);
        }

        public void ChangeState(IState newState) {
            m_currentState.OnExit(this);
            m_currentState = newState;
            m_currentState.OnEnter(this);
        }

    }
}