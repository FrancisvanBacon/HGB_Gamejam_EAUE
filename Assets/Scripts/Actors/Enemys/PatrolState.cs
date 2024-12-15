using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors.Enemys {
    public class PatrolState : MonoBehaviour, IState {
    
        [SerializeField] private List<Transform> waypoints;
        private int currentWaypointIndex;

        private bool m_isPatroling;
        
        public void OnEnter(ActorStateController actor) {
            SetIndexToNearestWaypoint();
            m_isPatroling = true;
            StartCoroutine(PatrolRoutine((EnemyStateController)actor));
        }

        public void FixedUpdateState(ActorStateController actor) {
            
        }

        public void OnAction(ActorStateController actor) {
            
        }

        public void OnHurt(ActorStateController actor) {
            
        }

        public void OnExit(ActorStateController actor) {
            ((EnemyStateController)actor).CurrentTarget = this.transform;
            m_isPatroling = false;
            StopCoroutine("PatrolRoutine");
        }

        private IEnumerator PatrolRoutine(EnemyStateController actor) {
            
            actor.CurrentTarget = waypoints[currentWaypointIndex];
            
            while (m_isPatroling) {
                
                yield return actor.MoveToTargetRoutine();
                
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                actor.CurrentTarget = waypoints[currentWaypointIndex];
                
                yield return null;
            }
            
        }

        private void SetIndexToNearestWaypoint() {

            float minDistance = float.MaxValue;
            
            for (int i = 0; i < waypoints.Count; i++) {

                float distance = Vector3.Distance(waypoints[i].position, transform.position);
                
                if (distance < minDistance) {
                    minDistance = distance;
                    currentWaypointIndex = i;
                }
            }
        }
    }
}