using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables {
    public class AutoProjectileSpawner : ProjectileSpawner {

        [SerializeField] private float loopPeriodInSeconds = 1f;
        [SerializeField] private bool StartShootingOnStart;
        private IEnumerator coroutine;

        private void Start() {
            coroutine = ShootRoutine();
            
            StartCoroutine(coroutine);
        }
        
        public void StartShooting() {

            if (coroutine != null) return;
            
            coroutine = ShootRoutine();
            
            StartCoroutine(coroutine);
        }

        public void Stop() {
            if (coroutine != null) StopCoroutine(coroutine);
        }

        private IEnumerator ShootRoutine() {

            while (true) {
                Shoot();
                yield return new WaitForSeconds(loopPeriodInSeconds);
            }
        }

    }
}