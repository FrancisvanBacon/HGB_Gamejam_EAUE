using System;
using System.Collections;
using UnityEngine;

namespace Interactables {
    public class CooldownProjectileSpawner : ProjectileSpawner {

        [SerializeField] private float cooldown = 0.5f;
        private IEnumerator coroutine;
        protected bool m_cooldownActive;

        private void OnEnable() {
            if (coroutine != null) StopCoroutine(coroutine);
            m_cooldownActive = false;
        }

        private void OnDisable() {
            if (coroutine != null) StopCoroutine(coroutine);
            m_cooldownActive = false;
        }

        public override void Shoot() {

            if (m_cooldownActive) return;
        
            base.Shoot();
            m_cooldownActive = true;
            StartCooldown();
        }

        private void StartCooldown() {
            Debug.Log("Starting CD");
            coroutine = CooldownRoutine();
            StartCoroutine(coroutine);
        }

        private IEnumerator CooldownRoutine() {
            yield return new WaitForSeconds(cooldown);
            m_cooldownActive = false;
        }
    }
}