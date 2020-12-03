using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Chess
{
    class Piece
    {
        public PieceType piece_type;
        public string team;
        public Piece(string _team, string piece_type_name)
        {
            if (!Enum.TryParse(piece_type_name, true, out piece_type))
                // no match Enum type
                Console.WriteLine("bug");
            team = _team;
        }
        public Piece(string _team, PieceType _piece_type)
        {
            piece_type = _piece_type;
            team = _team;
        }

        public bool[,] ValidPath(int row, int col,Piece[,] now_map)
        {
            bool[,] bool_map = new bool[8, 8];
               for(int i=0;i<8;i++){
                        for(int j=0;j<8;j++){
                            bool_map[i,j]=false;
                        }
                    }

            switch(piece_type){
                case PieceType.Pawn:
                    if(team == "black")
                    {
                        if (row + 1 <8)
                        {
                            if (now_map[row + 1, col] == null)
                            {
                                bool_map[row + 1, col] = true;
                            }
                            if (col + 1 < 8)
                            {
                                if (now_map[row + 1, col + 1] != null)
                                {
                                    if (now_map[row + 1, col + 1].team == "white") bool_map[row + 1, col + 1] = true;
                                }
                            }
                            if (col - 1 > 0)
                            {
                                if (now_map[row + 1, col - 1] != null)
                                {
                                    if (now_map[row + 1, col - 1].team == "white") bool_map[row + 1, col - 1] = true;
                                }
                            }
                        }
                        if (row == 1 && now_map[row + 2, col] == null)
                        {
                            bool_map[row + 2, col] = true;
                        }
                        break;
                    }
                    else if(team == "white")
                    {
                        if (row - 1 > 0)
                        {
                            if (now_map[row - 1, col] == null)
                            {
                                bool_map[row - 1, col] = true;
                            }
                            if (col + 1 < 8)
                            {
                                if (now_map[row - 1, col + 1] != null)
                                {
                                    if (now_map[row - 1, col + 1].team == "black") bool_map[row - 1, col + 1] = true;
                                }
                            }
                            if (col - 1 > 0)
                            {
                                if (now_map[row - 1, col - 1] != null)
                                {
                                    if (now_map[row - 1, col - 1].team == "black") bool_map[row - 1, col - 1] = true;
                                }
                            }
                        }
                        if (row ==6 &&now_map[row-2,col]==null)
                        {
                            bool_map[row - 2, col] = true;
                        }
                        break;
                    }
                    break;

                case PieceType.King:
                    if (team == "black")
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (row + i >= 0 && row + i < 8 && col + j >= 0 && col + j < 8)
                                {
                                    if (now_map[row + i, col + j] == null)
                                    {
                                        bool_map[row + i, col + j] = true;
                                    }
                                    else
                                    {
                                        if (now_map[row + i, col + j].team != "black") { bool_map[row + i, col + j] = true; }

                                    }
                                }
                            }
                        }
                    }
                    else if (team == "white")
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (row + i >= 0 && row + i < 8 && col + j >= 0 && col + j < 8 )
                                {
                                    if(now_map[row + i, col + j] == null)
                                    {
                                        bool_map[row + i, col + j] = true;
                                    }
                                    else 
                                    {
                                        if (now_map[row + i, col + j].team != "white") { bool_map[row + i, col + j] = true; }

                                    }
                                }
                            }
                        }
                    }
                    break;

                case PieceType.Queen:
                    Diagonal_path(row, col, now_map, bool_map);
                    Cross_path(row, col, now_map, bool_map);
                    break;

                case PieceType.Bishop:
                    Diagonal_path(row, col, now_map, bool_map);
                    break;

                case PieceType.Rook:
                    Cross_path(row, col, now_map, bool_map);
                    break;
                    
                case PieceType.Knight:
                    if (team == "white")
                    {
                        if (row - 2 >= 0 && col - 1 >= 0)
                        {
                            if(now_map[row - 2, col - 1]==null)bool_map[row - 2, col - 1] = true;
                            else
                            {
                                if(now_map[row - 2, col - 1].team!="white") bool_map[row - 2, col - 1] = true; ;
                            }
                        }
                        if (row - 1 >= 0 && col - 2 >= 0)
                        {
                            if(now_map[row - 1, col - 2]==null)bool_map[row - 1, col - 2] = true;
                            else
                            {
                                if(now_map[row - 1, col - 2].team!="white") bool_map[row - 1, col - 2] = true; ;
                            }
                        }
                        if (row + 1 < 8 && col - 2 >= 0)
                        {
                            if(now_map[row + 1, col - 2]==null)bool_map[row + 1, col - 2] = true;
                            else
                            {
                                if(now_map[row + 1, col - 2].team!="white") bool_map[row + 1, col - 2] = true;
                            }
                        }
                        if (row + 2 < 8 && col - 1 >= 0)
                        {
                           if(now_map[row + 2, col - 1]==null)bool_map[row + 2, col - 1] = true;
                           else
                            {
                                if(now_map[row + 2, col - 1].team!="white") bool_map[row + 2, col - 1] = true;
                            }
                        }
                        if (row + 2 < 8 && col + 1 < 8)
                        {
                           if(now_map[row + 2, col + 1]==null)bool_map[row + 2, col + 1] = true;
                            else
                            {
                                if(now_map[row + 2, col + 1].team!="white") bool_map[row + 2, col + 1] = true;
                            }
                        }
                        if (row + 1 < 8 && col + 2 < 8)
                        {
                            if(now_map[row + 1, col + 2]==null)bool_map[row + 1, col + 2] = true;
                            else
                            {
                                if(now_map[row + 1, col + 2].team!="white") bool_map[row + 1, col + 2] = true;
                            }
                        }
                        if (row - 1 >= 0 && col + 2 < 8)
                        {
                           if(now_map[row - 1, col + 2]==null)bool_map[row - 1, col + 2] = true;
                           else
                            {
                                if(now_map[row - 1, col + 2].team!="white") bool_map[row - 1, col + 2] = true;
                            }
                        }
                        if (row - 2 >= 0 && col + 1 < 8)
                        {
                            if(now_map[row - 2, col + 1]==null)bool_map[row - 2, col + 1] = true;
                            else
                            {
                                if(now_map[row - 2, col + 1].team!="white") bool_map[row - 2, col + 1] = true;
                            }
                        }
                    }
                    else if (team == "black")
                    {
                        if (row - 2 >= 0 && col - 1 >= 0)
                        {
                            if (now_map[row - 2, col - 1] == null) bool_map[row - 2, col - 1] = true;
                            else
                            {
                                if (now_map[row - 2, col - 1].team != "black") bool_map[row - 2, col - 1] = true; ;
                            }
                        }
                        if (row - 1 >= 0 && col - 2 >= 0)
                        {
                            if (now_map[row - 1, col - 2] == null) bool_map[row - 1, col - 2] = true;
                            else
                            {
                                if (now_map[row - 1, col - 2].team != "black") bool_map[row - 1, col - 2] = true; ;
                            }
                        }
                        if (row + 1 < 8 && col - 2 >= 0)
                        {
                            if (now_map[row + 1, col - 2] == null) bool_map[row + 1, col - 2] = true;
                            else
                            {
                                if (now_map[row + 1, col - 2].team != "black") bool_map[row + 1, col - 2] = true;
                            }
                        }
                        if (row + 2 < 8 && col - 1 >= 0)
                        {
                            if (now_map[row + 2, col - 1] == null) bool_map[row + 2, col - 1] = true;
                            else
                            {
                                if (now_map[row + 2, col - 1].team != "black") bool_map[row + 2, col - 1] = true;
                            }
                        }
                        if (row + 2 < 8 && col + 1 < 8)
                        {
                            if (now_map[row + 2, col + 1] == null) bool_map[row + 2, col + 1] = true;
                            else
                            {
                                if (now_map[row + 2, col + 1].team != "black") bool_map[row + 2, col + 1] = true;
                            }
                        }
                        if (row + 1 < 8 && col + 2 < 8)
                        {
                            if (now_map[row + 1, col + 2] == null) bool_map[row + 1, col + 2] = true;
                            else
                            {
                                if (now_map[row + 1, col + 2].team != "black") bool_map[row + 1, col + 2] = true;
                            }
                        }
                        if (row - 1 >= 0 && col + 2 < 8)
                        {
                            if (now_map[row - 1, col + 2] == null) bool_map[row - 1, col + 2] = true;
                            else
                            {
                                if (now_map[row - 1, col + 2].team != "black") bool_map[row - 1, col + 2] = true;
                            }
                        }
                        if (row - 2 >= 0 && col + 1 < 8)
                        {
                            if (now_map[row - 2, col + 1] == null) bool_map[row - 2, col + 1] = true;
                            else
                            {
                                if (now_map[row - 2, col + 1].team != "black ") bool_map[row - 2, col + 1] = true;
                            }
                        }
                    }
                    break;
            }
            return bool_map;
        }
        private void Cross_path(int row,int col,Piece[,] now_map, bool[,] bool_map)//判斷十字路徑
        {
            if (team == "white")
            {
                for (int i = 1; i < 8; i++)
                {
                    if (col + i < 8)
                    {
                        if (now_map[row, col + i] == null)
                        {
                            bool_map[row, col + i] = true;
                        }
                        else
                        {
                            if (now_map[row, col + i].team == "black")
                            {
                                bool_map[row, col + i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (col - i >= 0)
                    {
                        if (now_map[row, col - i] == null)
                        {
                            bool_map[row, col - i] = true;
                        }
                        else
                        {
                            if (now_map[row, col - i].team == "black")
                            {
                                bool_map[row, col - i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row - i >= 0)
                    {
                        if (now_map[row - i, col] == null)
                        {
                            bool_map[row - i, col] = true;
                        }
                        else
                        {
                            if (now_map[row - i, col].team == "black")
                            {
                                bool_map[row - i, col] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row + i < 8)
                    {
                        if (now_map[row + i, col] == null)
                        {
                            bool_map[row + i, col] = true;
                        }
                        else
                        {
                            if (now_map[row + i, col].team == "black")
                            {
                                bool_map[row + i, col] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else if (team == "black")
            {
                for (int i = 1; i < 8; i++)
                {
                    if (col + i < 8)
                    {
                        if (now_map[row, col + i] == null)
                        {
                            bool_map[row, col + i] = true;
                        }
                        else
                        {
                            if (now_map[row, col + i].team == "white")
                            {
                                bool_map[row, col + i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    if (col - i >= 0)
                    {
                        if (now_map[row, col - i] == null)
                        {
                            bool_map[row, col - i] = true;
                        }
                        else
                        {
                            if (now_map[row, col - i].team == "white")
                            {
                                bool_map[row, col - i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    if (row - i >= 0)
                    {
                        if (now_map[row - i, col] == null)
                        {
                            bool_map[row - i, col] = true;
                        }
                        else
                        {
                            if (now_map[row - i, col].team == "white")
                            {
                                bool_map[row - i, col] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    if (row + i < 8)
                    {
                        if (now_map[row + i, col] == null)
                        {
                            bool_map[row + i, col] = true;
                        }
                        else
                        {
                            if (now_map[row + i, col].team == "white")
                            {
                                bool_map[row + i, col] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return;
        }
        public void Diagonal_path(int row, int col, Piece[,] now_map, bool[,] bool_map)//判斷對角路徑
        {
            if (team == "white")
            {
                for (int i = 1; i < 8; i++)
                {
                    if (row + i < 8 && col + i < 8)
                    {
                        if (now_map[row + i, col + i] == null)
                        {
                            bool_map[row + i, col + i] = true;
                        }
                        else
                        {
                            if (now_map[row + i, col + i].team == "black")
                            {
                                bool_map[row + i, col + i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row - i > 0 && col + i < 8)
                    {
                        if (now_map[row - i, col + i] == null)
                        {
                            bool_map[row - i, col + i] = true;
                        }
                        else
                        {
                            if (now_map[row - i, col + i].team == "black")
                            {
                                bool_map[row - i, col + i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row + i < 8 && col - i > 0)
                    {
                        if (now_map[row + i, col - i] == null)
                        {
                            bool_map[row + i, col - i] = true;
                        }
                        else
                        {
                            if (now_map[row + i, col - i].team == "black")
                            {
                                bool_map[row + i, col - i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row - i > 0 && col - i > 0)
                    {
                        if (now_map[row - i, col - i] == null)
                        {
                            bool_map[row - i, col - i] = true;
                        }
                        else
                        {
                            if (now_map[row - i, col - i].team == "black")
                            {
                                bool_map[row - i, col - i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (team == "black")
            {
                for (int i = 1; i < 8; i++)
                {
                    if (row + i < 8 && col + i < 8)
                    {
                        if (now_map[row + i, col + i] == null)
                        {
                            bool_map[row + i, col + i] = true;
                        }
                        else
                        {
                            if (now_map[row + i, col + i].team == "white")
                            {
                                bool_map[row + i, col + i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row - i > 0 && col + i < 8)
                    {
                        if (now_map[row - i, col + i] == null)
                        {
                            bool_map[row - i, col + i] = true;
                        }
                        else
                        {
                            if (now_map[row - i, col + i].team == "white")
                            {
                                bool_map[row - i, col + i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row + i < 8 && col - i > 0)
                    {
                        if (now_map[row + i, col - i] == null)
                        {
                            bool_map[row + i, col - i] = true;
                        }
                        else
                        {
                            if (now_map[row + i, col - i].team == "white")
                            {
                                bool_map[row + i, col - i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < 8; i++)
                {
                    if (row - i > 0 && col - i > 0)
                    {
                        if (now_map[row - i, col - i] == null)
                        {
                            bool_map[row - i, col - i] = true;
                        }
                        else
                        {
                            if (now_map[row - i, col - i].team == "white")
                            {
                                bool_map[row - i, col - i] = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /*
         * 白方:
         * p:士兵 
         * r:城堡
         * h:騎士 
         * b:主教 
         * q:皇后 
         * k:國王 
         * 黑方:
         * P:士兵
         * R:城堡
         * H:騎士
         * B:主教
         * Q:皇后
         * K:國王 
         * 
         * N:空格
         */
        public static PieceType PieceTypeFromString(string str)
        {
            return (PieceType)Enum.Parse(typeof(PieceType), str, true);
        }
        public static string PieceToImagePath(Piece piece)
        {
            switch (piece.team)
            {
                case "white":
                    return "w" + piece.piece_type.ToString() + ".png";
                case "black":
                    return "b" + piece.piece_type.ToString() + ".png";
                default:
                    return "NULL";
            }
                
        }
        public static Piece PieceFromChar(char c)
        {
            if (char.IsUpper(c))
            {
                return new Piece("white", Enum.GetName(typeof(PieceType), char.ToUpper(c)));
            }
            else
            {
                return new Piece("black", Enum.GetName(typeof(PieceType), char.ToUpper(c)));
            }
            
        }

        public enum PieceType
        {
            Pawn='P',
            Rook='R',
            Knight='H',
            Bishop='B',
            Queen='Q',
            King='K'
        }
    }
}
