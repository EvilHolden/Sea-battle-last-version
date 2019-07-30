using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static MainForm.MainWindow;

namespace MainForm
{
    struct Statistics
    {
        private Label fourCellsShipLabel, threeCellsShipLabel, twoCellsShipLabel, oneCellShipLabel, shotsCountLabel;
        private int fourCellsShipsCount, twoCellsShipsCount, threeCellsShipsCount, oneCellsShipsCount,shotsCount;

        public Statistics(Label fourCellsShip, Label threeCellsShip, Label twoCellsShip, Label oneCellShip, Label shotsCount)
        {
            fourCellsShipLabel = fourCellsShip;
            threeCellsShipLabel = threeCellsShip ;
            twoCellsShipLabel = twoCellsShip;
            oneCellShipLabel = oneCellShip;
            shotsCountLabel = shotsCount;

            fourCellsShipsCount = 1;
            twoCellsShipsCount = 3;
            threeCellsShipsCount = 2;
            oneCellsShipsCount = 4;
            this.shotsCount = 0;
        }

        public void UpDate()
        {
            oneCellShipLabel.Content = oneCellsShipsCount;
            twoCellsShipLabel.Content = twoCellsShipsCount;
            threeCellsShipLabel.Content = threeCellsShipsCount;
            fourCellsShipLabel.Content = fourCellsShipsCount;
            shotsCountLabel.Content = shotsCount;
        }

        public void DeleteShipCount(int shipSize)
        {
            switch(shipSize)
            {
                case 1: oneCellsShipsCount--; oneCellShipLabel.Content = oneCellsShipsCount; ; break;
                case 2: twoCellsShipsCount--; twoCellsShipLabel.Content = twoCellsShipsCount; ; break;
                case 3: threeCellsShipsCount--; threeCellsShipLabel.Content = threeCellsShipsCount; ; break;
                case 4: fourCellsShipsCount--; fourCellsShipLabel.Content = fourCellsShipsCount; ; break;
            }

        }

        public void AddShot()
        {
            shotsCountLabel.Content = ++shotsCount;
        }
    }
}
//MainWindow mainWindow = new MainWindow();

//            if (Ship[0].X == Ship[1].X)//Если корабль стоит вертикально
//            {

//            }
//            else if (Ship[0].Y == Ship[1].Y)//Если корабль стоит горизонтально
//            {//проверяем пять клеток в начале корабля и 5 клеток в конце корабля
//                Point firstPoint = Ship[0];
//                if (mainWindow.IsCorrectCell(firstPoint.X -1 ,firstPoint.Y))
//                {
//                    if (LeftMatrix[firstPoint.X - 1, firstPoint.Y] == 0)
//                        CloseCell();
//                }
//                if (mainWindow.IsCorrectCell(firstPoint.X - 1, firstPoint.Y + 1))
//                {
//                    if (LeftMatrix[firstPoint.X - 1, firstPoint.Y + 1] == 0)
//                        CloseCell();
//                }
//                if (mainWindow.IsCorrectCell(firstPoint.X, firstPoint.Y - 1))
//                {
//                    if (LeftMatrix[firstPoint.X, firstPoint.Y - 1] == 0)
//                        CloseCell();
//                }
//                if (mainWindow.IsCorrectCell(firstPoint.X, firstPoint.Y + 1))
//                {
//                    if (LeftMatrix[firstPoint.X, firstPoint.Y + 1] == 0)
//                        CloseCell();
//                }

//            }

//public void CloseCellsAtShip(List<Point> Ship,int[,] LeftMatrix)
//{

//    closedCellsLabel.Content = closedCells;
//    openedCellsLabel.Content = openedCells;
//}
