using Actors.Enemys;
using UnityEngine;

namespace Actors.Player {
    public class GuardState : IState {
        private CharacterStateController m_character;

        private GuardObject m_guardingObject;

        private StateType m_assignedState;

        private bool m_lockState;
        private bool m_guarding;

        private const float BLOCKINGMOVESPEED = 2f;

        private float m_lastSpeed;

        public GuardState(StateType assignedState) {
            m_assignedState = assignedState;
        }
        public void OnEnter(ActorStateController actor) {
            m_character = actor as CharacterStateController;
            m_lastSpeed = m_character.Speed;
            
            m_character.SetDefaultState(m_assignedState);
            
            GuardObject[] objects = GameObject.FindObjectsByType<GuardObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (GuardObject obj in objects) {
                if (obj.IsAssignedItem(m_assignedState)) {
                    m_guardingObject = obj;
                    break;
                }
            }

        }

        public void FixedUpdateState(ActorStateController actor) {
        
            if (!m_character.IsPlayerControlled) {
                if (m_guarding) {
                    m_guardingObject.OnAutoGuard?.Invoke();
                }
                m_lockState = true;
                return;
            }
            
            if (m_lockState && (m_character.MoveInput != Vector2.zero || m_character.LookInput != Vector2.zero)) {
                m_lockState = false;
                m_guardingObject.OnAutoGuardEnd?.Invoke();
                m_guardingObject.OnGuardEnd?.Invoke();
                m_character.SetSpeed(m_lastSpeed);
                m_guarding = false;
            }
            
            if (m_character.LookInput != Vector2.zero) {
                m_lockState = false;
                m_guardingObject.OnGuard?.Invoke();
                m_character.SetSpeed(BLOCKINGMOVESPEED);
                m_guarding = true;
                m_character.Animator.SetBool("IsGuardState", true);
            }
            else if (!m_lockState && m_character.MoveInput != Vector2.zero) {
                m_guardingObject.OnAutoGuardEnd?.Invoke();
                m_guardingObject.OnGuardEnd?.Invoke();
                m_character.SetSpeed(m_lastSpeed);
                m_guarding = false;
                m_character.Animator.SetBool("IsGuardState", false);
            }
            
        }

        public void OnExit(ActorStateController actor) {
            m_guardingObject.OnAutoGuardEnd?.Invoke();
            m_guardingObject.OnGuardEnd?.Invoke();
            m_character.SetSpeed(m_lastSpeed);
            m_guarding = false;
            Debug.Log("Guard state exited");
            m_character.Animator.SetBool("IsGuardState", false);
        }
    }
}