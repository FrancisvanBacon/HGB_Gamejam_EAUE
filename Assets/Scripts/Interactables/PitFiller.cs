using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class PitFiller : MonoBehaviour {

        [SerializeField] private bool disablePitOnFill;
        public bool DisablePitOnFill => disablePitOnFill;

        [SerializeField] private UnityEvent onPushedInsidePit;

        public void PushInPit() => onPushedInsidePit?.Invoke();

    }
}