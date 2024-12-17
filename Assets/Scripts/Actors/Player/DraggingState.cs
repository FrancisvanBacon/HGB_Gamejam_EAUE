using UnityEngine;

namespace Actors.Player {
    public class DraggingState : IState {

        private CharacterStateController m_character;

        private bool m_isDragging;
        private GridSnap m_gridSnap;
        
        public void OnEnter(ActorStateController actor) {
        
            m_character = actor as CharacterStateController;
            
            m_gridSnap = actor.GetComponent<GridSnap>();
            
            m_gridSnap.SnapToAdjacentCell(Vector3.zero);

            m_character.LockInput = true;
        }

        public void FixedUpdateState(ActorStateController actor) {

            if (m_isDragging) return;

            if (m_gridSnap.IsSnapped) {
                m_isDragging = true;
                m_character.MovmentConstraint = CalculateConstraint(actor);
                m_character.SetSpeed(m_character.Speed / 3);
                m_character.LockInput = false;
                m_character.LockRotation = true;
            }

        }

        public void OnExit(ActorStateController actor) {
            m_character.MovmentConstraint = -1;
            m_character.SetSpeed(m_character.Speed * 3);
            m_character.LockRotation = false;
            m_character.LockInput = false;
        }

        private int CalculateConstraint(ActorStateController actor) {
            switch (m_character.CurrentDirection) {
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 0;
                case 3:
                    return 1;
            }
            return -1;
        }
    }
}