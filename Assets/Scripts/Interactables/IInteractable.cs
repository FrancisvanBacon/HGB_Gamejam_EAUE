﻿using Actors.Player;
using Items;

namespace Interactables {
    public interface IInteractable {
        public void Interact(ClassType classType, ItemType item);
        public void Select();
        public void Deselect();
    }
}