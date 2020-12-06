using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
namespace FinalProject_Chess
{
    public partial class Form1 : Form
    {
        Chess chess = new Chess();
        ChessAlgorithm algorithm = new ChessAlgorithm();
        public PictureBox[] pics = new PictureBox[32];//used to display pieces related to map
        public PictureBox piece;
        public Point[,] point = new Point[8, 8];// the coordinate of each block
        public Panel table = new Panel();


        public Form1()
        {
            InitializeComponent();
            //map
            table.BackgroundImageLayout = ImageLayout.Stretch;
            table.Size = new Size(560, 560);
            table.Location = new Point(50, 50);
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    point[i, j] = new Point(j * 70, i * 70);
            table.BackgroundImage = Image.FromFile("board.png");
            Controls.Add(table);
            table.MouseDown += Table_MouseDown;
            initial();
        }

        private void Table_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = table.PointToClient(Cursor.Position);
            int x = p.X / 70;
            int y = p.Y / 70;
            // MessageBox.Show("", $"{x} {y}", MessageBoxButtons.OK);
            if (chess.is_selected_piece)
            {
                // move the selected piece if path is valid
                if (!chess.MovePiece(y, x, out bool is_deselect))
                {
                    if (is_deselect)
                    {
                        // not only no move, set the piece to Transparent
                        piece.BackColor = Color.Transparent;
                        return;
                    }
                    return;
                }
                algorithm.AI(ref chess.map);
                for (int i = 0; i < 8; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < 8; j++)
                        if (chess.map[i, j] == null)
                            Console.Write("    n    ");
                        else
                        {
                            if (chess.map[i, j].team == "white")
                                switch (chess.map[i, j].piece_type)
                                {
                                    case Piece.PieceType.Bishop:
                                        Console.Write("Wbishop  ");
                                        break;
                                    case Piece.PieceType.King:
                                        Console.Write("Wking    ");
                                        break;
                                    case Piece.PieceType.Knight:
                                        Console.Write("WKnight  ");
                                        break;
                                    case Piece.PieceType.Pawn:
                                        Console.Write("WPawn    ");
                                        break;
                                    case Piece.PieceType.Queen:
                                        Console.Write("WQueen   ");
                                        break;
                                    case Piece.PieceType.Rook:
                                        Console.Write("WRook    ");
                                        break;

                                }
                            else
                                switch (chess.map[i, j].piece_type)
                                {
                                    case Piece.PieceType.Bishop:
                                        Console.Write("Bbishop  ");
                                        break;
                                    case Piece.PieceType.King:
                                        Console.Write("Bking    ");
                                        break;
                                    case Piece.PieceType.Knight:
                                        Console.Write("BKnight  ");
                                        break;
                                    case Piece.PieceType.Pawn:
                                        Console.Write("BPawn    ");
                                        break;
                                    case Piece.PieceType.Queen:
                                        Console.Write("BQueen   ");
                                        break;
                                    case Piece.PieceType.Rook:
                                        Console.Write("BRook    ");
                                        break;

                                }
                        }
                }
                Console.WriteLine();
                // the piece is now moved to new location
                piece.Location = point[y, x];
                piece.BackColor = Color.Transparent;
            }
            else
            {
                // select the piece if player valid
                if (chess.SelectPiece(y, x))
                {
                    piece = pics[GetPictureBoxIndexFromLocation(y, x)];
                    piece.BackColor = Color.LightBlue;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void initial()
        {
            for (int i = 0; i < 32; i++)
            {
                pics[i] = new PictureBox();
                table.Controls.Add(pics[i]);
                pics[i].Size = new Size(70, 70);
                pics[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pics[i].BringToFront();
                pics[i].BackColor = Color.Transparent;
                pics[i].MouseDown += Table_MouseDown;
            }
            int c = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pics[c++].Image = Image.FromFile(Piece.PieceToImagePath(chess.map[i, j]));
                    pics[c - 1].Location = point[i, j];
                }
            }
            for (int i = 6; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pics[c++].Image = Image.FromFile(Piece.PieceToImagePath(chess.map[i, j]));
                    pics[c - 1].Location = point[i, j];
                }
            }

        }
        public int GetPictureBoxIndexFromLocation(int row, int col)
        {
            for (int i = 0; i < 32; ++i)
            {
                if (pics[i].Location == point[row, col])
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
