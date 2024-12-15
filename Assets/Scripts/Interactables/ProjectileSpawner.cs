using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    [RequireComponent(typeof(GridSnap))]
    public class ProjectileSpawner : MonoBehaviour {
        public GameObject ProjectilePrefab;

        [SerializeField] private UnityEvent OnShot;
        
        private GridSnap m_gridSnap;

        protected virtual void Start() {
            m_gridSnap = GetComponent<GridSnap>();
        }

        public virtual void Shoot() {
            Instantiate(ProjectilePrefab, transform.position, transform.rotation);
            OnShot?.Invoke();
        }
    }
}