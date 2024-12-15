using UnityEngine;

namespace Actors.Player {
    public class DraggingState : IState {

        private CharacterStateController m_character;
        
        public void OnEnter(ActorStateController actor) {
        
            m_character = actor as CharacterStateController;
            
            m_character.MovmentConstraint = CalculateConstraint(actor);
            m_character.SetSpeed(m_character.Speed / 3);
            m_character.LockRotation = true;
        }

        public void FixedUpdateState(ActorStateController actor) {
            
        }

        public void OnAction(ActorStateController actor) {
            
        }

        public void OnHurt(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
            m_character.MovmentConstraint = -1;
            m_character.SetSpeed(m_character.Speed * 3);
            m_character.LockRotation = false;
        }

        private int CalculateConstraint(ActorStateController actor) {
            switch (actor.CurrentDirection) {
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 0;
                case 3:
                    return 1;
            }
            return -1;
        }
    }
}