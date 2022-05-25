using System.Text.RegularExpressions;
using Games.Core;

namespace Games
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var match = new GameMatch();
            match.MatchStart += React;
            match.Start();

            System.Console.WriteLine("Phew, that was close!");

            void React(object? sender, GameEventArgs a) {
                throw new Exception("I'm about to end this man's whole career XD");
            }
        }
    }
}