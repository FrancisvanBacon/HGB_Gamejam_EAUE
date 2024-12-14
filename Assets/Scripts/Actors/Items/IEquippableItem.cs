using Actors.Player;

namespace Actors.Items {
    public interface IEquippableItem {
        public void Equipt(ClassType classType);
        public void Unequipt();
        public void Use(ClassType classType);
    }
}