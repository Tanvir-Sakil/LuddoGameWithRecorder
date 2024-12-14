using System;
using System.Collections.Generic;
namespace LuddoGameCLI
{
    public class Player
    {
        public string Name { get; private set; }
        public string Colour { get; private set; }
        public List<Dice> Dices { get; private set; }
        public bool Finished { get; private set; }

        public int Index { get; set; }

        public bool IsHuman;

        public Player(string colour, bool HumanOrNot, string name = "")
        {
            Colour = colour;
            Name = name ?? "Computer";
            IsHuman = HumanOrNot;
            Finished = false;

            Dices = new List<Dice>();
            for (int i = 1; i <= 4; i++)
            {
                Dices.Add(new Dice(i, colour));
            }
        }

        public int ChooseDice(List<Dice> availableDices)
        {
            if (availableDices.Count == 1)
                return 0;
            if (IsHuman)
            {
                Console.WriteLine("Choose a dice to move:");
                for (int i = 0; i < availableDices.Count; i++)
                    Console.WriteLine($"{i} - {availableDices[i].Id}");

                int pawnIndex;
                while (!int.TryParse(Console.ReadLine(), out pawnIndex) || pawnIndex < 0 || pawnIndex >= availableDices.Count)
                {
                    Console.WriteLine("Invalid choice.Please select a valid dice.");
                }
                return pawnIndex;
            }
            else
            {
                return new Random().Next(availableDices.Count);
            }


        }

        public override string ToString()
        {
            return $"{Name} ({Colour})";
        }
    }
}

