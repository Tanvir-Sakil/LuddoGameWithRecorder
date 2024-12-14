using System;
namespace LuddoGameCLI
{
    public class Roll
    {
        private static readonly Random rand = new();
        public const int MIN = 1;
        public const int MAX = 6;
        public static int Throw() => rand.Next(MIN, MAX + 1);
    }
}
