using System;
using System.Collections.Generic;

namespace LuddoGameCLI
{
    public class DicePositioner
    {
        private static readonly Dictionary<string, List<(int, int)>> COLOUR_CELL = new Dictionary<string, List<(int, int)>>
        {
            { "yellow", new List<(int, int)> { (0, 0), (15, 7), (15, 13), (15, 19), (15, 25), (15, 31), (15, 37) } },
            { "blue", new List<(int, int)> { (0, 0), (3, 43), (5, 43), (7, 43), (9, 43), (11, 43), (13, 43) } },
            { "red", new List<(int, int)> { (0, 0), (15, 79), (15, 73), (15, 67), (15, 61), (15, 55), (15, 49) } },
            { "green", new List<(int, int)> { (0, 0), (27, 43), (25, 43), (23, 43), (21, 43), (19, 43), (17, 43) } }
        };

        private static readonly Dictionary<string, List<(int, int)>> INITIAL_CELL = new Dictionary<string, List<(int, int)>>
        {
            { "yellow", new List<(int, int)> { (0, 0), (5, 13), (5, 18), (7, 13), (7, 18) } },
            { "blue", new List<(int, int)> { (0, 0), (5, 70), (5, 75), (7, 70), (7, 75) } },
            { "red", new List<(int, int)> { (0, 0), (23, 70), (23, 75), (25, 70), (25, 75) } },
            { "green", new List<(int, int)> { (0, 0), (23, 13), (23, 18), (25, 13), (25, 18) } }
        };

        private static readonly (int, int)[] COMMON_CELL = new (int, int)[] {
            (0, 0), (13, 2), (13, 8), (13, 14), (13, 20), (13, 26), (13, 32), (13, 38),
            (11, 38), (9, 38), (7, 38), (5, 38), (3, 38), (1, 38), (1, 44),
            (1, 50), (3, 50), (5, 50), (7, 50), (9, 50), (11, 50), (13, 50),
            (13, 56), (13, 62), (13, 68), (13, 74), (13, 80), (13, 86), (15, 86),
            (17, 86), (17, 80), (17, 74), (17, 68), (17, 62), (17, 56), (17, 50),
            (19, 50), (21, 50), (23, 50), (25, 50), (27, 50), (29, 50), (29, 44),
            (29, 38), (27, 38), (25, 38), (23, 38), (21, 38), (19, 38), (17, 38),
            (17, 32), (17, 26), (17, 20), (17, 14), (17, 8), (17, 2), (15, 2)
        };

        public (int row, int column) GetDicePosition(Dice dice, (int Common, int Private) position)
        {
            int commonPos = position.Common;
            int privatePos = position.Private;
            string colour = dice.Colour.ToLower();

            if (commonPos == -1 && privatePos == -1)
            {
                return INITIAL_CELL[colour][dice.Index];
            }
            else if (privatePos > 0)
            {
                if (privatePos >= COLOUR_CELL[colour].Count)
                {
                    Console.WriteLine($"{dice.Id} has finished its move.");
                    return (-1, -1);
                }
                return COLOUR_CELL[colour][privatePos];
            }
            else
            {
                if (commonPos == 0)
                {
                    return (15, 2);
                }
                return COMMON_CELL[commonPos];
            }
        }
    }
}
