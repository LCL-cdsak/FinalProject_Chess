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

        public Dictionary<string, bool[]> team_castling = new Dictionary<string, bool[]>();

        public Dictionary<string, bool[,]> all_team_path = new Dictionary<string, bool[,]>();

        Dictionary<Piece, bool[,]> check_king_pieces = new Dictionary<Piece, bool[,]>(); // 儲存"非長直線"_"直接"_威脅國王之piece極其威脅路徑(Pawn, Knight, King)。
        Dictionary<Piece, bool[,]> path_check_king_pieces = new Dictionary<Piece, bool[,]>(); // 儲存"長直線"_"直接"_威脅國王棋之piece及其威脅路徑(所有長直線移動之棋)。
        Dictionary<Piece, bool[,]> protect_king_pieces = new Dictionary<Piece, bool[,]>(); // 儲存保王棋與敵方威脅路徑bool map(用作判定保王棋走位)。
        public Chess()
        {
            map = CreateChessMapFromChar(init_map);
            // Game Init
            InitChessGame();
            
        }

        public void InitChessGame()
        {
            current_team = "white";
            is_selected_piece = false;
            string[] team_names = { "white", "black" };

            foreach(string team_name in team_names)
            {
                team_castling[team_name] = new bool[2] { true, true};

                all_team_path[team_name] = new bool[8, 8];

                check_king_pieces.Clear();
                path_check_king_pieces.Clear();
                protect_king_pieces.Clear();
            }
            


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
    }
}