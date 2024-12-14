using System;

namespace LuddoGameCLI
{
    public class GameStarter
    {
        private readonly Menu menu = new Menu();
        private readonly Game game = new Game();
        private readonly ManagePlayer playerManager;
        private readonly CLIGame cligame;
        public GameStarter()
        {
            playerManager = new ManagePlayer(game);
        }
        public void Start()
        {
            Console.WriteLine("Welcome to Luddo Game");
            int choice = menu.GetUserInitialChoice();
            switch (choice)
            {
                case 0:
                    playerManager.InitializePlayer();
                
                    StartGamePlay(0);
                    break;
                case 1:
                    StartGamePlay(1);
                    break;
                case 2:
                    StartGamePlay(2);
                    break;
            }
        }

        private void StartGamePlay(int option)
        {
            CLIGame cliGame = new CLIGame(game);
            if (option == 0)
            {
                cliGame.PlayGame();
            }
            if(option ==1)
            {
                cliGame.ContinueGame();
            }
            if(option==2)
            {
                cliGame.RunRecoredGame();
            }
            
        }
    }
}
