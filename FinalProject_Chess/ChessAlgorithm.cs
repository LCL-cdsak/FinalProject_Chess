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
        public int calculate_point(Piece[,] current, int i, int j)
        {
            if (current[i, j] == null)
                return 0;
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
        public int minmax(ref Piece[,] current, int point, int layer)//current is map status，return point of a match
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
                                                            int highest_score = minmax(ref current, point + temp_score - temp_score_2, layer - 1);
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
        public void AI(ref Piece[,] map)
        {
            int fromx = 0, fromy = 0, tox = 0, toy = 0;
            int point = int.MinValue;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] == null)
                        continue;
                    else if (map[i, j].team == "black")//set com as black temporary
                    {
                        bool[,] paths = map[i, j].ValidPath(i, j, map);//get all valid paths of this piece
                        for (int m = 0; m < 8; m++)
                            for (int n = 0; n < 8; n++)
                            {
                                if (paths[m, n])
                                {
                                    int c = minmax(ref map, 0, 1);
                                    point = point > c ? point : c;
                                    //若point改成c，則i,j移動到m,n為當前最佳步法
                                    if (point == c)
                                    {
                                        fromx = i;
                                        fromy = j;
                                        tox = m;
                                        toy = n;
                                    }
                                }
                            }
                    }
                }
            map[tox, toy] = map[fromx, fromy];
            map[fromx, fromy] = null;
        }
    }
}


