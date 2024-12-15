using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class Lever : MonoBehaviour {

        [SerializeField] private UnityEvent OnLeverLeft;
        [SerializeField] private UnityEvent OnLeverRight;

        private bool m_isright;
    
        public void ToggleLever() {
        
            if (m_isright) OnLeverRight.Invoke();
            if (!m_isright) OnLeverLeft.Invoke();
        
            m_isright = !m_isright;
        }

    }
}
