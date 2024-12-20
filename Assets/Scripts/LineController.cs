using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour {

    [SerializeField] private List<string> obstructingLayers;

    [SerializeField] private float lineMaxLength = 5f;
    
    [SerializeField] private LineRenderer lineRenderer;

    private int m_layermask;

    private void Start() {
            
        m_layermask = LayerMask.GetMask(obstructingLayers.ToArray());
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
    }
    private void FixedUpdate() {

        if (lineRenderer == null || lineRenderer.enabled == false) return;
        
        lineRenderer.SetPosition(0, transform.localPosition);
            
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, lineMaxLength, m_layermask);

        float distance = lineMaxLength;
        
        if (hit.collider != null) {
            distance = Vector2.Distance(transform.position, hit.point);
        }
        
        lineRenderer.SetPosition(1, transform.localPosition + Vector3.up * distance);

    }
}