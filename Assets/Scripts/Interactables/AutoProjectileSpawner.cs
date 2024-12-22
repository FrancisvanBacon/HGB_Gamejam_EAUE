using System.Collections;
using UnityEngine;

namespace Interactables {
    public class AutoProjectileSpawner : ProjectileSpawner {

        [SerializeField] private float loopPeriodInSeconds = 1f;
        [SerializeField] private bool StartShootingOnStart;
        private IEnumerator coroutine;
        
        private void OnEnable() {
            if (StartShootingOnStart) {
                StartShooting();
            }
        }
        
        private void OnDisable() {
            Stop();
        }
        
        public void StartShooting() {

            if (coroutine != null) return;
            
            coroutine = ShootRoutine();
            
            StartCoroutine(coroutine);
        }

        public void Stop() {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator ShootRoutine() {

            while (true) {
                Shoot();
                yield return new WaitForSeconds(loopPeriodInSeconds);
            }
        }

    }
}