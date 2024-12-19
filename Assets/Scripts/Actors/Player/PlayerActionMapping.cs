using System.Collections.Generic;
using Interactables;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Actors.Player {
    public class PlayerActionMapping : MonoBehaviour {
        [SerializeField] private List<ItemReaction> actionReactions;
        private Dictionary<string, UnityEvent> m_eventDictionary = new Dictionary<string, UnityEvent>();

        private void Start() {

            foreach (var itemReaction in actionReactions) {

                m_eventDictionary.TryAdd(itemReaction.ClassType.ToString() + itemReaction.ItemType.ToString(),
                    itemReaction.OnReaction);

            }

        }

        public void InvokeReaction(ClassType classType, ItemType itemType) {

            if (m_eventDictionary.TryGetValue(classType.ToString() + itemType.ToString(), out UnityEvent unityEvent)) {
                unityEvent?.Invoke();
            }

        }
    }
}