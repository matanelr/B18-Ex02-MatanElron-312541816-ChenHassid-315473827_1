using System;
using System.Collections.Generic;

namespace B18_Ex02
{
    // $G$ CSS-999 (-5) Every Class/Enum which is not nested should be in a separate file.
    internal enum eGameStatus
    {
        Winner,
        Lose,
        Draw,
        NotFinished,
    }

    internal class GameManager
    {
        private eGameStatus m_GameStatus;
        private bool v_Turn;
        private BoardGame m_BoardGame;
        private Player m_Player1;
        private Player m_Player2;
        private static Random s_Random;

        internal GameManager()
        {
            m_GameStatus = eGameStatus.NotFinished;
            v_Turn = true;
        }

        internal Player GetPlayer1()
        {
            return this.m_Player1;
        }

        internal Player GetPlayer2()
        {
            return this.m_Player2;
        }

        internal eGameStatus GetGameStatus()
        {
            return this.m_GameStatus;
        }

        internal BoardGame GetBoardGame()
        {
            return this.m_BoardGame;
        }

        internal void StartGame()
        {
            string nameOfPlayer1;
            short boardSize;
            string typeOfSeconedPlayer;

            nameOfPlayer1 = GameUI.GetPlayerName();
            this.m_Player1 = new Player(eShapeType.X, nameOfPlayer1, ePlayerType.Person);
            boardSize = GameUI.GetBoardSize();
            typeOfSeconedPlayer = GameUI.GetTypeOfSecondPlayer();
            setSeconedPlayer(typeOfSeconedPlayer);
            this.m_BoardGame = new BoardGame(boardSize);
            gameRound();
        }

        private void gameRound()
        {
            string currentMoveString;
            string prevMoveString;

            this.m_BoardGame.BuildBoard();
            currentMoveString = playFirstMoveOfGame();
           
            while (this.m_GameStatus == eGameStatus.NotFinished)
            {
                if (v_Turn)
                {
                    prevMoveString = currentMoveString;
                    currentMoveString = GameUI.GetMoveFromUser(m_Player2, m_Player1, m_BoardGame, prevMoveString);

                    if (GameUI.IsQuitInput(currentMoveString))
                    {
                        if (checkForQuitting(m_Player1, m_Player2))
                        {
                            break;
                        }
                        else
                        {
                            currentMoveString = GameUI.GetMessageInvalidQuit(m_BoardGame);
                        }
                    }

                    playCurrentPlayerTurn(m_Player1, m_Player2, ref currentMoveString, prevMoveString);
                }
                else
                {
                    if (m_Player2.PlayerType == ePlayerType.Person)
                    {
                        prevMoveString = currentMoveString;
                        currentMoveString = GameUI.GetMoveFromUser(m_Player1, m_Player2, m_BoardGame, prevMoveString);

                        if (GameUI.IsQuitInput(currentMoveString))
                        {
                            if (checkForQuitting(m_Player2, m_Player1))
                            {
                                break;
                            }
                            else
                            {
                                currentMoveString = GameUI.GetMessageInvalidQuit(m_BoardGame);
                            }
                        }

                        playCurrentPlayerTurn(m_Player2, m_Player1, ref currentMoveString, prevMoveString);
                    }
                    else
                    {
                        playComputerTurn(ref currentMoveString);
                    }

                    checkGameStatus();
                }
            }

            if (GameUI.checkForAnotherGameRound())
            {
                v_Turn = true;
                this.m_GameStatus = eGameStatus.NotFinished;
                short sizeOfBoard = m_BoardGame.Size;
                this.m_BoardGame = new BoardGame(sizeOfBoard);

                gameRound();
            }
        }

