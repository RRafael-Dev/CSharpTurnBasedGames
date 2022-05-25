namespace Games.Core
{
    public sealed class GameTurn : GameElement
    {
        public int Number { get; private set; }
        public GamePlayer Owner { get; private set; }
        public bool Active {get; private set;} = true;

        public GameTurn(GameMatch gameMatch, int number, GamePlayer turnOwner)
        {
            Match = gameMatch;
            Number = number;
            Owner = turnOwner;
            AssertSameMatch(turnOwner);
        }

        public void End() {
            Active = false;
            RequireMatch().EndTurn(this);
        }

        public GameTurn Next(GamePlayer turnOwner) => new GameTurn(RequireMatch(), Number + 1, turnOwner);
    }
}