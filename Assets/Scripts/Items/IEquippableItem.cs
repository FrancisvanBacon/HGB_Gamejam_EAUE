using Actors.Player;

namespace Items {
    public interface IEquippableItem {
        public ItemType ItemType { get; set; }
        public void Equip(CharacterStateController character);
        public void Unequip();
        public void Use();
    }
}