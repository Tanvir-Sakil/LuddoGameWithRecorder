using System;
using System.Collections.Generic;
using System.IO;

namespace LuddoGameCLI
{
    public class CLIGame
    {
        private readonly Game game;
        private readonly Menu menu;
        private readonly ManagePlayer playerManager;
        private MakeRecord recordMaker;
        private RunRecord recordRunner;

        public CLIGame(Game existingGame)
        {
            game = existingGame;
            menu = new Menu();
            playerManager = new ManagePlayer(game);
            recordMaker = new MakeRecord();
            recordRunner = null;
        }

        private T ValidateInput<T>(string prompt, Func<string, T> converter, IEnumerable<T> allowedInput = null, string errorMessage = "Invalid Option!")
        {
            T result;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                try
                {
                    result = converter(input);

                    if (allowedInput == null || allowedInput.Contains(result))
                        break;

                    Console.WriteLine(errorMessage);
                }
                catch
                {
                    Console.WriteLine(errorMessage);
                }
            }

            return result;
        }

        private Stream PromptForFile(string mode)
        {
            Stream fileStream = null;

            while (fileStream == null)
            {
                Console.WriteLine("Enter filename:");
                string fileName = Console.ReadLine();

                try
                {
                    fileStream = mode == "rb" ? File.OpenRead(fileName) : File.Create(fileName);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}. Try again.");
                }
            }

            return fileStream;
        }

        private bool DoesUserWantToSaveGame()
        {
            string text = "Save game?\n0 - No\n1 - Yes";
            return ValidateInput(text, int.Parse, new[] { 0, 1 }) == 1;
        }

        private void RecordPlayers()
        {
            foreach (var player in game.Players)
            {
                recordMaker.AddPlayer(player);
                Console.WriteLine($"Added player {player.Name} to the record.");
            }
        }

        public void PlayGame()
        {
            RecordPlayers(); // Record all players before starting the game.

            while (!game.GameOver)
            {
                game.GamePlay();

                if (!game.GameOver)
                {
                    recordMaker.AddGameTurn(game.RolledValue, game.ActivePlayer?.Index ?? -1);
                    Console.WriteLine($"Recorded turn: Rolled {game.RolledValue} for player {game.ActivePlayer?.Name}.");
                    PrintInfoAfterTurn();
                    PromptToContinue();
                }

                if (DoesUserWantToSaveGame())
                {
                    SaveGame();
                }
            }

            PrintGameResults();
        }

        public void ContinueGame()
        {
            Console.WriteLine("Loading saved game...");
            using (var fileStream = PromptForFile("rb"))
            {
                recordRunner = new RunRecord(fileStream);
            }

            var savedPlayers = recordRunner.GetPlayers();
            var savedDicePositions = recordRunner.GetDicePositions();

            // Convert savedDicePositions (Dictionary<string, (int, int)>) to Dictionary<Dice, (int, int)>
            var dicePositions = new Dictionary<Dice, (int, int)>();

            foreach (var entry in savedDicePositions)
            {
                var dice = Dice.FromString(entry.Key); // Convert string to Dice
                if (dice.HasValue)
                {
                    dicePositions[dice.Value] = entry.Value;
                }
                else
                {
                    Console.WriteLine($"Error: Could not restore Dice from string '{entry.Key}'");
                }
            }

            // Load players and restored dice positions
            game.LoadPlayers(savedPlayers, dicePositions);

            Console.WriteLine("Game state loaded. Replaying saved game history...");

            foreach (var turn in recordRunner.GetGameHistory())
            {
                Console.WriteLine($"Replaying turn: Player index {turn.index}, Rolled value {turn.rolledValue}");
                game.GamePlay(turn.index, turn.rolledValue);
            }

            Console.WriteLine("Game replay completed. Resuming game...");
            PlayGame();
        }



        public void RunRecoredGame()
        {
            Console.WriteLine("Showing saved game...");
            using (var fileStream = PromptForFile("rb"))
            {
                recordRunner = new RunRecord(fileStream);
            }

            var savedPlayers = recordRunner.GetPlayers();
            var savedDicePositions = recordRunner.GetDicePositions();

            // Convert savedDicePositions (Dictionary<string, (int, int)>) to Dictionary<Dice, (int, int)>
            var dicePositions = new Dictionary<Dice, (int, int)>();

            foreach (var entry in savedDicePositions)
            {
                var dice = Dice.FromString(entry.Key); // Convert string to Dice
                if (dice.HasValue)
                {
                    dicePositions[dice.Value] = entry.Value;
                }
                else
                {
                    Console.WriteLine($"Error: Could not restore Dice from string '{entry.Key}'");
                }
            }

            // Load players and restored dice positions
            game.LoadPlayers(savedPlayers, dicePositions);

            Console.WriteLine("Game state loaded. Replaying saved game history...");

            foreach (var turn in recordRunner.GetGameHistory())
            {
                Console.WriteLine($"Replaying turn: Player index {turn.index}, Rolled value {turn.rolledValue}");
                game.GamePlay(turn.index, turn.rolledValue,1);
            }

            //Console.WriteLine("Game replay completed. Resuming game...");
            //PlayGame();
        }

        private void SaveGame()
        {
            Console.WriteLine($"Saving {recordMaker.Players.Count} players and {recordMaker.GameHistory.Count} game turns.");

            // Save dice positions
            var dicePositions = new Dictionary<Dice, (int Common, int Private)>();
            foreach (var player in game.Players)
            {
                foreach (var dice in player.Dices)
                {
                    if (game.HandleDice.DicesPosition.ContainsKey(dice))  // Accessing DicesPosition through the instance of HandleDice
                    {
                        dicePositions[dice] = game.HandleDice.DicesPosition[dice];  // Correctly using game.HandleDice
                    }
                }
            }
            recordMaker.SaveDicePositions(dicePositions); // Ensure dice positions are saved

            using (var fileStream = PromptForFile("wb"))
            {
                recordMaker.Save(fileStream);
            }

            Console.WriteLine("Game saved successfully.");
        }


        private void PrintInfoAfterTurn()
        {
            if (game.ActivePlayer != null)
            {
                Console.WriteLine($"{game.ActivePlayer.Name} moved a pawn.");
                Console.WriteLine("Next turn...");
            }
            else
            {
                Console.WriteLine("Current player information is missing.");
            }
        }

        private void PromptToContinue()
        {
          //  Console.Write("Press Enter to continue...");
          //  Console.ReadLine();
        }

        private void PrintGameResults()
        {
            var gameResults = new GameResults(game.Standing);
            gameResults.PrintResults();
        }
    }
}
