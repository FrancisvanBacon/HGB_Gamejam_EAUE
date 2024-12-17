namespace Actors {
    public interface IState {
        public void OnEnter(ActorStateController actor);
        public void FixedUpdateState(ActorStateController actor);
        public void OnExit(ActorStateController actor);
    }
}