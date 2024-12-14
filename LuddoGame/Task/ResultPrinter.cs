using System;
using System;
using System.Collections.Generic;

namespace LuddoGameCLI
{
    public class GameResults
    {
        private readonly List<Player> standings;

        public GameResults(List<Player> standings)
        {
            this.standings = standings;
        }
        public void PrintResults()
        {
            Console.WriteLine("\nGame Results:");
            string winnerName = string.Empty;

            for (int i = 0; i < standings.Count; i++)
            {
                Player player = standings[i];
                int place = i + 1;
                if (place == 1)
                {
                    winnerName = player.Name;
                    Console.WriteLine($"{player.Name} finished in {place}st place.");
                }
                if (place == 2)
                {
                    Console.WriteLine($"{player.Name} finished in {place}nd place.");
                }
                if (place == 3)
                {
                    Console.WriteLine($"{player.Name} finished in {place}rd place.");
                }
                if (place == 4)
                {
                    Console.WriteLine($"{player.Name} finished in {place}th place.");
                }
            }

            Console.WriteLine($"The winner is {winnerName}. Congratulations!");
        }
    }
}
