using FMOD;

namespace Actors.Player {
    public class LuringState : IState {

        private float m_lastSpeed;
        public void OnEnter(ActorStateController actor) {
            CharacterStateController character = actor as CharacterStateController;
            
            m_lastSpeed = character.Speed;
            character.SetSpeed(2);
        }

        public void FixedUpdateState(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
            CharacterStateController character = actor as CharacterStateController;
            
            character.SetSpeed(m_lastSpeed);
        }
    }
}