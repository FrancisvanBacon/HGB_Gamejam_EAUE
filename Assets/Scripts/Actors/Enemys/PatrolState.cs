using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemys {
    public class PatrolState : MonoBehaviour, IState {
    
        [SerializeField] private List<Transform> waypoints;
        private float m_moveSpeed;
        private int currentWaypointIndex;
        
        public void OnEnter(ActorStateController actor) {
            SetIndexToNearestWaypoint();
            m_moveSpeed = actor.Speed;
        }

        public void FixedUpdateState(ActorStateController actor) {
        
            if (Vector3.Distance(actor.transform.position, waypoints[currentWaypointIndex].position) > 0.01f) {
                actor.transform.position = Vector3.MoveTowards(
                actor.transform.position,
                    waypoints[currentWaypointIndex].position,
                    0.01f * m_moveSpeed);
            }
            else {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
            
        }

        public void OnExit(ActorStateController actor) {
            
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