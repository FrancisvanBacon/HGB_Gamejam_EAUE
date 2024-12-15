using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class StateLever : MonoBehaviour {

        [SerializeField] private List<StateLeverEventWrapper> leverStates;

        private int m_currentState;

        private int m_countFactor = 1;

        public void TriggerLever() {

            if (leverStates.Count > 1) {
                
                leverStates[m_currentState].OnStateExit?.Invoke();
                m_currentState += (1 * m_countFactor);
                
                if (m_currentState >= leverStates.Count) {
                    m_countFactor = -1;
                }
                else {
                    m_countFactor = 1;
                }
                
                leverStates[m_currentState].OnStateEnter?.Invoke();
            }
            
        }
    }

    [Serializable]
    public class StateLeverEventWrapper {
        public UnityEvent OnStateEnter;
        public UnityEvent OnStateExit;
    }
}