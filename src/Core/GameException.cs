namespace Games.Core
{
    public class GameException : ApplicationException
    {
        public GameException(string message) : base(message) { }

        public GameException(string message, Exception innerException) : base(message, innerException) { }
    }
}