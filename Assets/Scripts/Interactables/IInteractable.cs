using Actors.Items;

namespace Interactables {
    public interface IInteractable {
        public void Interact(IEquippableItem item);
        public void Select();
        public void Deselect();
    }
}