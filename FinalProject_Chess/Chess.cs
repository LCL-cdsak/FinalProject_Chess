using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Chess
{
    class Chess
    {

        public char[,] init_map = new char[8, 8] {
                { 'r','h','b','q','k','b','h','r'},
                { 'p','p','p','p','p','p','p','p'},
                { 'n','n','n','n','n','n','n','n'},
                { 'n','n','n','n','n','n','n','n'},
                { 'n','n','n','n','n','n','n','n'},
                { 'n','n','n','n','n','n','n','n'},
                { 'P','P','P','P','P','P','P','P'},
                { 'R','H','B','Q','K','B','H','R'}
            };
        public Piece[,] chess_map = new Piece[8, 8];

        Piece[,] CreateChessMapFromChar(char[,] char_table)
        {
            Piece[,] map = new Piece[8, 8];
            for (int row = 0; row < 8; ++row)
            {
                for (int col = 0; col < 8; ++col)
                {
                    if (char_table[row, col] == 'n')
                    {
                        map[row, col] = null;
                    }
                    else
                    {
                        map[row, col] = new Piece(Piece.PieceTypeFromChar(char_table[row, col]));
                    }
                }
            }
            return map;
        }
     
    }
}
