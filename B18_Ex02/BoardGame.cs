using System.Collections.Generic;
using System.Text;
namespace B18_Ex02
{
    // $G$ CSS-999 (-3) Every Class/Enum which is not nested should be in a separate file.
    public enum eSquareType
    {
        Invalid,
        None,
        O,
        U,
        X,
        K,
    }

    internal class Square
    {
        internal eSquareType m_Type;
        readonly internal int r_Row;
        readonly internal int r_Column;

        // $G$ CSS-013 (-1) Input parameters names should start with i_PascaleCase.
        internal Square(eSquareType type, int m_Row, int m_Column)
        {
            this.m_Type = type;
            this.r_Row = m_Row;
            this.r_Column = m_Column;
        }

        internal eSquareType Type
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
            }
        }

        internal int Row
        {
            get { return r_Row; }
        }

        internal int Column
        {
            get { return r_Column; }
        }

        internal string ToStringSqureType()
        {
            string squareTypeString = "";
            switch (this.m_Type)
            {
                case (eSquareType.None):
                    squareTypeString = "   ";
                    break;

                case (eSquareType.K):
                    squareTypeString = " K ";
                    break;

                case (eSquareType.O):
                    squareTypeString = " O ";
                    break;

                case (eSquareType.X):
                    squareTypeString = " X ";
                    break;

                case (eSquareType.U):
                    squareTypeString = " U ";
                    break;
            }

            return squareTypeString;
        }
    }

    internal class BoardGame
    {
        public Square[,] m_Board;
        public short m_Size;

        internal BoardGame(short size)
        {
            this.m_Size = size;
            m_Board = new Square[this.m_Size, this.m_Size];
            for (int i = 0; i < this.m_Size; i++)
            {
                for (int j = 0; j < this.m_Size; j++)
                {
                    m_Board[i, j] = new Square(eSquareType.None, i, j);
                }
            }
        }

        internal short Size
        {
            get { return m_Size; }
        }

        internal eSquareType StatusType
        {
            get { return this.StatusType; }
            set
            {
                this.StatusType = value;
            }
        }

        internal short GetSize()
        {
            return this.m_Size;
        }

        internal Square GetSquare(int i_Row, int i_Column)
        {
            return this.m_Board[i_Row, i_Column];
        }

        internal void BuildBoard()
        {
            {
                for (int i = 0; i < this.m_Size / 2 - 1; i++)
                {
                    for (int j = 0; j < this.m_Size; j++)
                    {
                        if (i % 2 == 1)
                        {
                            if (j % 2 == 0)
                            {
                                m_Board[i, j].Type = eSquareType.O;
                            }
                        }
                        else
                        {
                            if (j % 2 == 1)
                            {
                                m_Board[i, j].Type = eSquareType.O;
                            }
                        }
                    }
                }
                for (int i = this.m_Size - 1; i > this.m_Size / 2; i--)
                {
                    for (int j = 0; j < this.m_Size; j++)
                    {
                        if (i % 2 == 1)
                        {
                            if (j % 2 == 0)
                            {
                                m_Board[i, j].Type = eSquareType.X;
                            }
                        }
                        else
                        {
                            if (j % 2 == 1)
                            {
                                m_Board[i, j].Type = eSquareType.X;
                            }
                        }
                    }
                }
            }
        }

        internal void PrintBoard()
        {
            StringBuilder stringBuilderBoard = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            stringBuilderBoard.Append(" ");

            for (int k = 0; k < this.m_Size; k++)
            {
                stringBuilderBoard.Append("  " + (char)(k + 65) + " ");
            }

            stringBuilderBoard.Append(System.Environment.NewLine);

            for (int i = 0; i < this.m_Size; i++)
            {
                stringBuilderBoard.Append(" ");

                for (int k = 0; k < this.m_Size * 4 + 1; k++)
                {
                    stringBuilderBoard.Append("=");
                }

                stringBuilderBoard.Append(System.Environment.NewLine);
                stringBuilderBoard.Append((char)(i + 97));

                for (int j = 0; j < this.m_Size; j++)
                {
                    stringBuilderBoard.Append("|" + m_Board[i, j].ToStringSqureType());
                }

                stringBuilderBoard.Append("|");
                stringBuilderBoard.Append(System.Environment.NewLine);
            }

            stringBuilderBoard.Append(" ");
            for (int k = 0; k < this.m_Size * 4 + 1; k++)
            {
                stringBuilderBoard.Append("=");
            }

            System.Console.WriteLine(stringBuilderBoard.ToString());
        }

        internal int GetPointsOfPlayer(eShapeType i_Piece)
        {
            int countPlayerPoints = 0;
            switch (i_Piece)
            {
                case (eShapeType.X):
                    
                    for (int i = 0; i < this.m_Size; i++)
                    {
                        for (int j = 0; j < this.m_Size; j++)
                        {
                            Square currentSquare = this.m_Board[i, j];
                            if (currentSquare.Type == eSquareType.X)
                            {
                                countPlayerPoints += 1;
                            }
                            if (currentSquare.Type == eSquareType.K)
                            {
                                countPlayerPoints += 4;
                            }
                        }
                    }

                    break;

                case (eShapeType.O):
                    
                    for (int i = 0; i < this.m_Size; i++)
                    {
                        for (int j = 0; j < this.m_Size; j++)
                        {
                            Square currentSquare = this.m_Board[i, j];
                            if (currentSquare.Type == eSquareType.O)
                            {
                                countPlayerPoints += 1;
                            }
                            if (currentSquare.Type == eSquareType.U)
                            {
                                countPlayerPoints += 4;
                            }
                        }
                    }

                    break;
            }

            return countPlayerPoints;
        }

        private eSquareType getTypeOfSquareInBoard(int i_Row, int i_Column)
        {
            eSquareType typeToReturn;
            if (i_Row < 0 || i_Row > this.GetSize() - 1 || i_Column < 0 || i_Column > this.GetSize() - 1)
            {
                typeToReturn = eSquareType.Invalid;
            }
            else
            {
                typeToReturn = this.m_Board[i_Row, i_Column].Type;
            }

            return typeToReturn;
        }

        // $G$ DSN-003 (-10) This method is too long. 
        internal List<Move> GetListOfPlayerDiagonalMoves(eShapeType i_Shape)
        {
            List<Move> leggalMoves = new List<Move>();
            short boardSize = this.GetSize();

            for (int r = 0; r < boardSize; r++)
            {
                for (int c = 0; c < boardSize; c++)
                {
                    switch (i_Shape)
                    {
                        case eShapeType.O:

                            if (getTypeOfSquareInBoard(r, c) == eSquareType.O) //regular piece of O
                            {
                                if ((getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r + 1, c - 1)));
                                }
                                if ((getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r + 1, c + 1)));
                                }
                            }
                            if (getTypeOfSquareInBoard(r, c) == eSquareType.U) //King of O
                            {
                                if ((getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r + 1, c - 1)));
                                }
                                if ((getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r + 1, c + 1)));
                                }
                                if ((getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r - 1, c - 1)));
                                }
                                if ((getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r - 1, c + 1)));
                                }

                            }

                            break;

                        case eShapeType.X:
                            if (getTypeOfSquareInBoard(r, c) == eSquareType.X) //regular piece for X
                            {
                                if ((getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r - 1, c - 1)));
                                }
                                if ((getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r - 1, c + 1)));
                                }
                            }

                            if (getTypeOfSquareInBoard(r, c) == eSquareType.K) //King of X
                            {
                                if ((getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r + 1, c - 1)));
                                }
                                if ((getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r + 1, c + 1)));
                                }
                                if ((getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r - 1, c - 1)));
                                }
                                if ((getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.None))
                                {
                                    leggalMoves.Add(new Move(GetSquare(r, c), GetSquare(r - 1, c + 1)));
                                }
                            }

                            break;
                    }
                }
            }

            return leggalMoves;
        }

        internal List<Move> GetListOfPlayerJumps(eShapeType i_Shape)
        {
            List<Move> leggalJumps = new List<Move>();
            short boardSize = this.GetSize();

            for (int r = 0; r < boardSize; r++)
            {
                for (int c = 0; c < boardSize; c++)
                {
                    switch (i_Shape)
                    {
                        case eShapeType.O:
                            
                            if (getTypeOfSquareInBoard(r, c) == eSquareType.O) //regular piece of O
                            {
                                if ((getTypeOfSquareInBoard(r + 2, c - 2) == eSquareType.None) && (getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.X || getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.K))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r + 2, c - 2)));
                                }
                                if ((getTypeOfSquareInBoard(r + 2, c + 2) == eSquareType.None) && (getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.X || getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.K))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r + 2, c + 2)));
                                }
                            }
                            if (getTypeOfSquareInBoard(r, c) == eSquareType.U) //King of O
                            {
                                if ((getTypeOfSquareInBoard(r + 2, c - 2) == eSquareType.None) && (getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.X || getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.K))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r + 2, c - 2)));
                                }
                                if ((getTypeOfSquareInBoard(r + 2, c + 2) == eSquareType.None) && (getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.X || getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.K))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r + 2, c + 2)));
                                }
                                if ((getTypeOfSquareInBoard(r - 2, c - 2) == eSquareType.None) && (getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.X || getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.K))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r - 2, c - 2)));
                                }
                                if ((getTypeOfSquareInBoard(r - 2, c + 2) == eSquareType.None) && (getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.X || getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.K))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r - 2, c + 2)));
                                }
                            }

                            break;

                        case eShapeType.X:
                            
                            if (getTypeOfSquareInBoard(r, c) == eSquareType.X) //regular piece of X
                            {
                                if ((getTypeOfSquareInBoard(r - 2, c - 2) == eSquareType.None) && (getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.O || getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.U))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r - 2, c - 2)));
                                }
                                if ((getTypeOfSquareInBoard(r - 2, c + 2) == eSquareType.None) && (getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.O || getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.U))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r - 2, c + 2)));
                                }
                            }
                            if (getTypeOfSquareInBoard(r, c) == eSquareType.K) //king of X
                            {
                                if ((getTypeOfSquareInBoard(r + 2, c - 2) == eSquareType.None) && (getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.O || getTypeOfSquareInBoard(r + 1, c - 1) == eSquareType.U))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r + 2, c - 2)));
                                }
                                if ((getTypeOfSquareInBoard(r + 2, c + 2) == eSquareType.None) && (getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.O || getTypeOfSquareInBoard(r + 1, c + 1) == eSquareType.U))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r + 2, c + 2)));
                                }
                                if ((getTypeOfSquareInBoard(r - 2, c - 2) == eSquareType.None) && (getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.O || getTypeOfSquareInBoard(r - 1, c - 1) == eSquareType.U))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r - 2, c - 2)));
                                }
                                if ((getTypeOfSquareInBoard(r - 2, c + 2) == eSquareType.None) && (getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.O || getTypeOfSquareInBoard(r - 1, c + 1) == eSquareType.U))
                                {
                                    leggalJumps.Add(new Move(GetSquare(r, c), GetSquare(r - 2, c + 2)));
                                }
                            }

                            break;
                    }
                }
            }

            return leggalJumps;
        }
    }
}

