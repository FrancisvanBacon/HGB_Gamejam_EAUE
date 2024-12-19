using UnityEngine;

namespace Actors {
    public class Respawner : MonoBehaviour {

        public Vector3 RespawnPosition;
        
        private void Start() {
            RespawnPosition = transform.parent.position;
        }
        public void Respawn() {
            
            if (gameObject.TryGetComponent(out ActorStateController controller)) {
                controller.ResetState();
            }
            if (transform.parent.TryGetComponent(out GridSnap gridSnap)) {
                gridSnap.StopAllCoroutines();
            }
            
            transform.parent.position = RespawnPosition;

            
        }

    }
}