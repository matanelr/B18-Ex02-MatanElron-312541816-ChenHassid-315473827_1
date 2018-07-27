using System;

namespace B18_Ex02
{
    internal static class GameUI
    {
        internal static string GetPlayerName()
        {
            string nameOfPlayer;

            Console.WriteLine("Welcome! what is your player name?");
            nameOfPlayer = Console.ReadLine();

            while (!CheckValidPlayerName(nameOfPlayer))
            {
                Console.WriteLine("Invalid player name, please enter a maximum 20 letters name without spaces");
                nameOfPlayer = Console.ReadLine();
            }

            return nameOfPlayer;
        }

        internal static string GetSeconedPlayerName()
        {
            string nameOfPlayer;

            Console.WriteLine("What is your competitor name?");
            nameOfPlayer = Console.ReadLine();

            while (!CheckValidPlayerName(nameOfPlayer))
            {
                Console.WriteLine("Invalid player name, please enter a maximum 20 letters name without spaces");
                nameOfPlayer = Console.ReadLine();
            }

            return nameOfPlayer;
        }

        internal static short GetBoardSize()
        {
            string sizeString;
            short boardSize;
            bool validSize;

            Console.WriteLine("Please enter the size of the board? (6/8/10)");

            sizeString = Console.ReadLine();
            validSize = short.TryParse(sizeString, out boardSize);

            while (!validSize || (boardSize != 6 && boardSize != 8 && boardSize != 10))
            {
                Console.WriteLine("Not valid size! Please enter 6, 8 or 10");
                sizeString = Console.ReadLine();
                validSize = short.TryParse(sizeString, out boardSize);
            }

            return boardSize;
        }

        internal static string GetTypeOfSecondPlayer()
        {
            string typeOfGame;

            Console.WriteLine("Please enter 1 for playing against a computer opponent or 2 for another human upponent");
            typeOfGame = Console.ReadLine();

            while (!checkValidTypeOfSecondPlayer(typeOfGame))
            {
                Console.WriteLine("Invalid type, please enter: 1 for playing against a computer opponent or 2 for another human opponent");
                typeOfGame = Console.ReadLine();
            }

            return typeOfGame;
        }

        private static bool checkValidTypeOfSecondPlayer(string i_TypeOfGame)
        {
            return (i_TypeOfGame.Equals("1") || i_TypeOfGame.Equals("2"));
        }

        internal static bool CheckValidPlayerName(string i_NameOfPlayer)
        {
            const short k_PlayerNameValidLength = 20;
            return !((i_NameOfPlayer.Length > k_PlayerNameValidLength) || i_NameOfPlayer.Contains(" "));
        }
             
        internal static string GetMoveFromUser(Player i_PrevioustPlayer, Player i_NextPlayer, BoardGame i_BoardGame, string i_PrevioustMove)
        {
            string message;
            string currentMoveString;

            i_BoardGame.PrintBoard();

            message = string.Format("{0}'s move was ({1}): {2}{3}{4}'s turn ({5}):",
                                    i_PrevioustPlayer.Name, i_PrevioustPlayer.GetShapeType(), i_PrevioustMove,
                                    Environment.NewLine, i_NextPlayer.Name, i_NextPlayer.GetShapeType());
            Console.WriteLine(message);
            currentMoveString = GetValidMoveString(i_BoardGame);

            return currentMoveString;
        }

        internal static string GetMessageInvalidQuit(BoardGame i_BoardGame)
        {
            string currentMoveString;
            Console.WriteLine("You can't quit at this point of the game. Please Enter a valid move");
            currentMoveString = Console.ReadLine();

            while (!checkValidMoveInput(currentMoveString, i_BoardGame) || currentMoveString.Equals("Q"))
            {
                Console.WriteLine("Invalid move! please enter a new move");
                currentMoveString = Console.ReadLine();
            }

            return currentMoveString;
        }

        internal static string GetFirstMoveFromUser(Player i_CurrentPlayer, BoardGame i_BoardGame)
        {
            string message;
            string currentMoveString;

            i_BoardGame.PrintBoard();
            message = string.Format("{0}'s turn:", i_CurrentPlayer.Name);
            Console.WriteLine(message);
            currentMoveString = GetValidMoveString(i_BoardGame);

            return currentMoveString;
        }

        internal static string GetValidMoveString(BoardGame i_BoardGame)
        {
            string currentMoveString = Console.ReadLine();

            while (!checkValidMoveInput(currentMoveString, i_BoardGame))
            {
                Console.WriteLine("Invalid move! please enter a new move");
                currentMoveString = Console.ReadLine();
            }

            return currentMoveString;
        }

