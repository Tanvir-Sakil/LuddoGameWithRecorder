using System;

namespace LuddoGameCLI
{
    public struct Dice
    {
        public int Index { get; }
        public string Colour { get; }
        public string Id { get; }

        public Dice(int index, string colour)
        {
            Index = index;
            Colour = colour;
            Id = $"{colour[0].ToString().ToUpper()}{index}";
        }

        // Convert Dice to a string for serialization
        public override string ToString()
        {
            return $"{Index}-{Colour}";
        }

        // Recreate Dice from a string during deserialization
        public static Dice? FromString(string str)
        {
            var parts = str.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[0], out int index))
            {
                return new Dice(index, parts[1]);
            }
            return null; // Return null if parsing fails
        }

        // Override Equals to compare Dice instances
        public override bool Equals(object obj)
        {
            if (obj is Dice other)
            {
                return Index == other.Index && Colour == other.Colour;
            }
            return false;
        }

        // Override GetHashCode for dictionary key usage
        public override int GetHashCode()
        {
            return HashCode.Combine(Index, Colour);
        }
    }
}
