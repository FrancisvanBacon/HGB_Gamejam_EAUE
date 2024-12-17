using UnityEngine;

namespace Actors {
    public class Respawner : MonoBehaviour {

        public Vector3 RespawnPosition;
        
        private void Start() {
            RespawnPosition = transform.position;
        }
        public void Respawn() {
            transform.position = RespawnPosition;
        }

    }
}