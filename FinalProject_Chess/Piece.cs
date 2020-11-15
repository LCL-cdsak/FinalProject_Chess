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
        public Piece(string piece_type_name)
        {
            if (!Enum.TryParse(piece_type_name, true, out piece_type))
                // no match Enum type
                Console.WriteLine("bug");
        }
        public Piece(PieceType type)
        {
            piece_type = type;
        }
        public bool[,] ValidPath()
        {
            bool[,] bool_map = new bool[8, 8];
            switch(piece_type){
                case PieceType.wPawn:
                case PieceType.bPawn:
                    break;
                case PieceType.wRook:
                case PieceType.bRook:
                    break;
                case PieceType.wKnight:
                case PieceType.bKnight:
                    break;
            }
            return bool_map;
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
        static PieceType PieceTypeFromString(string str)
        {
            return (PieceType)Enum.Parse(typeof(PieceType), str, true);
        }
        static string PieceTypeToImagePath(PieceType type)
        {
            return type.ToString() + ".png";
        }
        public enum PieceType
        {
            wPawn,
            wRook,
            wKnight,
            wBishop,
            wQueen,
            wKing,
            bPawn,
            bRook,
            bKnight,
            bBishop,
            bQueen,
            bKing
        }
    }

}
