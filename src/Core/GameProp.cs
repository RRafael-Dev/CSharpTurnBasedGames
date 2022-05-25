namespace Games.Core
{

    public class GameProp<O> : GameElement where O : GameElement
    {
        private O _owner;
        public virtual O Owner { get => _owner; set => SetOwner(value); }

        public GameProp(O owner) {
            _owner = owner;
        }

        protected virtual void SetOwner(O owner) {
            _owner = owner;
        }
    }
}