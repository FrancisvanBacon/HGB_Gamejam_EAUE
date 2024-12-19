using UnityEngine;

namespace Actors {

    [RequireComponent(typeof(Collider2D))]
    public class RespawnPlatform : MonoBehaviour {
        
        [SerializeField] private Transform respawnPoint;

        private void Start() {
            if (respawnPoint == null) {
                respawnPoint = transform;
            }
        }        
        
        private void OnTriggerEnter2D(Collider2D collision) {
        
            if (collision.gameObject.GetComponentInChildren<Respawner>() != null) {
                
                collision.gameObject.GetComponentInChildren<Respawner>().RespawnPosition = respawnPoint.position;
            }
            
        }

    }
}