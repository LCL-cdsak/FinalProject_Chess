using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Chess
{
    class Chess
    {
        Piece[,] chess_table = new Piece[8, 8];
        Piece[,] CreateChessTableFromChar(char[,] char_table)
        {
            Piece[,] table = new Piece[8, 8];
            for(int row=0; row<8; ++row)
            {
                for(int col=0; col<8; ++col)
                {
                    if (char_table[row, col] == 'n')
                    {
                        table[row, col] = null;
                    }
                    else
                    {
                        table[row, col] = new Piece(Piece.PieceTypeFromChar(char_table[row, col]));
                    }
                }
            }
            return table;
        }
    }
}
