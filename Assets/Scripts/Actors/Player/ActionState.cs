using Actors.Enemys;
using UnityEngine;

namespace Actors.Player {
    public class ActionState : IState {

        private float m_duration;
        private float m_timeElapsed;

        public ActionState(float actionDuration) {
            m_duration = actionDuration;
        }
        
        public void OnEnter(ActorStateController actor) {
            var character = actor as CharacterStateController;
            character.LockInput = true;
            character.StopPlayerMovement();
            character.Animator.SetBool("IsWalking", false);
            character.Animator.SetBool("IsAction", true);
        }

        public void FixedUpdateState(ActorStateController actor) {
            m_timeElapsed += Time.fixedDeltaTime;
            
            if (m_timeElapsed >= m_duration) {
                actor.ResetState();
            }
        }

        public void OnExit(ActorStateController actor) {
            var character = actor as CharacterStateController;
            character.StopPlayerMovement();
            character.LockInput = false;
            character.Animator.SetBool("IsAction", false);
        }
    }
}