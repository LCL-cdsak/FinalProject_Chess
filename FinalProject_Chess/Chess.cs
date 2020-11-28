using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

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
        public Piece[,] map = new Piece[8, 8];

        // game status
        private string current_team = null;
        public bool is_selected_piece = false; // true when player has seleted a piece
        public int[] selected_piece_location = new int[2]; // row, col (not x, y)

        public Chess()
        {
            map = CreateChessMapFromChar(init_map);

            // Game Init
            current_team = "white";
            is_selected_piece = false;
        }

        Piece[,] CreateChessMapFromChar(char[,] char_table)
        {
            Piece[,] chess_map = new Piece[8, 8];
            for (int row = 0; row < 8; ++row)
            {
                for (int col = 0; col < 8; ++col)
                {
                    if (char_table[row, col] == 'n')
                    {
                        chess_map[row, col] = null;
                    }
                    else
                    {
                        chess_map[row, col] = Piece.PieceFromChar(char_table[row, col]);
                    }
                }
            }
            return chess_map;
        }
           
        public bool[,] ValidPath(int row, int col)
        {
            // wrap the Piece.ValidPath, no need for arg "map")
            return map[row, col].ValidPath(row, col, map);
        }
        public bool IsDeselect(int row, int col)
        {
            // check if player want to deselected the piece
            if (is_selected_piece)
            {
                return selected_piece_location[0] == row && selected_piece_location[1] == col;
            }
            return false;
        }
        public bool SelectPiece(int row, int col)
        {
            if (map[row, col] == null)
                return false;
            if(map[row, col].team == current_team)
            {
                is_selected_piece = true;
                selected_piece_location[0] = row;
                selected_piece_location[1] = col;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool MovePiece(int row, int col)
        {
            if (!is_selected_piece)
            {
                // no piece selected
                return false;
            }
            if(!ValidPath(selected_piece_location[0], selected_piece_location[1])[row, col])
            {
                MessageBox.Show("NO", "NO", MessageBoxButtons.OK);
                // not a valid path
                return false;
            }
            is_selected_piece = false;
            map[row, col] = map[selected_piece_location[0], selected_piece_location[1]];
            map[selected_piece_location[0], selected_piece_location[1]] = null;
            return true;
        }
    }
}
