
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelState : MonoBehaviour {

    [SerializeField] private UnityEvent onLevelEnter;
    [SerializeField] private UnityEvent onLevelExit;
    private float levelTime = 0f;

    private void Start() {
        onLevelEnter?.Invoke();
    }

    private void Update() {
        
        levelTime += Time.deltaTime;
        
        if (levelTime > float.MaxValue - 2f) {
            SceneManager.LoadScene("MainMenu");
        }
        
    }

    private void OnDestroy() {
        onLevelExit?.Invoke();
    }

}
