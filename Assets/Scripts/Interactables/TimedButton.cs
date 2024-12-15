using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class TimedButton : Button {

        [SerializeField] private float ResetTimeInSeconds = 3f;

        [SerializeField] private UnityEvent onReset;
    
        private IEnumerator coroutine;
        public override void Trigger() {
        
            base.Trigger();
            coroutine = CountdownRoutine();

            StartCoroutine(coroutine);
        }

        private IEnumerator CountdownRoutine() {
            yield return new WaitForSeconds(ResetTimeInSeconds);
            m_isTriggered = false;
            onReset?.Invoke();
        }
    
    }
}
