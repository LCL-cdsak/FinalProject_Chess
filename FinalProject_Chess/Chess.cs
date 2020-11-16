using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Chess
{
    class Chess
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
        public Piece[,] map = new Piece[8, 8];
        public void initial()
        {
            for(int i=0;i<8;i++)
                for(int j=0;j<8;j++)
                {
                    char flag = grid[i,j];
                    switch (flag)
                    {
                        case 'r':
                            //map[i,j].piece_type.
                            break;
                    }
                }
        }
    }
}
