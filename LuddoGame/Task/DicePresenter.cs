using System;

public class DiePresenter
{
    public void PresentDieWithPlayerName(int number, string name)
    {
        // Horizontal line for the die border
        string horLine = new string('-', 9);
        string sps = new string(' ', 37);
        horLine = sps + horLine;

        // Matrix representation of die faces
        string[][] matrix = new string[][]
        {
            new string[] { "|       |", "|   #   |", "|       |" },
            new string[] { "|       |", "| #   # |", "|       |" },
            new string[] { "|     # |", "|   #   |", "| #     |" },
            new string[] { "| #   # |", "|       |", "| #   # |" },
            new string[] { "| #   # |", "|   #   |", "| #   # |" },
            new string[] { "| # # # |", "|       |", "| # # # |" }
        };

        // Add spaces before each cell
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                matrix[i][j] = sps + matrix[i][j];
            }
        }

        // Select the die face based on the number
        string[] die = matrix[number - 1];

        // Append the player name to the second row
        die[1] += "   " + name;

        // Combine the lines into a single string
        string result = string.Join(Environment.NewLine, new[] { horLine, die[0], die[1], die[2], horLine });
        Console.WriteLine(result);
    }
}
