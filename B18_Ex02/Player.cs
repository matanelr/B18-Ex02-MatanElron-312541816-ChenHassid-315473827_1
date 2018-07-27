namespace B18_Ex02
{
    internal enum eShapeType
    {
        X,
        O,
    }

    internal enum ePlayerType
    {
        Person,
        Computer,
    }

    internal class Player
    {
        private string m_PlayerName;
        private eShapeType m_Shape;
        private ePlayerType m_PlayerType;
        private bool v_JumpTurn;
        private int m_Points;

        internal Player(eShapeType shape, string PlayerName, ePlayerType i_PlayerType)
        {
            this.m_Shape = shape;
            this.m_PlayerName = PlayerName;
            v_JumpTurn = false;
            this.m_PlayerType = i_PlayerType;
            this.m_Points = 0;
        }

        internal ePlayerType PlayerType
        {
            get
            {
                return this.m_PlayerType;
            }

            set
            {
                m_PlayerType = value;
            }
        }

        internal int Points
        {
            get
            {
                return this.m_Points;
            }

            set
            {
                m_Points = value;
            }
        }

        internal bool IsJumpTurn
        {
            get
            {
                return this.v_JumpTurn;
            }

            set
            {
                v_JumpTurn = value;
            }
        }

        internal string Name
        {
            get
            {
                return this.m_PlayerName;
            }
            set
            {
                this.Name = value;
            }
        }

        internal eShapeType GetShapeType()
        {
            return this.m_Shape;
        }
    }
}

