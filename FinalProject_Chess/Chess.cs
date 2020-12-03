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
                return (selected_piece_location[0] == row) && (selected_piece_location[1] == col);
            }
            return false;
        }
        public bool SelectPiece(int row, int col)
        {
            if (map[row, col] == null)
                return false;
            if (map[row, col].team == current_team)
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
        public bool MovePiece(int row, int col, out bool is_deselect)
        {
            if (IsDeselect(row, col))
            {
                // player want to deselect the piece, let it be a false move
                is_selected_piece = false;
                is_deselect = true;
                return false;
            }
            else
            {
                is_deselect = false;
            }

            if (!is_selected_piece)
            {
                // no piece selected
                return false;
            }
            if (!ValidPath(selected_piece_location[0], selected_piece_location[1])[row, col])
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
        /*
         * score:
         * pawn = 1
         * knight = 4
         * bishop = 5(if two bishops are alive,both plus 0.5)
         * rook = 6
         * queen = 11
         * king = 1000
         */
        public int calculate_point(Piece[,] current, int i, int j)
        {
            switch (current[i, j].piece_type)
            {
                case Piece.PieceType.Queen:
                    return 11;
                case Piece.PieceType.Rook:
                    return 6;
                case Piece.PieceType.Pawn:
                    return 1;
                case Piece.PieceType.Knight:
                    return 4;
                case Piece.PieceType.Bishop:
                    return 5;
                case Piece.PieceType.King:
                    return 1000;
                default:
                    return 0;
            }

        }
        public int minmax(Piece[,] current, int point, int layer)//current is map status，return point of a match
        {
            if (layer < 0)
                return point;
            Piece temp;
            Piece temp2;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    //for loop all valid path，calculate the points(max and min)
                    if (current[i, j] == null)
                        continue;
                    else if (current[i, j].team == "black")//set com as black temporary
                    {
                        bool[,] paths = current[i, j].ValidPath(i, j, current);//get all valid paths of this piece
                        for (int m = 0; m < 8; m++)
                            for (int n = 0; n < 8; n++)
                                if (paths[m, n])
                                {
                                    //calculate points and recur to next level
                                    int temp_score = calculate_point(current, m, n);//calculate how many score can get if go to [m,n]
                                    //*******moving piece from [i,j] to [m,n]*******
                                    temp = current[m, n];
                                    current[m, n] = current[i, j];
                                    current[i, j] = null;
                                    //**********************************************
                                    int max = 0;
                                    for (int a = 0; a < 8; a++)
                                        for (int b = 0; b < 8; b++)
                                        {
                                            if (current[a, b] == null)
                                                continue;
                                            else if (current[a, b].team == "white")
                                            {
                                                bool[,] enemy_paths = current[a, b].ValidPath(a, b, current);
                                                for (int c = 0; c < 8; c++)
                                                    for (int d = 0; d < 8; d++)
                                                        if (enemy_paths[c, d])
                                                        {
                                                            int temp_score_2 = calculate_point(current, c, d);
                                                            //*******moving piece from [a,b] to [c,d]*******
                                                            temp2 = current[c, d];
                                                            current[c, d] = current[a, b];
                                                            current[a, b] = null;
                                                            //**********************************************
                                                            int highest_score = minmax(current, point + temp_score - temp_score_2, layer - 1);
                                                            point = highest_score > point ? highest_score : point;
                                                            //*******recover map to initial status**********
                                                            current[a, b] = current[c, d];
                                                            current[c, d] = temp2;
                                                            //**********************************************
                                                        }
                                            }
                                        }
                                    //*******recover map to initial status**********
                                    current[i, j] = current[m, n];
                                    current[m, n] = temp;
                                    //**********************************************
                                }
                    }
                }
            return point;
        }
    }
}
