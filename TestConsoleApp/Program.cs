using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        //static int[,] OurFieldMatrix = new int[10,10];// 1 - корабль, 0 - пустая клетка, -1 клетки вокруг корабля, -2 корабль ранен или потоплен

        //static private bool IsOtherShipsInCell(int i, int j)
        //{

        //    try
        //    {
        //        if (OurFieldMatrix[i, j] != 0) return true;
        //    }
        //    catch { }
        //    try
        //        {
        //            if (OurFieldMatrix[i - 1, j - 1] != 0) return true;
        //        }
        //        catch { }

        //    try
        //    {
        //        if (OurFieldMatrix[i - 1, j] != 0) return true;
        //    }
        //    catch { }
        //    try
        //    {
        //        if (OurFieldMatrix[i - 1, j + 1] != 0) return true;
        //    }
        //    catch { }

        //    try
        //    {
        //        if (OurFieldMatrix[i + 1, j - 1] != 0) return true;
        //    }
        //    catch { }
        //    try
        //    {
        //        if (OurFieldMatrix[i + 1, j] != 0) return true;
        //    }
        //    catch { }
        //    try
        //    {
        //        if (OurFieldMatrix[i + 1, j + 1] != 0) return true;
        //    }
        //    catch { }
        //    try
        //    {
        //        if (OurFieldMatrix[i, j - 1] != 0) return true;
        //    }
        //    catch { }
        //    try
        //    {
        //        if (OurFieldMatrix[i, j + 1] != 0) return true;
        //    }
        //    catch { }

        //    return false;
        //}

        //static public void SetOurShipsOnField()
        //{
        //    bool correct = true;
        //    for (int LengthShip = 4, Count = 1; LengthShip != 0; LengthShip--, Count++)
        //    {
        //        for (int CountIn = Count; CountIn > 0; CountIn--)
        //        {
        //            int j, i, Direction;
        //            do
        //            {
        //                correct = true;
        //                Random myRand = new Random();
        //                i = myRand.Next(0, OurFieldMatrix.GetLength(0) - 1);
        //                j = myRand.Next(0, OurFieldMatrix.GetLength(1) - 1);
        //                Direction = myRand.Next(0, 3);// 0 - лево 1 - вверх 2 - вправо 3 - вниз
        //                switch (Direction)
        //                {
        //                    case 0:
        //                        if (j - LengthShip >= 0)
        //                        {
        //                            for (int goLeft = 0; goLeft < LengthShip; goLeft++)
        //                            {
        //                                if (IsOtherShipsInCell(i,j - goLeft))
        //                                {
        //                                    correct = false;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        else
        //                            correct = false;
        //                        break;
        //                    case 1:
        //                        if (i - LengthShip >= 0)
        //                        {
        //                            for (int goUp = 0; goUp < LengthShip; goUp++)
        //                            {
        //                                if (IsOtherShipsInCell(i -goUp, j))
        //                                {
        //                                    correct = false;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        else
        //                            correct = false;
        //                        break;
        //                    case 2:
        //                        if (j + LengthShip < OurFieldMatrix.GetLength(1))
        //                        {
        //                            for (int goRight = 0; goRight < LengthShip; goRight++)
        //                            {
        //                                if (IsOtherShipsInCell(i , j + goRight))
        //                                {
        //                                    correct = false;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        else
        //                            correct = false;
        //                        break;
        //                    case 3:
        //                        if (i + LengthShip < OurFieldMatrix.GetLength(0))
        //                        {
        //                            for (int goDown = 0; goDown < LengthShip; goDown++)
        //                            {
        //                                if (IsOtherShipsInCell(i + goDown, j))
        //                                {
        //                                    correct = false;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        else
        //                            correct = false;
        //                        break;
        //                }

        //            } while (!correct);

        //            switch (Direction)
        //            {
        //                case 0:
        //                    for (int goLeft = 0; goLeft < LengthShip; goLeft++)
        //                        OurFieldMatrix[i, j - goLeft] = 1;
        //                    break;
        //                case 1:
        //                    for (int goUp = 0; goUp < LengthShip; goUp++)
        //                        OurFieldMatrix[i - goUp, j] = 1;
        //                    break;
        //                case 2:
        //                    for (int goRight = 0; goRight < LengthShip; goRight++)
        //                        OurFieldMatrix[i, j + goRight] = 1;
        //                    break;
        //                case 3:
        //                    for (int goDown = 0; goDown < LengthShip; goDown++)
        //                        OurFieldMatrix[i + goDown, j] = 1;
        //                    break;

        //            }

        //            Console.Clear();
        //            correct = true;
        //            for (int k = 0; k < 10; k++)
        //            {
        //                for (int l = 0; l < 10; l++)
        //                    Console.Write(OurFieldMatrix[k, l] + " ");
        //                Console.WriteLine();
        //            }

        //        }
        //    }

        //}
        static void Main(string[] args)
        {
            //HashSet<Point> points = new HashSet<Point>();
            //points.Add(new Point(1, 2));
            //points.Add(new Point(2, 3));
            //points.Add(new Point(3, 4));
            //Console.WriteLineL(points.C)
            //Console.ReadLine();
        }
    }
}

//if (OurFieldMatrix[i - 1, j] != 0) return false;
//    if (OurFieldMatrix[i - 1, j + 1] != 0) return false;
// }

//if (i < OurFieldMatrix.GetLength(0) - 1)
//{
//    if (OurFieldMatrix[i + 1, j - 1] != 0) return false;
//    if (OurFieldMatrix[i + 1, j] != 0) return false;
//    if (OurFieldMatrix[i + 1, j + 1] != 0) return false;
//}

//if (j > 0)
//    if (OurFieldMatrix[i, j - 1] != 0) return false;

//if (j < OurFieldMatrix.GetLength(1) - 1)
//    if (OurFieldMatrix[i, j + 1] != 0) return false;

//if (OurFieldMatrix[i, j] != 0) return false;