        private string playFirstMoveOfGame()
        {
            string currentMoveString = GameUI.GetFirstMoveFromUser(m_Player1, m_BoardGame);

            if (GameUI.IsQuitInput(currentMoveString))
            {
                m_GameStatus = eGameStatus.Draw;
                GameUI.PrintGamePointStatus(this);
            }

            else
            {
                Move currentMove = getMoveFromString(currentMoveString);

                while (!currentMove.CheckIsValidMove(m_Player1.GetShapeType()))
                {
                    GameUI.PrintErrorOfMove(eTypeOfMove.Regular);
                    currentMoveString = GameUI.GetFirstMoveFromUser(m_Player1, m_BoardGame);
                    currentMove = getMoveFromString(currentMoveString);
                }

                currentMove.MoveOnBoard(m_BoardGame);
                this.v_Turn = false;
            }

            return currentMoveString;
        }

        private void setSeconedPlayer(string i_TypeOfSeconedPlayer)
        {
            if (GameUI.IsUserTypeOfPlayer(i_TypeOfSeconedPlayer))
            {
                string nameOfPlayer2 = GameUI.GetSeconedPlayerName();
                this.m_Player2 = new Player(eShapeType.O, nameOfPlayer2, ePlayerType.Person);
            }
            else
            {
                this.m_Player2 = new Player(eShapeType.O, "Computer", ePlayerType.Computer);
                s_Random = new Random();
            }
        }

