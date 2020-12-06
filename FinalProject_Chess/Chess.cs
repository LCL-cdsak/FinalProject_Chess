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
        public Dictionary<string, int[]> king_piece_locations = new Dictionary<string, int[]>();
        public Dictionary<string, Piece> king_pieces = new Dictionary<string, Piece>();
        public Piece protect_piece = null;
        public bool map_NotNull=false;
        // game status
        private string current_team = null;
        public bool is_selected_piece = false; // true when player has seleted a piece
        public int[] selected_piece_location = new int[2]; // row, col (not x, y)

        public Dictionary<string, bool[]> team_castling = new Dictionary<string, bool[]>();
        public Dictionary<string, bool[,]> all_team_path = new Dictionary<string, bool[,]>();
        public bool is_check, king_cant_move, must_move_king, is_gameover = false;
        public bool[,] check_path = null; // store the king check path.
        public List<Piece> protect_pieces = new List<Piece>();

        //Dictionary<Piece, bool[,]> check_king_pieces = new Dictionary<Piece, bool[,]>(); // 儲存"非長直線"_"直接"_威脅國王之piece極其威脅路徑(Pawn, Knight, King)。
        //Dictionary<Piece, bool[,]> path_check_king_pieces = new Dictionary<Piece, bool[,]>(); // 儲存"長直線"_"直接"_威脅國王棋之piece及其威脅路徑(所有長直線移動之棋)。
        //Dictionary<Piece, bool[,]> protect_king_pieces = new Dictionary<Piece, bool[,]>(); // 儲存保王棋與敵方威脅路徑bool map(用作判定保王棋走位)。

        public Chess()
        {
            map = CreateChessMapFromChar(init_map);
            // Game Init
            InitChessGame();
            
        }

        public void InitChessGame()
        {
            is_gameover = false;
            current_team = "white";
            is_selected_piece = false;
            string[] team_names = { "white", "black" };

            foreach(string team_name in team_names)
            {
                team_castling[team_name] = new bool[2] { true, true};

                all_team_path[team_name] = new bool[8, 8];

                check_path = null;

                //check_king_pieces.Clear();
                //path_check_king_pieces.Clear();
                //protect_king_pieces.Clear();
            }

            RoundInitialize();

        }
        public void RoundInitialize()
        {


            UpdateValidPath(); // main process
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
                        if(chess_map[row, col].piece_type == Piece.PieceType.King)
                        {
                            king_piece_locations[chess_map[row, col].team] = new int[2] { row, col };
                            king_pieces[chess_map[row, col].team] = chess_map[row, col];
                        }
                    }
                }
            }
            return chess_map;
        }
        public bool[,] ValidPath(int row, int col)
        {
            // wrap the Piece.ValidPath, no need for arg "map")
            //return map[row, col].ValidPath(row, col, map);
            if (must_move_king)
            {
                if (map[row, col].piece_type != Piece.PieceType.King)
                    return null;
            }
            return map[row, col].valid_path;
        }
        
        public void UpdateValidPath()
        {
            Console.WriteLine("UpdateValidPath");
            is_check = false;
            check_path = null;

            // Init Piece Round
            for (int i=0; i<8; ++i)
            {
                for(int k=0; k<8; ++k)
                {
                    if(map[i, k] != null)
                    {
                        map[i, k].RoundInitialize();
                    }
                }
            }
            protect_piece = null;
            Piece temp_piece;
            protect_pieces.Clear();
            // Create thread_paths, and add to the protecting_piece, set is_check
            for (int row = 0; row < 8; ++row)
            {
                for(int col = 0; col < 8; ++col)
                {
                    if(map[row,col] != null)
                    {
                        temp_piece = map[row, col].Thread_path(row, col, map, ref is_check, ref check_path);// if is_check, check_path will be set
                        //Console.WriteLine($"{map[row, col].team} {map[row, col].piece_type.ToString()} {is_check}");
                        if (temp_piece != null)
                        {
                            // not check, but protecting
                            if(temp_piece.team == current_team)
                                protect_pieces.Add(temp_piece);
                        }
                    }
                    
                }
            }

            // Create valid_path, and do AND with protect_path
            for(int row=0; row<8; ++row)
            {
                for(int col=0; col<8; ++col)
                {
                    if (map[row, col] != null)
                    {
                        if(map[row, col].team == current_team)
                            map[row, col].valid_path = map[row, col].ValidPath(row, col, map);
                    }
                }
            }
            Console.WriteLine($"Protect_pieces count = {protect_pieces.Count()}");
            for(int i=0; i<protect_pieces.Count(); ++i)
            {
                AndChessBoolMap(protect_pieces[i].valid_path, protect_pieces[i].protect_path);
            }

            // Create all_team_path, and determine king.valid_path, king_cant_move
            all_team_path["white"] = new bool[8, 8];
            all_team_path["black"] = new bool[8, 8];
            string enemy_team = (current_team == "white") ? "black" : "white";
            for(int row=0; row<8; ++row)
            {
                for(int col=0; col<8; ++col)
                {
                    if (map[row, col] != null)
                    {
                        if(map[row, col].team == enemy_team)
                        {
                            map[row, col].Team_path(row, col, map, all_team_path[enemy_team]);
                        }
                    }
                }
            }

            // Now, the valid path for all piece is complete

            // If is_check is true, check if able to protect king, 
            // Do AND to all threaded(current team) team with the thread_path (Thus, the piece which is not candidate will have a full false valid_path)
            bool is_candidate_exists = false, temp_bool;
            if (is_check)
            {
                MessageBox.Show("Check");
                for(int row=0; row<8; ++row)
                {
                    for(int col=0; col<8; ++col)
                    {
                        if(map[row, col] != null)
                        {
                            if(map[row, col].team == current_team && map[row, col].piece_type != Piece.PieceType.King)
                            {
                                temp_bool = AndChessBoolMap(map[row, col].valid_path, check_path);
                                if (temp_bool)
                                    is_candidate_exists = true;
                            }
                        }
                    }
                }
            }

            king_cant_move = AndChessKingBoolMap(king_piece_locations[current_team][0], king_piece_locations[current_team][1],
                                                 king_pieces[current_team].valid_path, all_team_path[enemy_team]);

            // Determine gameover-condition(king_cant_move & no candidate to protect king)
            must_move_king = false;
            if (is_check)
            {
                if (is_candidate_exists)
                {
                    // just use the valid_path result.
                    MessageBox.Show("candidate exists");
                }
                else
                {
                    // king must move
                    if (king_cant_move)
                    {
                        // game over
                        is_gameover = true;
                        MessageBox.Show("Game over");
                    }
                    else
                    {
                        // must move king, ValidPath need to use 
                        must_move_king = true;
                        MessageBox.Show("Must move king");
                    }
                }
            }

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
            Console.WriteLine($"Move {selected_piece_location[0]} {selected_piece_location[1]}, {row} {col}");
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
            if(ValidPath(selected_piece_location[0], selected_piece_location[1]) == null)
            {
                MessageBox.Show("King Check", "NO", MessageBoxButtons.OK);
                // not a valid path
                return false;
            }
            if (!ValidPath(selected_piece_location[0], selected_piece_location[1])[row, col])
            {
                MessageBox.Show("NO", "NO", MessageBoxButtons.OK);
                // not a valid path
                return false;
            }
            // valid path
            is_selected_piece = false;
            if (map[row, col] != null)
            {
                map_NotNull = true;
            }
            else
            {
                map_NotNull = false;
            }
            RemovePiece(row, col);
            map[row, col] = map[selected_piece_location[0], selected_piece_location[1]];
            map[selected_piece_location[0], selected_piece_location[1]] = null;
            if (map[row, col].piece_type == Piece.PieceType.King)
            {
                king_piece_locations[map[row, col].team][0] = row;
                king_piece_locations[map[row, col].team][1] = col;
            }
         current_team = (current_team == "white") ? "black" : "white";
            RoundInitialize();
            return true;
        }
        public void RemovePiece(int row, int col)
        {
            // Add stuff need to be clean here
            map[row, col] = null;
        }
        public static bool AndChessBoolMap(bool[,] a, bool[,] b)
            // The return value is true when at least one a&&b == true.
        {
            bool result = false;
            for(int row=0; row<8; ++row)
            {
                for(int col=0; col<8; ++col)
                {
                    if (a[row, col] && b[row, col])
                        result = true;
                    else
                        a[row, col] &= b[row, col];
                }
            }
            return result;
        }
        public static bool AndChessKingBoolMap(int row, int col, bool[,] king_valid_path, bool[,] all_team_path)
        {
            bool is_king_cant_move = true;
            int irow, icol;
            for(int i=0; i<8; ++i)
            {
                irow = row + Piece.king_offsets[i, 0];
                icol = col + Piece.king_offsets[i, 1];
                if(irow >=0 && irow <8 && icol >= 0 && icol < 8)
                {
                    if(king_valid_path[irow, icol])
                    {
                        if(all_team_path[irow, icol])
                        {
                            // danger location
                            king_valid_path[irow, icol] = false;
                        }
                        else
                        {
                            is_king_cant_move = false;
                        }
                    }
                }
            }
            return is_king_cant_move;
        }
    }
}