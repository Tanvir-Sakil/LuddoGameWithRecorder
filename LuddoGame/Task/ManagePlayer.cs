using System;
using System.Collections.Generic;

namespace LuddoGameCLI
{
    public class ManagePlayer
    {
        private Game game;

        public ManagePlayer(Game gameInstance)
        {
            game = gameInstance;
        }

        private void PlayerInfo(int playerNumber)
        {
            List<string> availableColours = game.RemainingColors();
            int playerType;
            do
            {
                Console.WriteLine("Choose type of player:\n0 - Computer\n1 - Human");
            } while (!int.TryParse(Console.ReadLine(), out playerType) || (playerType != 0 && playerType != 1));

            bool isHuman = playerType == 1;

            string playerName;
            if (isHuman)
            {
                Console.WriteLine("Enter player name:");
                playerName = Console.ReadLine() ?? "Player";
            }
            else
            { 
                playerName = $"Computer-{playerNumber}";
            }

            Console.WriteLine("Choose player colour:");
            for (int i = 0; i < availableColours.Count; i++)
                Console.WriteLine($"{i} - {availableColours[i]}");

            if (!int.TryParse(Console.ReadLine(), out int colourIndex) || colourIndex < 0 || colourIndex >= availableColours.Count)
            {
                Console.WriteLine("Invalid colour selection. Defaulting to first available colour.");
                colourIndex = 0;
            }

            string playerColour = availableColours[colourIndex];

            AddPlayer(new Player(playerColour, isHuman, playerName));
        }


        public void AddPlayer(Player player)
        {

            game.Players.Enqueue(player);
            int count = 0;
            bool shouldPrint = false;
            foreach (var dice in player.Dices)
            {
                game.HandleDice.MoveDieToPool(dice, shouldPrint);
                count++;
                if (count == 3)
                {
                    shouldPrint = true;
                }
            }
        }
        public void InitializePlayer()
        {
            string[] counts = { "first", "second", "third", "fourth last" };
            string textAdd = "Add {0} player";
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine(string.Format(textAdd, counts[i]));
                PlayerInfo(i+1);
                Console.WriteLine("Player added");
            }
            for (int i = 2; i < 4; i++)
            {
                Console.WriteLine($"Choose option:\n0 - add player\n1 - start game with {i} players");
                int choice = ValidateInput(0, 1);

                if (choice == 1)
                {
                    break;
                }
                else if (choice == 0)
                {
                    Console.WriteLine(string.Format(textAdd, counts[i]));
                    PlayerInfo(i+1);
                    Console.WriteLine("Player added");
                }
            }
        }

        private int ValidateInput(int minValue, int maxValue)
        {
            int input;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input) && input >= minValue && input <= maxValue)
                {
                    return input;
                }
                Console.WriteLine($"Invalid input. Please enter a number between {minValue} and {maxValue}.");
            }
        }
    }
}

