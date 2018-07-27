using System;
namespace B18_Ex02
{
    internal enum eTypeOfMove
    {
        Jump,
        Regular,
    }
    internal class Move
    {
        private Square m_FromSquare;
        private Square m_ToSquare;
        private eTypeOfMove m_TypeOfMove;

        internal Move(Square m_FromPiece, Square m_ToPiece)
        {
            this.m_FromSquare = m_FromPiece;
            this.m_ToSquare = m_ToPiece;
        }

        internal eTypeOfMove MoveType
        {
            get { return this.m_TypeOfMove; }
            set
            {
                this.m_TypeOfMove = value;
            }
        }

        internal Square GetToPiece()
        {
            return this.m_ToSquare;
        }

        internal Square GetFromPiece()
        {
            return this.m_FromSquare;
        }

        internal bool IsEqualsTo(Move i_MovetoCompare)
        {
            bool isEqual = true;
            if (this.GetFromPiece().Row != i_MovetoCompare.GetFromPiece().Row || this.GetToPiece().Row != i_MovetoCompare.GetToPiece().Row)
            {
                isEqual = false;
            }
            if (this.GetFromPiece().Column != i_MovetoCompare.GetFromPiece().Column || this.GetToPiece().Column != i_MovetoCompare.GetToPiece().Column)
            {
                isEqual = false;
            }

            return isEqual;
        }

        internal void MoveOnBoard(BoardGame i_BoardGame)
        {
            switch (this.MoveType)
            {
                case (eTypeOfMove.Regular):

                    if (m_FromSquare.Type == eSquareType.X && m_ToSquare.Row == 0)
                    {
                        m_ToSquare.Type = eSquareType.K;
                    }

                    else
                        if (m_FromSquare.Type == eSquareType.O && m_ToSquare.Row == i_BoardGame.GetSize() - 1)
                    {
                        m_ToSquare.Type = eSquareType.U;
                    }
                    else
                    {
                        m_ToSquare.Type = m_FromSquare.Type;
                    }
                    m_FromSquare.Type = eSquareType.None;
                    break;

                case (eTypeOfMove.Jump):
                    capturePieceOnBoard(i_BoardGame);

                    if (m_FromSquare.Type == eSquareType.X && m_ToSquare.Row == 0)
                    {
                        m_ToSquare.Type = eSquareType.K;
                    }

                    else
                    {
                        if (m_FromSquare.Type == eSquareType.O && m_ToSquare.Row == i_BoardGame.GetSize() - 1)
                        {
                            m_ToSquare.Type = eSquareType.U;
                        }
                        else
                        {
                            m_ToSquare.Type = m_FromSquare.Type;
                        }
                    }
                    m_FromSquare.Type = eSquareType.None;
                    break;
            }
        }

        private void capturePieceOnBoard(BoardGame i_BoardGame)
        {
            int rowOfCapturPiece = 0;
            int columnOfCapturPiece = 0;

            if (m_FromSquare.Row > m_ToSquare.Row)
            {
                rowOfCapturPiece = m_FromSquare.Row - 1;

                if (m_FromSquare.Column > m_ToSquare.Column)
                {
                    columnOfCapturPiece = m_FromSquare.Column - 1;
                }
                else
                {
                    columnOfCapturPiece = m_FromSquare.Column + 1;
                }
            }
            else
            {
                rowOfCapturPiece = m_FromSquare.Row + 1;

                if (m_FromSquare.Column > m_ToSquare.Column)
                {
                    columnOfCapturPiece = m_FromSquare.Column - 1;
                }
                else
                {
                    columnOfCapturPiece = m_FromSquare.Column + 1;
                }
            }

            i_BoardGame.GetSquare(rowOfCapturPiece, columnOfCapturPiece).Type = eSquareType.None;
        }

        internal bool CheckIsValidMove(eShapeType i_ShapeOfPlayer)
        {
            bool isValidMove = true;

            switch (i_ShapeOfPlayer)
            {
                case eShapeType.X:
                    if (m_FromSquare.Type != eSquareType.X && m_FromSquare.Type != eSquareType.K)
                    {
                        isValidMove = false;
                    }
                    else
                    {
                        if (m_ToSquare.Type != eSquareType.None)
                        {
                            isValidMove = false;
                        }
                        else
                        {
                            if (m_FromSquare.Type == eSquareType.X)
                            {
                                isValidMove = isValidDiagonalMove(eShapeType.X);
                            }
                            else
                            {
                                isValidMove = isValidDiagonalKingMove(eShapeType.X);
                            }
                        }
                    }
                    break;

                case eShapeType.O:

                    if (m_FromSquare.Type != eSquareType.O && m_FromSquare.Type != eSquareType.U)
                    {
                        isValidMove = false;

                    }
                    else
                    {
                        if (m_ToSquare.Type != eSquareType.None)
                        {
                            isValidMove = false;
                        }
                        else
                        {
                            if (m_FromSquare.Type == eSquareType.O)
                            {
                                isValidMove = isValidDiagonalMove(eShapeType.O);
                            }

                            else
                            {
                                isValidMove = isValidDiagonalKingMove(eShapeType.O);
                            }
                        }
                    }
                    break;
            }

            return isValidMove;

        }

        private bool isValidDiagonalMove(eShapeType i_Shape)
        {
            bool isValidMove = false;

            switch (i_Shape)
            {
                case eShapeType.X:
                    if ((m_FromSquare.Row - 1 == m_ToSquare.Row) && (m_FromSquare.Column - 1 == m_ToSquare.Column))
                    {
                        isValidMove = true;
                    }

                    if ((m_FromSquare.Row - 1 == m_ToSquare.Row) && (m_FromSquare.Column + 1 == m_ToSquare.Column))
                    {
                        isValidMove = true;
                    }

                    break;

                case eShapeType.O:
                    if ((m_FromSquare.Row + 1 == m_ToSquare.Row) && (m_FromSquare.Column - 1 == m_ToSquare.Column))
                    {
                        isValidMove = true;
                    }

                    if ((m_FromSquare.Row + 1 == m_ToSquare.Row) && (m_FromSquare.Column + 1 == m_ToSquare.Column))
                    {
                        isValidMove = true;
                    }

                    break;
            }

            return isValidMove;
        }

        private bool isValidDiagonalKingMove(eShapeType i_ShapeOfMove)
        {
            bool isValidKingMove = false;

            if ((m_FromSquare.Row - 1 == m_ToSquare.Row) && (m_FromSquare.Column - 1 == m_ToSquare.Column))
            {
                isValidKingMove = true;
            }

            if ((m_FromSquare.Row - 1 == m_ToSquare.Row) && (m_FromSquare.Column + 1 == m_ToSquare.Column))
            {
                isValidKingMove = true;
            }

            if ((m_FromSquare.Row + 1 == m_ToSquare.Row) && (m_FromSquare.Column - 1 == m_ToSquare.Column))
            {
                isValidKingMove = true;
            }

            if ((m_FromSquare.Row + 1 == m_ToSquare.Row) && (m_FromSquare.Column + 1 == m_ToSquare.Column))
            {
                isValidKingMove = true;
            }

            return isValidKingMove;
        }

    }
}
