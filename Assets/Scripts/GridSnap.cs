using System.Collections.Generic;
using UnityEngine;

public class GridSnap : MonoBehaviour {
        
    private Grid m_grid;
    private Vector3 m_target;
    private bool m_isSnapped;

    [SerializeField] private bool snapAtStart = true;
        
    private void Start() {
        m_grid = GameObject.FindFirstObjectByType<Grid>();

        if (snapAtStart) {
            StartCoroutine(SnapCoroutine());
        }
    }
        
    public void SnapInstant() {
        if (m_grid == null) return;
        transform.position = m_grid.GetCellCenterWorld(m_grid.WorldToCell(transform.position));
    }
        
    public IEnumerator<bool> SnapCoroutine() {
            
        if (m_isSnapped) yield return true;
            
        m_target = m_grid.GetCellCenterWorld(m_grid.WorldToCell((Vector2)transform.position));

        while (Vector2.Distance((Vector2)transform.position, m_target) > 0.01f) {
                
            transform.position = Vector3.MoveTowards(transform.position, m_target, Time.deltaTime * 10f);

            yield return false;
        }
        m_isSnapped = true;
        yield return true;
    }
}