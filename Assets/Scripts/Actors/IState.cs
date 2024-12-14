namespace Actors {
    public interface IState {
        public void OnEnter(ActorStateController actor);
        public void UpdateState(ActorStateController actor);
        public void OnAction(ActorStateController actor);
        public void OnHurt(ActorStateController actor);
        public void OnExit(ActorStateController actor);
    }
}