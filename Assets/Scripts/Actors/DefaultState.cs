using UnityEngine;

namespace Actors {
    public class DefaultState : IState {
        
        public void OnEnter(ActorStateController actor) {
            
        }
        public void FixedUpdateState(ActorStateController actor) { }

        public void OnAction(ActorStateController actor) {
            
        }

        public void OnHurt(ActorStateController actor) {
        
        }

        public void OnExit(ActorStateController actor) {
            
        }
    }
}