        private void playComputerTurn(ref string io_CurrentMoveString)
        {
            List<Move> computerJumpsMoves = m_BoardGame.GetListOfPlayerJumps(eShapeType.O);
            int lengthOfJumpsList = computerJumpsMoves.Count;
            Move currentMoveForComputer = null;

            if (lengthOfJumpsList > 0)
            {
                while (lengthOfJumpsList > 0)
                {
                    int indexOfJumplMove = s_Random.Next(0, lengthOfJumpsList);
                    currentMoveForComputer = computerJumpsMoves[indexOfJumplMove];
                    currentMoveForComputer.MoveType = eTypeOfMove.Jump;
                    currentMoveForComputer.MoveOnBoard(m_BoardGame);
                    m_Player2.IsJumpTurn = true;

                    if (hasAnotherJump(currentMoveForComputer, m_Player2))
                    {
                        computerJumpsMoves = getListOfJumpsForPiece(m_Player2.GetShapeType(), currentMoveForComputer.GetToPiece());
                        lengthOfJumpsList = computerJumpsMoves.Count;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                List<Move> computerDiagonalMoves = m_BoardGame.GetListOfPlayerDiagonalMoves(eShapeType.O);
                int lengthOfListDiagonal = computerDiagonalMoves.Count;
                int indexOfDiagonalMove = s_Random.Next(0, lengthOfListDiagonal);

                currentMoveForComputer = computerDiagonalMoves[indexOfDiagonalMove];
                currentMoveForComputer.MoveType = eTypeOfMove.Regular;
                currentMoveForComputer.MoveOnBoard(m_BoardGame);
            }

            io_CurrentMoveString = getStringFromMove(currentMoveForComputer);
            v_Turn = !v_Turn;
        }

        // $G$ CSS-013 (-3) Input parameters names should start with i_PascaleCase.
        private bool checkForQuitting(Player i_playerTurn, Player i_notPlayerTurn)
        {
            int playerTurnPoint = m_BoardGame.GetPointsOfPlayer(i_playerTurn.GetShapeType());
            int NotplayerTurnPoint = m_BoardGame.GetPointsOfPlayer(i_notPlayerTurn.GetShapeType());
            bool isValidQuit = (playerTurnPoint <= NotplayerTurnPoint);

            if (isValidQuit)
            {
                if (playerTurnPoint == NotplayerTurnPoint)
                {
                    m_GameStatus = eGameStatus.Draw;
                }
                else
                {
                    if (i_playerTurn.GetShapeType() == eShapeType.X)
                    {

                        m_GameStatus = eGameStatus.Lose;
                    }
                    else
                    {
                        m_GameStatus = eGameStatus.Winner;
                    }

                    i_notPlayerTurn.Points += NotplayerTurnPoint - playerTurnPoint;
                }

                GameUI.PrintGamePointStatus(this);
            }

            return isValidQuit;
        }

        private void checkGameStatus()
        {
            List<Move> diagonalMovesOfPlayer1 = m_BoardGame.GetListOfPlayerDiagonalMoves(eShapeType.X);
            List<Move> diagonalMovesOfPlayer2 = m_BoardGame.GetListOfPlayerDiagonalMoves(eShapeType.O);
            List<Move> jumpsMovesOfPlayer1 = m_BoardGame.GetListOfPlayerJumps(eShapeType.X);
            List<Move> jumpsMovesOfPlayer2 = m_BoardGame.GetListOfPlayerJumps(eShapeType.O);

            if (diagonalMovesOfPlayer1.Count == 0 && diagonalMovesOfPlayer2.Count == 0 && jumpsMovesOfPlayer1.Count == 0 && jumpsMovesOfPlayer2.Count == 0)
            {
                this.m_GameStatus = eGameStatus.Draw;
                GameUI.PrintGamePointStatus(this);
            }
            else
            {
                if (diagonalMovesOfPlayer1.Count == 0 && jumpsMovesOfPlayer1.Count == 0 || m_BoardGame.GetPointsOfPlayer(m_Player1.GetShapeType()) == 0)
                {
                    this.m_GameStatus = eGameStatus.Lose;
                    m_Player2.Points = m_BoardGame.GetPointsOfPlayer(m_Player2.GetShapeType()) - m_BoardGame.GetPointsOfPlayer(m_Player1.GetShapeType());
                    GameUI.PrintGamePointStatus(this);
                }
                else
                {
                    if (diagonalMovesOfPlayer2.Count == 0 && jumpsMovesOfPlayer2.Count == 0 || m_BoardGame.GetPointsOfPlayer(m_Player2.GetShapeType()) == 0)
                    {
                        this.m_GameStatus = eGameStatus.Winner;
                        m_Player1.Points = m_BoardGame.GetPointsOfPlayer(m_Player1.GetShapeType()) - m_BoardGame.GetPointsOfPlayer(m_Player2.GetShapeType());
                        GameUI.PrintGamePointStatus(this);
                    }
                }
            }
        }

        private Move getValidMove(ref string io_CurrentMoveString, Player i_PlayerTurn, Player i_NotPlayerTurn)
        {
            Move currentMove = getMoveFromString(io_CurrentMoveString);

            while (!isValidMove(currentMove, i_PlayerTurn))
            {
                io_CurrentMoveString = GameUI.GetValidMoveString(m_BoardGame);
                currentMove = getMoveFromString(io_CurrentMoveString);
            }

            return currentMove;
        }

        private void playCurrentPlayerTurn(Player i_PlayerTurn, Player i_NotPlayerTurn, ref string i_CurrentMoveString, String io_PrevMoveString)
        {
            Move currentMove = getValidMove(ref i_CurrentMoveString, i_PlayerTurn, i_NotPlayerTurn);
            currentMove.MoveOnBoard(m_BoardGame);
            v_Turn = !v_Turn;

            if (i_PlayerTurn.IsJumpTurn)
            {
                while (hasAnotherJump(currentMove, i_PlayerTurn))
                {
                    i_CurrentMoveString = playAnotherTurn(ref currentMove, io_PrevMoveString, i_PlayerTurn, i_NotPlayerTurn);
                    io_PrevMoveString = i_CurrentMoveString;
                }
            }
        }

        private string playAnotherTurn(ref Move i_PrevtMove, string i_PrevMoveString, Player i_PlayerTurn, Player i_NotPlayerTurn)
        {
            List<Move> playerSecondJumps = getListOfJumpsForPiece(i_PlayerTurn.GetShapeType(), i_PrevtMove.GetToPiece());

            string i_CurrentMoveString = GameUI.GetMoveFromUser(i_NotPlayerTurn, i_PlayerTurn, m_BoardGame, i_PrevMoveString);
            i_PrevtMove = getMoveFromString(i_CurrentMoveString);

            bool isValid = false;
            while (!isValid)
            {
                if (isContainsMoveElement(playerSecondJumps, i_PrevtMove))
                {
                    isValid = true;
                    i_PrevtMove.MoveType = eTypeOfMove.Jump;
                    i_PlayerTurn.IsJumpTurn = !i_PlayerTurn.IsJumpTurn;
                    i_PrevtMove.MoveOnBoard(m_BoardGame);
                }
                else
                {
                    GameUI.PrintErrorOfMove(eTypeOfMove.Jump);
                    i_CurrentMoveString = GameUI.GetMoveFromUser(i_NotPlayerTurn, i_PlayerTurn, m_BoardGame, i_CurrentMoveString);
                    i_PrevtMove = getMoveFromString(i_CurrentMoveString);
                }
            }

            return i_CurrentMoveString;
        }

        private bool hasAnotherJump(Move i_CurrentMove, Player i_PlayerTurn)
        {
            List<Move> playerSecondJumps = getListOfJumpsForPiece(i_PlayerTurn.GetShapeType(), i_CurrentMove.GetToPiece());

            return (playerSecondJumps.Count > 0) ? true : false;
        }

        // $G$ CSS-013 (-1) Input parameters names should start with i_PascaleCase.
        private bool isValidMove(Move i_currentMove, Player i_PlayerTurn)
        {
            bool isValid = false;

            List<Move> playerJumpMoves = m_BoardGame.GetListOfPlayerJumps(i_PlayerTurn.GetShapeType());

            if (playerJumpMoves.Count > 0)
            {
                if (isContainsMoveElement(playerJumpMoves, i_currentMove))
                {
                    isValid = true;
                    i_currentMove.MoveType = eTypeOfMove.Jump;
                    i_PlayerTurn.IsJumpTurn = true;
                }
                else
                {
                    i_PlayerTurn.IsJumpTurn = false;
                    GameUI.PrintErrorOfMove(eTypeOfMove.Jump);
                }
            }
            else
            {
                if (i_currentMove.CheckIsValidMove(i_PlayerTurn.GetShapeType()))
                {
                    isValid = true;
                    i_currentMove.MoveType = eTypeOfMove.Regular;
                }
                else
                {
                    GameUI.PrintErrorOfMove(eTypeOfMove.Regular);
                }
            }

            return isValid;
        }


        private static bool isContainsMoveElement(List<Move> i_ListOfMoves, Move i_currentMove)
        {
            bool isContainsMove = false;

            foreach (Move m in i_ListOfMoves)
            {
                if (i_currentMove.IsEqualsTo(m))
                {
                    isContainsMove = true;
                    break;
                }
            }

            return isContainsMove;
        }

        private Move getMoveFromString(string i_CurrentMoveString)
        {
            string fromSquare = i_CurrentMoveString.Substring(0, 2);
            string toSquare = i_CurrentMoveString.Substring(3, 2);
            int columnOfFromSquare = fromSquare[0] - 65;
            int rowOfFromSquare = fromSquare[1] - 97;
            int columnOfToSquare = toSquare[0] - 65;
            int rowOfToSquare = toSquare[1] - 97;
            Move currentMove = new Move(m_BoardGame.GetSquare(rowOfFromSquare, columnOfFromSquare), m_BoardGame.GetSquare(rowOfToSquare, columnOfToSquare));

            return currentMove;
        }

        private List<Move> getListOfJumpsForPiece(eShapeType i_Shape, Square i_Square)
        {
            int squareRow = i_Square.Row;
            int squareColumn = i_Square.Column;
            Move currentMove;
            List<Move> leggalJumpsForPiece = m_BoardGame.GetListOfPlayerJumps(i_Shape);

            for (int i = 0; i < leggalJumpsForPiece.Count; i++)
            {
                currentMove = leggalJumpsForPiece[i];

                if (currentMove.GetFromPiece().Row != squareRow || currentMove.GetFromPiece().Column != squareColumn)
                {
                    leggalJumpsForPiece.Remove(currentMove);
                }
            }

            return leggalJumpsForPiece;
        }

        private static string getStringFromMove(Move i_Move)
        {
            string fromSquare = (char)(i_Move.GetFromPiece().Column + 65) + "" + (char)(i_Move.GetFromPiece().Row + 97); ;
            string toSquare = (char)(i_Move.GetToPiece().Column + 65) + "" + (char)(i_Move.GetToPiece().Row + 97);

            return (fromSquare + ">" + toSquare);
        }
    }
}


