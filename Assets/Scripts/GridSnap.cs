using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSnap : MonoBehaviour {
        
    private Grid m_grid;
    public float CellSize => m_grid.cellSize.x;
    private Vector3 m_target;
    public bool IsSnapped;

    [SerializeField] private bool snapAtStart = true;
        
    private void Start() {
        m_grid = GameObject.FindFirstObjectByType<Grid>();

        if (snapAtStart) {
            StartCoroutine(SnapCoroutine());
        }
    }

    public void SnapToAdjacentCell(Vector3 direction, float speed = 2f) {
        StopAllCoroutines();
        
        Vector3Int cell = new Vector3Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));
        
        StartCoroutine(SnapToCellRoutine(cell, speed));
    }
        
    public void SnapInstant() {
        if (m_grid == null) return;
        transform.position = m_grid.GetCellCenterWorld(m_grid.WorldToCell(transform.position));
    }
        
    public IEnumerator<bool> SnapCoroutine(float speed = 2f) {
            
        if (IsSnapped) yield return true;
            
        m_target = m_grid.GetCellCenterWorld(m_grid.WorldToCell((Vector2)transform.position));

        while (Vector2.Distance((Vector2)transform.position, m_target) > 0.01f) {
                
            transform.position = Vector3.MoveTowards(transform.position, m_target, Time.deltaTime * speed);

            yield return false;
        }
        IsSnapped = true;
        yield return true;
    }

    private IEnumerator<bool> SnapToCellRoutine(Vector3Int direction, float speed = 2f) {

        IsSnapped = false;
        
        var cell = m_grid.WorldToCell((Vector2)transform.position);
        cell += direction;
        m_target = m_grid.GetCellCenterWorld(cell);
        
        while (Vector2.Distance((Vector2)transform.position, m_target) > 0.01f) {
                
            transform.position = Vector3.MoveTowards(transform.position, m_target, Time.deltaTime * speed);

            yield return false;
        }

        IsSnapped = true;

        yield return true;

    }
}