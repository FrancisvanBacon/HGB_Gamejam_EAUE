using System.Collections.Generic;
using Actors.Enemys;
using UnityEngine;
using UnityEngine.Events;

namespace Actors {
    public class GuardObject : MonoBehaviour {

        [SerializeField] private List<StateType> assignedStateTypes;

        public UnityEvent OnGuard;
        public UnityEvent OnAutoGuard;
        public UnityEvent OnAutoGuardEnd;
        public UnityEvent OnGuardEnd;
        
        public bool IsAssignedItem(StateType type) => assignedStateTypes.Contains(type);
    }
}