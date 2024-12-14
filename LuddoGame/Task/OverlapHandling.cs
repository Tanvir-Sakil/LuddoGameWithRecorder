using System;
using System.Collections.Generic;
using System.Linq;

namespace LuddoGameCLI
{
    public class OverlapHandling
    {
        private HandleDice handleDice;

        public OverlapHandling(HandleDice existingHandleDice)
        {
            handleDice = existingHandleDice;
        }

        public void HandleOverlapping(Dice dice)
        {
            var OverlappedDices = handleDice.DicesAtSamePosition(dice);
            foreach (var overlappedDice in OverlappedDices)
            {
                if (overlappedDice.Colour == dice.Colour)
                {
                    int offset = 1;
                    handleDice.UpdateDicePosition(overlappedDice, (handleDice.DicesPosition[dice].Common, handleDice.DicesPosition[dice].Private + offset), true, false);
                }
                else
                {
                    handleDice.MoveDieToPool(overlappedDice, false);
                }
            }
        }
    }
}
