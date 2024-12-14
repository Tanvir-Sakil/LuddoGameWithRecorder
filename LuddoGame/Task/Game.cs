using System;
using System.Collections.Generic;
using System.Numerics;


namespace LuddoGameCLI
{
    public class Game
    {
        public DiePresenter dicePresenter = new DiePresenter();
        public HandleDice HandleDice = new();
        public Queue<Player> Players = new();
        public List<Dice> MovableDices { get; private set; } = new();
        private List<Dice> OverlappedDices = new();
        //private int RolledValue;
        public int RolledValue { get; private set; }//chnage

        public Player? ActivePlayer { get; private set; }
        private Dice? PickedDice = null;
        public bool GameOver { get; private set; }
        public List<Player> Standing = new();
        int StartOrNot = 1;

        public Game()
        {
            GameOver = false;
        }

        public List<string> RemainingColors()
        {
            HashSet<string> assignedColors = new();

            foreach (var activePlayer in Players)
            {
                assignedColors.Add(activePlayer.Colour);
            }

            List<string> allColors = new() { "yellow", "blue", "red", "green" };
            allColors.RemoveAll(color => assignedColors.Contains(color));

            return allColors;
        }
        /// <summary>
        /// new added
        /// Loads players into the game from a saved state.
        /// </summary>
        /// <param name="players">The list of players to load into the game.</param>
        public void LoadPlayers(IEnumerable<Player> players, Dictionary<Dice, (int Common, int Private)> savedPositions)
        {
            Players.Clear();
            HandleDice.DicesPosition.Clear(); 

            int index = 0;
            foreach (var player in players)
            {
                player.Index = index++;
                Players.Enqueue(player);

          
                foreach (var dice in player.Dices)
                {
                    if (savedPositions != null && savedPositions.ContainsKey(dice))
                    {
                        HandleDice.DicesPosition[dice] = savedPositions[dice];
                    }
                    else
                    {
                        HandleDice.DicesPosition[dice] = HandleDice.BoardPoolPosition; 
                    }
                }
            }

            Console.WriteLine("Players and dice positions loaded successfully.");
        }



        public void GamePlay(int? ind = null, int? rolledVal = null, int showRecord = 0)
        {
            if (showRecord == 0)
            {
                Console.Write(StartOrNot == 1 ? "Press Enter to start playing " : "Press Enter to continue playing ");
                StartOrNot = 0;
                Console.ReadLine();
                OverlappedDices.Clear();
                ActivePlayer = Players.Peek();
                RolledValue = rolledVal ?? Roll.Throw();
                MovableDices = MovableDice(ActivePlayer, RolledValue);

                Console.WriteLine($"{ActivePlayer.Name} rolled a {RolledValue}.");
                dicePresenter.PresentDieWithPlayerName(RolledValue, ActivePlayer.Name);

                if (MovableDices.Count > 0)
                {
                    int index = ind ?? ActivePlayer.ChooseDice(MovableDices);
                    if (index >= 0 && index < MovableDices.Count)
                    {
                        PickedDice = MovableDices[index];
                        NextTurn(ActivePlayer, PickedDice.Value);
                    }
                }
                else
                {
                    Console.WriteLine($"{ActivePlayer.Name} has no dices to move.");
                    PickedDice = null;
                }
                HandleDice.BoardDisplay();



                if (!GameOver)
                {

                    if (RolledValue != Roll.MAX)
                        Players.Enqueue(Players.Dequeue());
                }
                else
                {
                    Console.WriteLine("Game is Over!!!");
                }
            }
            else
            {
                HandleDice.BoardDisplay();
            }
        }



        private void NextTurn(Player player, Dice dice)
        {
            try
            {
                if (RolledValue == Roll.MAX && HandleDice.IsDieInPool(dice))
                {
                    Console.WriteLine($"Dice {dice.Id} place on starting point.");
                    HandleDice.PlaceDieAtStart(dice, false);
                    HandleOverlapping(dice);

                }

                else
                {
                    Console.WriteLine("Moving dice on board...");
                    HandleDice.MoveDice(dice, RolledValue);
                }

                if (HandleDice.IsFinsihedMove(dice))
                {
                    player.Dices.Remove(dice);
                    Console.WriteLine($"{player.Name} has finished dice {dice.Id}!");
                    if (player.Dices.Count == 0)
                    {
                        Standing.Add(player);
                        Players.Dequeue();
                        if (Players.Count == 1)
                        {
                            Standing.Add(Players.Dequeue());
                            GameOver = true;
                        }
                    }
                    return;
                }

                HandleOverlapping(dice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving dice {dice.Id}: {ex.Message}");
                throw;
            }
        }

        private List<Dice> MovableDice(Player currentPlayer, int diceRollValue)
        {
            List<Dice> movableDice = new();
            if (diceRollValue == Roll.MAX)
            {
                var poolDice = DiceFromPool(currentPlayer);
                if (poolDice.HasValue)
                    movableDice.Add(poolDice.Value);
            }
            foreach (var die in currentPlayer.Dices)
            {
                bool isInPool = HandleDice.IsDieInPool(die);
                bool canMove = HandleDice.IsDieMovementAllowed(die, diceRollValue);

                if (!isInPool && canMove)
                    movableDice.Add(die);
            }

            return movableDice;
        }

        private Dice? DiceFromPool(Player player)
        {
            foreach (var dice in player.Dices)
            {
                if (HandleDice.IsDieInPool(dice))
                    return dice;
            }
            return null;
        }

        private void HandleOverlapping(Dice dice)
        {
            OverlapHandling overlapHandling = new OverlapHandling(HandleDice);
            overlapHandling.HandleOverlapping(dice);
        }
    }
}
