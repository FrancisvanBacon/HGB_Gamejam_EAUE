using GameMode.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

        [SerializeField] private SceneReference targetScene;
                
        public void LoadScene() {
                SceneManager.LoadScene(targetScene.Name);
        }

}