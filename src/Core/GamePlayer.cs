namespace Games.Core
{
    public class GamePlayer : GameElement
    {
        private GameRole? _Role;
        private GameTurn? _Turn;

        public GameRole? Role { get => _Role; private set => SetRole(value); }
        public bool IsInTurn { get => _Turn != null; }

        public void BeginTurn(GameTurn turn)
        {
            AssertSameMatch(turn);
            if (turn.Owner != this)
                throw new GameException($"Player {this} cannot begin turn {turn} as it belongs to another player!");
            RequireMatch().AssertCurrentTurn(turn);

            _Turn = turn;
            OnBeginTurn();
        }

        protected virtual void OnBeginTurn() { }

        protected void EndTurn()
        {
            if (_Turn == null)
                throw new GameException($"Player {this} cannot end current turn!");
            _Turn.End();
            RequireMatch("A Player cannot end its turn outside of a match!").EndTurn(_Turn);
            _Turn = null;
            OnEndTurn();
        }

        protected virtual void OnEndTurn() { }

        protected virtual void SetRole(GameRole? role)
        {
            AssertSameMatch(role ?? (GameElement)this);
            if (_Turn != null)
                throw new GameException($"Player {this} cannot change role mid-turn!");
            _Role = role;
        }
    }
}