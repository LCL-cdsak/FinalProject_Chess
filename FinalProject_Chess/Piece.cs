﻿using System;
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
        public bool[,] ValidPath(int row, int col, Piece[,] now_map)
        {
            bool[,] bool_map = new bool[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool_map[i, j] = false;
                }
            }
            switch (piece_type)
            {
                case PieceType.wPawn:
                    break;
                case PieceType.bPawn:
                    break;
                case PieceType.wRook:
                case PieceType.bRook:
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
                                bool_map[row, col + i] = true;
                                break;
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
                                bool_map[row, col - i] = true;
                                break;
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
                                bool_map[row - i, col] = true;
                                break;
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
                                bool_map[row + i, col] = true;
                                break;
                            }
                        }
                    }
                    break;
                case PieceType.wKnight:
                case PieceType.bKnight:
                    if (row - 2 >= 0 && col - 1 >= 0) bool_map[row - 2, col - 1] = true;
                    if (row - 1 >= 0 && col - 2 >= 0) bool_map[row - 1, col - 2] = true;
                    if (row + 1 < 8 && col - 2 >= 0) bool_map[row + 1, col - 2] = true;
                    if (row + 2 < 8 && col - 1 >= 0) bool_map[row + 2, col - 1] = true;
                    if (row + 2 < 8 && col + 1 < 8) bool_map[row + 2, col + 1] = true;
                    if (row + 1 < 8 && col + 2 < 8) bool_map[row + 1, col + 2] = true;
                    if (row - 1 >= 0 && col + 2 < 8) bool_map[row - 1, col + 2] = true;
                    if (row - 2 >= 0 && col + 1 < 8) bool_map[row - 2, col + 1] = true;
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
        public static PieceType PieceTypeFromString(string str)
        {
            return (PieceType)Enum.Parse(typeof(PieceType), str, true);
        }
        public static string PieceTypeToImagePath(PieceType type)
        {
            return type.ToString() + ".png";
        }
        public static PieceType PieceTypeFromChar(char c)
        {
            return PieceTypeFromString(Enum.GetName(typeof(PieceType), c));
        }
        public enum PieceType
        {
            wPawn = 'p',
            wRook = 'r',
            wKnight = 'h',
            wBishop = 'b',
            wQueen = 'q',
            wKing = 'k',
            bPawn = 'P',
            bRook = 'R',
            bKnight = 'H',
            bBishop = 'B',
            bQueen = 'Q',
            bKing = 'K'
        }
    }
}
