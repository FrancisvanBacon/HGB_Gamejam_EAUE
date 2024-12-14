using UnityEngine;

namespace Actors {
    public class MoveState : IState {

        private MoveController m_controller;

        public void OnEnter(ActorStateController actor) {
            m_controller = actor.gameObject.GetComponent<MoveController>();
            if (m_controller != null) {
                m_controller.enabled = true;
            }
        }
        public void UpdateState(ActorStateController actor) { }

        public void OnAction(ActorStateController actor) {
            
        }

        public void OnHurt(ActorStateController actor) {
        
        }

        public void OnExit(ActorStateController actor) {
            if (m_controller != null) {
                m_controller.enabled = false;
            }
        }
    }
}