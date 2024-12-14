using Actors.Items;
using Actors.Player;

namespace Interactables {
    public interface IInteractable {
        public void Interact(ClassType classType, ItemType item);
        public void Select();
        public void Deselect();
    }
}