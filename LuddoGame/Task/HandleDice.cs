using System;
using System.Collections.Generic;
namespace LuddoGameCLI
{
    public class HandleDice
    {
        private BuildBoard LuddoBoard;
        private const int BOARD_SIZE = 56;
        private const int BOARD_COLOUR_SIZE = 7;

        private static readonly Dictionary<string, int> StartingCell = new()
        {
            { "yellow", 1 },
            { "blue", 15 },
            { "red", 29 },
            { "green", 43 }
        };

        public Dictionary<Dice, (int Common, int Private)> DicesPosition = new();
        public (int, int) BoardPoolPosition = (-1, -1);

        public HandleDice()
        {
           
            LuddoBoard = new BuildBoard();
        }


        public void RestoreDicePositions(Dictionary<string, (int Common, int Private)> savedPositions)
        {
            foreach (var entry in savedPositions)
            {
                // Convert the string key back to a Dice object using Dice.FromString()
                var dice = Dice.FromString(entry.Key); // This will return Dice?
                if (dice.HasValue) // Check if dice is not null
                {
                    // Restore the position for the dice in the DicesPosition dictionary
                    DicesPosition[dice.Value] = entry.Value;
                }
                else
                {
                    Console.WriteLine($"Error: Could not restore Dice from string '{entry.Key}'");
                }
            }
            Console.WriteLine("Dice positions restored successfully.");
        }





        public void UpdateDicePosition(Dice die, (int, int) newPosition, bool displayAfterUpdate, bool fromPool)
        {
            DicesPosition[die] = newPosition;
            if (displayAfterUpdate)
            {
                LuddoBoard.BoardDisplay(DicesPosition, true);
            }
        }

        public void MoveDieToPool(Dice die, bool displayAfterMove)
        {
            UpdateDicePosition(die, BoardPoolPosition, displayAfterMove, true);
        }

        public bool IsDieInPool(Dice die)
        {
            return DicesPosition.TryGetValue(die, out var position) && position == BoardPoolPosition;
        }

        public void PlaceDieAtStart(Dice die, bool displayAfterPlacement)
        {
            var startPosition = StartingCell[die.Colour.ToLower()];
            UpdateDicePosition(die, (startPosition, -1), displayAfterPlacement, true);
        }

        public bool IsDieMovementAllowed(Dice die, int rollValue)
        {
            var position = DicesPosition[die];
            return position.Item2 + rollValue <= BOARD_COLOUR_SIZE;
        }
        public void MoveDice(Dice dice, int rolledValue)
        {
            Console.WriteLine($"Moving {dice.Id} by {rolledValue} steps.");
            var (common, priv) = DicesPosition[dice];
           
            int start = StartingCell[dice.Colour.ToLower()];
            int cellsMoved = (common >= start) ? (common - start) : (common + BOARD_SIZE - start);

            if (priv > 0)
            {
                priv += rolledValue;
            }
            else if (cellsMoved + rolledValue >= 56)
            {
                priv += rolledValue - (56 - cellsMoved);
                common = (start + 55) % BOARD_SIZE;
            }
            else
            {
                common = (common + rolledValue) % BOARD_SIZE;
            }

            UpdateDicePosition(dice, (common, priv), false, false);
        }


        public bool IsFinsihedMove(Dice dice)
        {
            var position = DicesPosition[dice];
            return position.Item2 == BOARD_COLOUR_SIZE;
        }

        public List<Dice> DicesAtSamePosition(Dice dice)
        {
            var position = DicesPosition[dice];
            List<Dice> samePositionDices = new();

            foreach (var Positions in DicesPosition)
            {
                if (Positions.Value == position && Positions.Key.Colour != dice.Colour)
                    samePositionDices.Add(Positions.Key);
            }
            return samePositionDices;
        }

        public void BoardDisplay()
        {
            LuddoBoard.BoardDisplay(DicesPosition);
        }
    }
}
