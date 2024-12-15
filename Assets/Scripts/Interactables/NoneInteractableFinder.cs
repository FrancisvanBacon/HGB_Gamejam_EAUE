using Actors.Player;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables {
    public class NoneInteractableFinder : InteractableFinder {
        
        [SerializeField] private UnityEvent<ClassType, ItemType, string> onInteraction;
        
        public override bool HasInteractable() => true;
        
        public override void Interact(ClassType classType, ItemType itemType, string param = "") {
            onInteraction?.Invoke(classType, itemType, param);
        }
        
    }
}