using UnityEngine;

namespace Actors {
    public class Respawner : MonoBehaviour {

        public Vector3 RespawnPosition;
        
        private void Start() {
            RespawnPosition = transform.position;
        }
        public void Respawn() {
            
            if (gameObject.TryGetComponent(out ActorStateController controller)) {
                controller.ResetState();
            }
            if (gameObject.TryGetComponent(out GridSnap gridSnap)) {
                gridSnap.StopAllCoroutines();
            }
        
            transform.position = RespawnPosition;

            
        }

    }
}