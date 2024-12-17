using Actors.Player;
using UnityEngine;

namespace Actors.Enemys {

    [RequireComponent(typeof(Collider2D))]
    public class DamageBox : MonoBehaviour{

        private void OnTriggerEnter2D(Collider2D other) {

            if (other.TryGetComponent(out CharacterStateController controller)) {
                controller.PlayerRespawn();
            }

        }

    }
}