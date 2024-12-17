using Actors;
using Actors.Player;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float Speed = 8f;

    private int m_layermask;
    [SerializeField] private ProjectileType projectileType;

    [SerializeField] private float lifeTime = 10f;

    private float m_elapsedTime;

    private void Start() {

        if (projectileType == ProjectileType.Unfriendly) {
            m_layermask = LayerMask.GetMask(new []{"Walls", "PlayerActors", "SurfingPlayerActors", "Interactable"});
        }
        else if (projectileType == ProjectileType.Friendly) {
            m_layermask = LayerMask.GetMask(new []{"Walls", "Interactable"});
        }
        
    }

    private void FixedUpdate() {
        
        m_elapsedTime += Time.fixedDeltaTime;

        if (m_elapsedTime >= lifeTime) {
            Destroy(gameObject);
            return;
        }
    
        transform.position = Vector3.MoveTowards(
                transform.position,
                    transform.up * 10000f,
                    0.01f * Speed);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
        Vector2.up, 
        0.3f, 
        m_layermask);

        if (hit.collider != null) {
            
            HandleTrigger(hit.collider.gameObject);
        }
    }

    private void HandleTrigger(GameObject other) {
        
        if (projectileType == ProjectileType.Unfriendly && other.TryGetComponent(
                out CharacterStateController controller)) {
            controller.PlayerRespawn();
        }
        
        if (projectileType == ProjectileType.Friendly && other.CompareTag("Enemy")) {
            other.gameObject.SetActive(false);
        }
            
        Destroy(this.gameObject);
        
    }

    public enum ProjectileType {
        Unfriendly,
        Friendly
    }
}
