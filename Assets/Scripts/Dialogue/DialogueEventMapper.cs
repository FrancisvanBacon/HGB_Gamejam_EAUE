using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Dialogue {
    public class DialogueEventMapper : MonoBehaviour {

        [SerializeField] private List<KeyedEvent> events;
        private Dictionary<string, UnityEvent> m_eventLookup = new Dictionary<string, UnityEvent>();

        private void Start() {

            foreach (var obj in events) {
                m_eventLookup.Add(obj.Key, obj.Event);
            }
            
        }

        [YarnCommand("InvokeEvent")]
        public void InvokeEvent(string eventKey) {

            if (m_eventLookup.TryGetValue(eventKey, out UnityEvent reaction)) {
                reaction?.Invoke();
            }
            
        }

    }

    [Serializable]
    public class KeyedEvent {
        public string Key;
        public UnityEvent Event;
    }
}