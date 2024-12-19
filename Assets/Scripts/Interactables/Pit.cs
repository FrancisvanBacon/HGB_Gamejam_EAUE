using System;
using Actors.Player;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    [RequireComponent(typeof(Collider2D), typeof(GridSnap))]
    public class Pit : MonoBehaviour {

        [SerializeField] private UnityEvent onPitFilled;
        private void OnTriggerEnter2D(Collider2D other) {

            string otherTag = other.tag;

            if (otherTag.Equals("Player")) {
            
                if (other.GetComponentInChildren<CharacterStateController>() != null) {
                    other.GetComponentInChildren<CharacterStateController>().PlayerRespawn();
                    return;
                }
                
            }
            
            if (other.gameObject.TryGetComponent(out PitFiller filler)) {
                filler.PushInPit();
                if (filler.DisablePitOnFill) onPitFilled?.Invoke();
            }
        }
    }
}
