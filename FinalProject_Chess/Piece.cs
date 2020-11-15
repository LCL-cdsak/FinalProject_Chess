using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Chess
{
    class Piece
    {
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
        enum PieceType
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
