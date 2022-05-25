namespace Games.Core
{
    public class GameElement
    {
        public virtual GameMatch? Match { get; protected set; }

        public GameMatch RequireMatch(string? msgIfNull = null)
        {
            AssertMatchAssigned(msgIfNull);
            return Match.Require<GameMatch>();
        }

        public void AssertMatchAssigned(string? msgIfNull = null)
        {
            if (Match == null)
                throw new GameException(msgIfNull ?? $"Element {this} has no match assigned yet!");
        }

        public void AssertSameMatch(GameElement e)
        {
            AssertMatchAssigned();
            e.AssertMatchAssigned();

            if (Match != e.Match)
                throw new GameException($"Elements {this} and {e} belong to different matches!");
        }
    }

    public class GameElementWithProps<GE> : GameElement where GE : GameElement
    {
        protected List<GameProp<GameElement>> Props { get; private set; } = new List<GameProp<GameElement>>();
        public virtual void AddProp(GameProp<GameElement> prop)
        {
            throw new GameException("Props can only be added by the element itself!");
        }

        public virtual void RemoveProp(GameProp<GameElement> prop)
        {
            throw new GameException("Props can only be removed by the element itself!");
        }
    }
}