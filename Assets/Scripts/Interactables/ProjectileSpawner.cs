using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class ProjectileSpawner : MonoBehaviour {
        public GameObject ProjectilePrefab;

        [SerializeField] private UnityEvent OnShot;

        public virtual void Shoot() {
            if (ProjectilePrefab != null) Instantiate(ProjectilePrefab, transform.position, transform.rotation);
            OnShot?.Invoke();
        }
    }
}