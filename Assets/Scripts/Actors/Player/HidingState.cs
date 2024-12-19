using UnityEngine;

namespace Actors.Player {
    public class HidingState : IState {

        private float m_lastSpeed;
        private const float HIDINGSPEED = 2;
        public void OnEnter(ActorStateController actor) {
            m_lastSpeed = actor.Speed;
            actor.gameObject.layer = LayerMask.NameToLayer("HidingPlayerActors");

            var character = actor as CharacterStateController;
            
            character.SetSpeed(HIDINGSPEED);
            
            character.Animator.SetBool("IsHiding", true);
        }

        public void FixedUpdateState(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
            var character = actor as CharacterStateController;
            
            character.Animator.SetBool("IsHiding", false);
            
            character.SetSpeed(m_lastSpeed);
            actor.gameObject.layer = LayerMask.NameToLayer("PlayerActors");
        }
    }
}