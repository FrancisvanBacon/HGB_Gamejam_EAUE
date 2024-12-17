using Interactables;
using UnityEngine;

namespace Actors.Enemys {
    public class SentryState : IState {
        public void OnEnter(ActorStateController actor) {
            var sentry = actor.gameObject.GetComponent<Sentry>();
            sentry.enabled = true;
        }

        public void FixedUpdateState(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
            var sentry = actor.gameObject.GetComponent<Sentry>();
            sentry.enabled = false;
        }
    }
}