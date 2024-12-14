using Actors.Player;

namespace Actors.Items {
    public interface IEquippableItem {
        public ItemType ItemType { get; set; }
        public void Equipt(CharacterStateController character);
        public void Unequipt();
        public void Use();
    }
}