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
        public char[,] grid = new char[8, 8] {
                { 'r','h','b','q','k','b','h','r'},
                { 'p','p','p','p','p','p','p','p'},
                { 'n','n','n','n','n','n','n','n'},
                { 'n','n','n','n','n','n','n','n'},
                { 'n','n','n','n','n','n','n','n'},
                { 'n','n','n','n','n','n','n','n'},
                { 'P','P','P','P','P','P','P','P'},
                { 'R','H','B','Q','K','B','H','R'}
            };
        public PictureBox bknight = new PictureBox();
        public PictureBox bking = new PictureBox();
        public PictureBox bkueen = new PictureBox();
        public PictureBox bkishop = new PictureBox();
        public PictureBox bkook = new PictureBox();
        public PictureBox bpawn = new PictureBox();
        public Point[,] point = new Point[8, 8];
        public Panel table = new Panel();
        public bool _MouseDown = false;

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
            if (_MouseDown)
            {
                Point p = table.PointToClient(Cursor.Position);
                int x = p.X / 70;
                int y = p.Y / 70;
                bknight.Location = point[y, x];
                _MouseDown = false;
                bknight.BackColor = Color.Transparent;
            }

                //throw new NotImplementedException();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void initial()
        {//pieces
            bknight.Image = Image.FromFile("bKnight.png");
            bknight.SizeMode = PictureBoxSizeMode.StretchImage;
            bknight.Size = new Size(70, 70);
            table.Controls.Add(bknight);
            bknight.Location = point[0, 0];
            bknight.BringToFront();
            bknight.BackColor = Color.Transparent;
            bknight.MouseClick += new MouseEventHandler(knightclicked);
  
        }
        private void knightclicked(object sender, MouseEventArgs e)
        {
            if (_MouseDown)
            {
                _MouseDown = false;
                bknight.BackColor = Color.Transparent;
            }
            else
            {
                _MouseDown = true;
                bknight.BackColor = Color.LightBlue;
            }
            
        }
    }
}
