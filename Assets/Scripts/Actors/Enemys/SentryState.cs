using Interactables;
using UnityEngine;

namespace Actors.Enemys {
    public class SentryState : IState {

        private Vector3 m_startpos;
        private Quaternion m_startrot;
        private float m_moveSpeed;

        private bool m_isPositioned;

        public SentryState(Vector3 startPos, Quaternion startrot) {
            m_startpos = startPos;
            m_startrot = startrot;
        }
        public void OnEnter(ActorStateController actor) {
            m_moveSpeed = actor.Speed;
            m_isPositioned = false;

            var gridSnap = actor.gameObject.GetComponent<GridSnap>();

            if (gridSnap != null) {
                gridSnap.StopAllCoroutines();
            }
        }

        public void FixedUpdateState(ActorStateController actor) {

            if (m_isPositioned) return;
            
            if (Vector3.Distance(actor.transform.position, m_startpos) > 0.01f) {
                
                m_isPositioned = false;
                
                actor.transform.position = Vector3.MoveTowards(
                actor.transform.position,
                    m_startpos,
                    0.01f * m_moveSpeed);
            }
            else {

                m_isPositioned = true;
                
                var gridSnap = actor.gameObject.GetComponent<GridSnap>();
                gridSnap.SnapInstant();
            
                actor.transform.rotation = m_startrot;
                
                var sentry = actor.gameObject.GetComponent<ProjectileSpawner>();

                if (sentry == null) sentry = actor.gameObject.GetComponentInChildren<ProjectileSpawner>();
                
                sentry.enabled = true;
            }
            
        }

        public void OnExit(ActorStateController actor) {
            var sentry = actor.gameObject.GetComponent<ProjectileSpawner>();
            if (sentry == null) sentry = actor.gameObject.GetComponentInChildren<ProjectileSpawner>();
            sentry.enabled = false;
        }
    }
}