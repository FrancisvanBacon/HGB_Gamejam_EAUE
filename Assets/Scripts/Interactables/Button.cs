using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class Button : MonoBehaviour {

        [SerializeField] private UnityEvent onTrigger;
        protected bool m_isTriggered;

        public virtual void Trigger() {
            if (!m_isTriggered) onTrigger?.Invoke();
        }
    }
}
