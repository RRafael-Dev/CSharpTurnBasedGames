namespace Games.Core
{
    public class GameEventArgs : EventArgs
    {
        public GameMatch Match { get; private set; }

        public GameEventArgs(GameMatch match)
        {
            this.Match = match;
        }
    }

    public class GamePlayerEventArgs : GameEventArgs
    {
        public GamePlayer Player { get; private set; }

        public GamePlayerEventArgs(GameMatch match, GamePlayer player) : base(match)
        {
            this.Player = player;
        }
    }
}