
using System.Collections.Generic;
using UnityEngine;

namespace Interactables {
    public class Sentry : CooldownProjectileSpawner {

        [SerializeField] private float shootingRange = 100f;
        [SerializeField] private List<string> rayBlockingLayers;
        [SerializeField] private List<string> targetLayers;
        [SerializeField] private List<string> targetTags;

        private int m_layerMask;
        

        private void Start() {
            List<string> layers = new List<string>();
            
            layers.AddRange(rayBlockingLayers);
            layers.AddRange(targetLayers);
            
            m_layerMask = LayerMask.GetMask(layers.ToArray());
            if (targetTags == null) {
                targetTags = new List<string>();
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Vector3 direction = new Vector3(transform.up.x * shootingRange, transform.up.y * shootingRange,
                transform.up.z * shootingRange);
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }

        private void FixedUpdate() {

            RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            transform.up, 
            shootingRange,
            m_layerMask);

            if (hit.collider != null) {

                string layer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                string tag = hit.collider.gameObject.tag;
                
                if (targetLayers.Contains(layer) && (targetTags.Contains(tag) || targetTags.Count == 0)) {
                    Shoot();
                }
                
            }
            
        }

    }
}
