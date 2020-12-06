using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Chess
{
    class ChessAlgorithm
    {
        /*
  * score:
  * pawn = 1
  * knight = 4
  * bishop = 5(if two bishops are alive,both plus 0.5)
  * rook = 6
  * queen = 11
  * king = 1000
  */
        public int calculate_point(ref Piece[,] current, int i, int j)
        {
            if (current[i, j] == null)
            {
                return 0;
            }
            else
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
        }

       /* public int minmax(string status, ref Piece[,] current, int layer, int max_layer, ref int fromx, ref int fromy, ref int tox, ref int toy,int g,int h)//current is map status，return point of a match
        {
            int value = 0;
            if (layer == 0)
                return calculate_point(ref current,g,h);
            if (status == "black")
            {
                value = int.MinValue;
                Piece temp;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (current[i, j] == null || current[i, j].team == "white")
                            continue;
                        else
                        {
                            bool[,] path = current[i, j].ValidPath(i, j, current);
                            for (int m = 0; m < 8; m++)
                                for (int n = 0; n < 8; n++)
                                    if (path[m, n])
                                    {
                                        //move piece from i,j to m,n
                                        temp = current[m, n];
                                        current[m, n] = current[i, j];
                                        current[i, j] = null;

                                        int c = minmax("white", ref current, layer - 1, max_layer, ref fromx, ref fromy, ref tox, ref toy,m,n);
                                        if (c > value)
                                        {
                                            if (max_layer ==layer)
                                            {
                                                fromx = i;
                                                fromy = j;
                                                tox = m;
                                                toy = n;
                                            }
                                            value = c;
                                        }

                                        //回復原盤面
                                        current[i, j] = current[m, n];
                                        current[m, n] = temp;
                                    }
                        }
            }
            else
            {
                value = int.MaxValue;
                Piece temp;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (current[i, j] == null || current[i, j].team == "black")
                            continue;
                        else
                        {
                            bool[,] path = current[i, j].ValidPath(i, j, current);
                            for (int m = 0; m < 8; m++)
                                for (int n = 0; n < 8; n++)
                                    if (path[m, n])
                                    {
                                        int k = calculate_point(ref current, m, n);
                                        //move piece from i,j to m,n
                                        temp = current[m, n];
                                        current[m, n] = current[i, j];
                                        current[i, j] = null;                    
                                        int c = minmax("white", ref current, layer - 1, max_layer, ref fromx, ref fromy, ref tox, ref toy,m,n);
                                        if (c < value)
                                            value = c;
                                        //回復原盤面
                                        current[i, j] = current[m, n];
                                        current[m, n] = temp;
                                    }
                        }
            }
            return value;

        }*/
        public int minmax(ref Piece[,] current, int layer, int max_layer, ref int fromx, ref int fromy, ref int tox, ref int toy)//current is map status，return point of a match
        {
            int highest_score_of_a_match = int.MinValue;
            int x = int.MinValue;
            if (layer < 0)
                return 0;
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
                        bool[,] paths = current[i, j].ValidPath(i, j, current);//get all valid paths of piece[i,j]                       
                        for (int m = 0; m < 8; m++)
                            for (int n = 0; n < 8; n++)
                                if (paths[m, n] == true)
                                {
                                    //calculate points and recur to next level
                                    int temp_score = calculate_point(ref current, m, n);//calculate how many score can get if go to [m,n]
                                    int temp_score_2 = int.MinValue;
                                    //Console.WriteLine(temp_score);
                                    //*******moving piece from [i,j] to [m,n]*******
                                    temp = current[m, n];
                                    current[m, n] = current[i, j];
                                    current[i, j] = null;
                                    //**********************************************
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
                                                            if (temp_score_2 > calculate_point(ref current, c, d))
                                                                continue;
                                                            else
                                                            {
                                                                temp_score_2 = calculate_point(ref current, c, d);
                                                                int z = temp_score - temp_score_2;//此局分數
                                                                //*******moving piece from [a,b] to [c,d]*******
                                                                temp2 = current[c, d];
                                                                current[c, d] = current[a, b];
                                                                current[a, b] = null;
                                                                //**********************************************
                                                                int y = minmax(ref current, layer - 1, max_layer, ref fromx, ref fromy, ref tox, ref toy);
                                                                x = x > y ? x : y;//選出子棋局最大的分數，紀錄在x裡
                                                                if (highest_score_of_a_match < (x + z))
                                                                {
                                                                    highest_score_of_a_match = x + z;
                                                                    if (layer == max_layer)
                                                                    {
                                                                        fromx = i;
                                                                        fromy = j;
                                                                        tox = m;
                                                                        toy = n;
                                                                    }
                                                                }
                                                                //*******recover map to initial status**********
                                                                current[a, b] = current[c, d];
                                                                current[c, d] = temp2;
                                                                //**********************************************
                                                            }
                                                        }
                                            }
                                        }
                                    //*******recover map to initial status**********
                                    current[i, j] = current[m, n];
                                    current[m, n] = temp;
                                    //**********************************************
                                }
                    }
                    else
                        continue;
                }
            return highest_score_of_a_match;
        }
        public void AI(ref Piece[,] map)
        {
            int fromx = 0, fromy = 0, tox = 0, toy = 0;
            minmax(ref map, 1, 1, ref fromx, ref fromy, ref tox, ref toy);
            map[tox, toy] = map[fromx, fromy];
            map[fromx, fromy] = null;
        }
    }
}