using Actors.Player;
using UnityEngine;

namespace Actors.Enemys {

    [RequireComponent(typeof(Collider2D))]
    public class DamageBox : MonoBehaviour{

        private void OnTriggerEnter2D(Collider2D other) {

            if (other.GetComponentInChildren<CharacterStateController>() != null) {
                other.GetComponentInChildren<CharacterStateController>().PlayerRespawn();
            }

        }

    }
}