        private static bool checkValidMoveInput(String i_currentMove, BoardGame i_BoardGame)
        {
            bool isValid = true;
            const short k_ValidInputLength = 5;
            short boardSize = i_BoardGame.GetSize();

            if (i_currentMove.Length == 1)
            {
                if (!i_currentMove.Equals("Q"))
                {
                    isValid = false;
                }
            }

            else
            {
                if (i_currentMove.Length == k_ValidInputLength && i_currentMove[2] == '>')
                {
                    if (i_currentMove[0] < 'A' || i_currentMove[0] > boardSize + 65 || i_currentMove[3] < 'A' || i_currentMove[3] > boardSize + 65)
                    {
                        isValid = false;
                    }
                    else if (i_currentMove[1] < 'a' || i_currentMove[1] > boardSize + 97 || i_currentMove[4] < 'a' || i_currentMove[4] > boardSize + 97)
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        internal static void PrintErrorOfMove(eTypeOfMove i_ExpectedTypeOfMove)
        {
            switch (i_ExpectedTypeOfMove)
            {
                case eTypeOfMove.Jump:
                    Console.WriteLine("Invalid move! You must take a jump");
                    break;

                case eTypeOfMove.Regular:
                    Console.WriteLine("Invalid move! please take a valid one");
                    break;
            }
        }

        internal static void PrintGamePointStatus(GameManager i_CurrentGame)
        {
            string message;

            switch (i_CurrentGame.GetGameStatus())
            {
                case eGameStatus.Draw:
                    i_CurrentGame.GetBoardGame().PrintBoard();
                    message = string.Format("This game is a draw!{0}points of {1}:{2}{3}points of {4}:{5}",
                                      System.Environment.NewLine, i_CurrentGame.GetPlayer1().Name, i_CurrentGame.GetPlayer1().Points,
                                      System.Environment.NewLine, i_CurrentGame.GetPlayer2().Name, i_CurrentGame.GetPlayer2().Points);
                    Console.WriteLine(message);
                    break;

                case eGameStatus.Lose:
                    i_CurrentGame.GetBoardGame().PrintBoard();
                    message = string.Format("{0} is the winner of this game!!{1}{2}: {3} points{4}{5}: {6} points"
                                      , i_CurrentGame.GetPlayer2().Name, System.Environment.NewLine, i_CurrentGame.GetPlayer2().Name, i_CurrentGame.GetPlayer2().Points,
                                      System.Environment.NewLine, i_CurrentGame.GetPlayer1().Name, i_CurrentGame.GetPlayer1().Points);
                    Console.WriteLine(message);
                    break;

                case eGameStatus.Winner:
                    i_CurrentGame.GetBoardGame().PrintBoard();
                    message = string.Format("{0} is the winner of this game!!{1}{2}: {3} points{4}{5}: {6} points"
                                      , i_CurrentGame.GetPlayer1().Name, System.Environment.NewLine, i_CurrentGame.GetPlayer1().Name, i_CurrentGame.GetPlayer1().Points,
                                      System.Environment.NewLine, i_CurrentGame.GetPlayer2().Name, i_CurrentGame.GetPlayer2().Points);
                    Console.WriteLine(message);
                    break;
            }
        }

        // $G$ CSS-999 (-3) internal methods should start with an uppercase letter
        internal static bool checkForAnotherGameRound()
        {
            string anotherGameRound;
            string message;
            bool continuePlay = false;

            message = string.Format("{0}Would you like to play another round? please press yes/no", System.Environment.NewLine);
            Console.WriteLine(message);
            anotherGameRound = System.Console.ReadLine();

            if (anotherGameRound.Equals("yes"))
            {
                message = string.Format("Welcome to another game round!{0}", System.Environment.NewLine);
                Console.WriteLine(message);
                continuePlay = true;
            }
            else
            {
                if (anotherGameRound.Equals("no"))
                {
                    message = string.Format("Thank you for playing!! Goodbye");
                    Console.WriteLine(message);
                    System.Console.ReadLine();
                    continuePlay = false;
                }
                else
                {
                    message = string.Format("Invalid input!");
                    Console.WriteLine(message);
                    continuePlay = checkForAnotherGameRound();
                }
            }

            return continuePlay;
        }

        internal static bool IsQuitInput(string i_UserInput){
            
            return i_UserInput.Equals("Q");
        }

        internal static bool IsUserTypeOfPlayer(string i_TypeOfSeconedPlayer)
        {
            return i_TypeOfSeconedPlayer.Equals("2");
        }
    }
}