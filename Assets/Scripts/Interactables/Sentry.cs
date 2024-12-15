using Actors.Player;
using Interactables;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Sentry : ProjectileSpawner {

    protected override void Start() {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.TryGetComponent(out CharacterStateController character)) {
            Shoot();
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        
    }

}
