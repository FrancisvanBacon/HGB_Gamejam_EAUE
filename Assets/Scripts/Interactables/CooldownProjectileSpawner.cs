using System.Collections;
using UnityEngine;

namespace Interactables {
    public class CooldownProjectileSpawner : ProjectileSpawner {

        [SerializeField] private float cooldown = 0.5f;
        private IEnumerator coroutine;
        private bool m_cooldownActive;


        public override void Shoot() {
            base.Shoot();
            m_cooldownActive = true;
            if (coroutine != null) {
                coroutine = CooldownRoutine();
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator CooldownRoutine() {
            yield return new WaitForSeconds(cooldown);
            m_cooldownActive = false;
        }
    }
}