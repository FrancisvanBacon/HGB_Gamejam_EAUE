﻿using UnityEngine;

namespace Actors.Player {
    public class ActionState : IState {

        private float m_duration;
        private float m_timeElapsed;

        public ActionState(float actionduration) {
            m_duration = actionduration;
        }
        
        public void OnEnter(ActorStateController actor) {
            var character = actor as CharacterStateController;
            character.LockInput = true;
        }

        public void FixedUpdateState(ActorStateController actor) {
            m_timeElapsed += Time.fixedDeltaTime;

            if (m_timeElapsed >= m_duration) {
                actor.ResetState();
            }
        }

        public void OnAction(ActorStateController actor) {
            throw new System.NotImplementedException();
        }

        public void OnHurt(ActorStateController actor) {
            throw new System.NotImplementedException();
        }

        public void OnExit(ActorStateController actor) {
            var character = actor as CharacterStateController;
            character.LockInput = false;
        }
    }
}