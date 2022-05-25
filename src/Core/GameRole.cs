namespace Games.Core
{
    public class GameRole : GameElement
    {
        public string Name { get; private set; }

        public GameRole(string name) {
            Name = name;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;

        public override bool Equals(object? obj) => obj is GameRole && obj.Require<GameRole>().Name == Name;
    }
}