using System;

namespace LuddoGameCLI
{
    public class Menu
    {
        public int GetUserInitialChoice()
        {
            Console.WriteLine("Choose option:\n0 - Start new game\n1 - Continue Saved game\n2 - View saved game");
            if (int.TryParse(Console.ReadLine(), out int choice) && (choice == 0 || choice == 1 ||choice==2))
            {
                return choice;
            }
            Console.WriteLine("Invalid input. Defaulting to start a new game.");
            return 0;
        }
    }
}
