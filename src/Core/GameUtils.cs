namespace Games.Core
{
    public static class GameElementExtensions
    {
        // public static G Require<G>(this object? value, string? msgIfNull = null) where G : GameElement
        // {
        //     if (value == null)
        //         throw new GameException($"Element of type {typeof(G)} was null!");
        //     if (value is not G)
        //         throw new GameException($"Element {value} is not a {typeof(G)}!");
        //     return value as G;
        // }

        // public static GameElement Require(this GameElement? value, string? msgIfNull = null) => value.Require<GameElement>(msgIfNull);
    }

    public static class GameUtilExtensions
    {
        public static G Require<G>(this object? value, string? msgIfNull = null) where G : class
        {
            if (value == null)
                throw new GameException($"Object of required type {typeof(G)} was null!");
            if (value is not G)
                throw new GameException($"Object {value} is not a {typeof(G)}!");
            return (G) value;
        }

        public static GameElement RequireGameElement(this object? value, string? msgIfNull = null) => value.Require<GameElement>(msgIfNull);

        public static object Require(this object? value, string? msgIfNull = null) => value.Require<Object>(msgIfNull);

        public static bool InvokeSafe<A>(this EventHandler<A>? value, object? sender, A args)
        {
            try
            {
                value?.Invoke(sender, args);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return false;
            }
        }

        public static bool InvokeSafe(this EventHandler? value, object? sender, EventArgs args)
        {
            try
            {
                value?.Invoke(sender, args);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return false;
            }
        }
    }
}