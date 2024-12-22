using Actors.Player;
using Items;
using UnityEngine;

namespace Actors.Enemys {
    public class LuredState : IState {

        private Transform m_targetTransform;
        private ItemType m_targetItemtype;
        private float m_lureDistance = 1f;
        private float m_maxDistance = 5f;
        private float m_moveSpeed = 2.3f;

        public LuredState(ItemType item) {
            m_targetItemtype = item;
        }

        public void OnEnter(ActorStateController actor) {
            var rigidbody = actor.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (actor is CharacterStateController) {
                ((CharacterStateController)actor).LockInput = true;
            }

            var box = actor.gameObject.GetComponentInChildren<DamageBox>();

            if (box != null) box.GetComponent<Collider2D>().enabled = false;

            var targetObj = GameObject.FindObjectsByType<EquippableItemObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var obj in targetObj) {
                if (!obj.Type.Equals(m_targetItemtype)) continue;
                m_targetTransform = obj.transform;
                break;
            }
            
        }
        
        public void FixedUpdateState(ActorStateController actor) {

            if (Vector3.Distance(actor.transform.position, m_targetTransform.position) > m_lureDistance) {
                actor.transform.position = Vector3.MoveTowards(
                actor.transform.position,
                    m_targetTransform.position,
                    0.01f * m_moveSpeed);
            }

            if (Vector3.Distance(actor.transform.position, m_targetTransform.position) > m_maxDistance) {
                
                m_targetTransform.gameObject.GetComponentInChildren<EquippableItemObject>().Use();
                actor.ResetState();
            }
            
        }

        public void OnExit(ActorStateController actor) {
            var rigidbody = actor.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            
            if (actor is CharacterStateController) {
                ((CharacterStateController)actor).LockInput = true;
            }
            
            var box = actor.gameObject.GetComponentInChildren<DamageBox>();

            if (box != null) box.GetComponent<Collider2D>().enabled = true;
        }
    }
}