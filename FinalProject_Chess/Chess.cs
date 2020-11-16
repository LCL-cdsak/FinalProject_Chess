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
        public Piece[,] map = new Piece[8, 8];
        public Chess()//constructor
        {
            for(int i=0;i<8;i++)
                for(int j=0;j<8;j++)
                {
                    
                    switch (init_map[i,j])
                    {
                        case 'r':
                            map[i, j] = new Piece("bRook");
                            map[i, j].piece_type = Piece.PieceType.bRook;
                            break;
                        case 'h':
                            map[i, j] = new Piece("bKnight");
                            map[i, j].piece_type = Piece.PieceType.bKnight;
                            break;
                        case 'b':
                            map[i, j] = new Piece("bBishop");
                            map[i, j].piece_type = Piece.PieceType.bBishop;
                            break;
                        case 'q':
                            map[i, j] = new Piece("bQueen");
                            map[i, j].piece_type = Piece.PieceType.bQueen;
                            break;
                        case 'k':
                            map[i, j] = new Piece("bKing");
                            map[i, j].piece_type = Piece.PieceType.bKing;
                            break;
                        case 'p':
                            map[i, j] = new Piece("bPawn");
                            map[i, j].piece_type = Piece.PieceType.bPawn;
                            break;
                        case 'R':
                            map[i, j] = new Piece("wRook");
                            map[i, j].piece_type = Piece.PieceType.wRook;
                            break;
                        case 'H':
                            map[i, j] = new Piece("wKnight");
                            map[i, j].piece_type = Piece.PieceType.wKnight;
                            break;
                        case 'B':
                            map[i, j] = new Piece("wBishop");
                            map[i, j].piece_type = Piece.PieceType.wBishop;
                            break;
                        case 'Q':
                            map[i, j] = new Piece("wQueen");
                            map[i, j].piece_type = Piece.PieceType.wQueen;
                            break;
                        case 'K':
                            map[i, j] = new Piece("wKing");
                            map[i, j].piece_type = Piece.PieceType.wKing;
                            break;
                        case 'P':
                            map[i, j] = new Piece("wPawn");
                            map[i, j].piece_type = Piece.PieceType.wPawn;
                            break;
                        default:
                            map[i, j] = null;
                            break;
                    }
                }
        }
        public bool[,] T(int row, int col)
        {
            Piece test = new Piece(Piece.PieceType.bKnight);
            return test.ValidPath(row, col, map);
        }
    }
}
