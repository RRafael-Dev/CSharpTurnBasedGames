using System.Text.RegularExpressions;
namespace Games.Core
{
    public enum GameMatchState { IDLE, STARTED, ENDED };

    public class GameMatch : GameElement
    {
        // events
        public event EventHandler<GameEventArgs>? MatchStart;
        public event EventHandler<GameEventArgs>? MatchEnd;
        public event EventHandler<GamePlayerEventArgs>? PlayerBeginTurn;
        public event EventHandler<GamePlayerEventArgs>? PlayerEndTurn;

        // properties
        private GameTurn? Turn;
        private int LastTurnNumber = 0;
        public override GameMatch? Match { get => this; protected set => throw new GameException("Cannot assign match to a Match object!"); }
        public GameMatchState State { get; private set; } = GameMatchState.IDLE;
        public string? Name { get; private set; }
        public int? TurnNumber { get => Turn?.Number; }
        public GamePlayer? TurnOwner { get => Turn?.Owner; }
        public GamePlayer? NextTurnOwner { get; private set; }
        private readonly List<GamePlayer> players = new List<GamePlayer>();

        private void AssertState(GameMatchState expectedState, string action)
        {
            if (State != expectedState)
                throw new GameException($"Cannot {action} in state {State}!");
        }

        public void Start(string? name = null)
        {
            AssertState(GameMatchState.IDLE, "start match");
            try
            {
                State = GameMatchState.STARTED;
                Name = name;
                OnMatchStart();
                MatchStart?.InvokeSafe(this, new GameEventArgs(this));
            }
            catch (Exception e)
            {
                FinishMatch(e);
            }
        }

        protected virtual void OnMatchStart() {}

        protected void End()
        {
            AssertState(GameMatchState.STARTED, "end match");
            try
            {
                State = GameMatchState.ENDED;
                OnMatchEnd();
                MatchEnd.InvokeSafe(this, new GameEventArgs(this));
            }
            catch (Exception e)
            {
                FinishMatch(e);
            }
        }

        protected virtual void OnMatchEnd() {}

        public void AssertCurrentTurn(GameTurn turn)
        {
            AssertSameMatch(turn);
            if (turn != Turn)
                throw new GameException($"Turn {turn} is illegal!");
        }

        protected virtual int GetNextTurnNumber(GamePlayer player) => LastTurnNumber + 1;

        protected virtual GameTurn? GetNextTurn(GameTurn lastTurn) => null;

        protected virtual GamePlayer? GetNextPlayer(GameTurn lastTurn) => null;

        public void BeginTurn(GamePlayer player) {
            BeginTurn(Turn?.Next(player) ?? new GameTurn(this, GetNextTurnNumber(player), player));
        }

        private void BeginTurn(GameTurn turn)
        {
            AssertState(GameMatchState.STARTED, "end player turn");
            AssertSameMatch(turn);

            var player = turn.Owner;

            if (NextTurnOwner != null && player != NextTurnOwner)
                throw new GameException($"Player {player} cannot begin a turn now!");
            NextTurnOwner = null;

            Turn = turn;
            player.BeginTurn(turn);

            try
            {
                OnBeginTurn(player, turn);
                LastTurnNumber = turn.Number;

                PlayerBeginTurn?.InvokeSafe(this, new GamePlayerEventArgs(this, player));
            }
            catch (Exception e)
            {
                FinishMatch(e);
            }
        }

        protected virtual void OnBeginTurn(GamePlayer player, GameTurn turn) {}

        public void EndTurn(GameTurn turn)
        {
            AssertState(GameMatchState.STARTED, "end player turn");
            AssertSameMatch(turn);

            if (turn != Turn)
                throw new GameException($"Player {turn.Owner} cannot end current turn!");
            if (turn.Active)
                throw new GameException($"Turn {turn} is still active!");

            try
            {
                OnEndTurn(turn.Owner, turn);
                PlayerEndTurn?.InvokeSafe(this, new GamePlayerEventArgs(this, turn.Owner));
            
                if ((Turn = GetNextTurn(turn)) != null)
                    BeginTurn(Turn);
                else
                    NextTurnOwner = GetNextPlayer(turn);
            }
            catch (Exception e)
            {
                FinishMatch(e);
            }
        }

        protected virtual void OnEndTurn(GamePlayer player, GameTurn turn) {}

        private void FinishMatch(Exception e)
        {
            // TODO: FinishMatch
            State = GameMatchState.ENDED;
            MatchEnd?.InvokeSafe(this, new GameEventArgs(this));

            throw e;
        }
    }
}