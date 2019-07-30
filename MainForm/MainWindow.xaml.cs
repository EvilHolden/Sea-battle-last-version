using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Threading;

//1. Показывать время, которое прошло сначала игры 
//2. Показывать оставшееся корабли. Либо просто текст и числа, либо закрашивать иконки кораблей, когда их потапили.
//3. В окно результаты добавить имя игроки, время игры, кто выйграл.
//4. Поставить числа от 1 до 10 и буквы по границе поля
//5. Очень долго расставляет корабли на поле. Больше 1 - 3 секунды
//6. Показывать статистику убитых и раненых кораблей в виде чисел.


namespace MainForm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
        //class Statistics1
        //{
        //    void df1()
        //    {
            
            
        //    }
        //}

        //class Statistics
        //{
        //    void df()
        //    {
            
        //    }
        //}



    public partial class MainWindow : Window
    {

        Statistics LeftStatistics,RightStatistics;

        //Label[] Digits = new Label[10], Letters = new Label[10];

        //private void SetDigitsAndLetters()
        //{
        //    double height = wrapPanelLeft.Width / 10 , top = wrapPanelLeft.Margin.Top - height, left = wrapPanelLeft.Margin.Left + height/2 - 10;

        //    char chars = 'A'; int digits = 1;
        //    for(int i = 0; i < 10; i++)
        //    {
        //        Digits[i] = new Label();
        //        Digits[i].Content = digits.ToString();
        //        Digits[i].Margin = new Thickness(left,top,0,0);
        //        GridLeft.Children.Add(Digits[i]);
        //        left += height;
        //        digits++;
        //    }
        //}

        class CompareXY : IComparer<Point>
        {
            public int Compare(Point first, Point second)
            {
                if (first.X == second.X)
                {
                    if (first.Y < second.Y)
                        return -1;
                    else if (first.Y > second.Y)
                        return 1;
                    else return 0;

                }
                else if (first.Y == second.Y)
                {
                    if (first.X < second.X)
                        return -1;
                    else if (first.X > second.X)
                        return 1;
                    else return 0;
                }
                return 0;
            }
        }

        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int x, int y)
            {
                X = x; Y = y;
            }
            public Point(double x, double y)
            {
                X = int.Parse(x.ToString()); Y = int.Parse(y.ToString());
            }
        }

        Button[,] CreateButtons(int iLength, int jLength, WrapPanel wrapPanel, bool IsRight)
        {
            Button[,] buttons = new Button[iLength, jLength];
            for (int i = 0; i < iLength; i++)
                for (int j = 0; j < jLength; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].BorderThickness = new Thickness(1);
                    buttons[i, j].Padding = new Thickness(0, 0, 0, 0);
                   // buttons[i, j].Margin = new Thickness(1d);

                    buttons[i, j].BorderBrush = new SolidColorBrush(/*Color.FromRgb(158, 197, 255)*/Colors.LightBlue);
                    buttons[i, j].Background = new SolidColorBrush(Colors.White);

                    buttons[i, j].Width = wrapPanel.Width / 11 + 1;
                    buttons[i, j].Height = wrapPanel.Height / 11 + 1;

                    buttons[i, j].Name = "A" + i.ToString() + j.ToString();
                    if (IsRight)
                        buttons[i, j].AddHandler(Button.ClickEvent, new RoutedEventHandler(ClickButtonFromRightField));
                    else
                        buttons[i, j].IsHitTestVisible = false;
                }
            return buttons;

            //buttons[i, j].BorderThickness = new Thickness(1);
        }

        void AddToWrapPanel(Button[,] buttons, WrapPanel wrapPanel)
        {
            wrapPanel.Children.Clear();
            for (int i = 0; i < buttons.GetLength(0); i++)
                for (int j = 0; j < buttons.GetLength(1); j++)
                    wrapPanel.Children.Add(buttons[i, j]);
        }

        void PlayerButton_Click(object sender, RoutedEventArgs e)
        {
            PlayersForm playersForm = new PlayersForm();
            playersForm.Owner = this;
            playersForm.ShowDialog();
        }

        void SetRandomMatrix(Point[,] PointMatrix, int CountSwap)
        {

            for (int i = 0; i < CountSwap; i++)
            {
                int indexI1 = MyRand.Next(0, 9), indexJ1 = MyRand.Next(0, 9);
                int indexI2 = MyRand.Next(0, 9), indexJ2 = MyRand.Next(0, 9);
                Point TimePoint = PointMatrix[indexI2, indexJ2];
                PointMatrix[indexI2, indexJ2] = PointMatrix[indexI1, indexJ1];
                PointMatrix[indexI1, indexJ1] = TimePoint;
            }
        }

        public bool IsCorrectCell(int i, int j)
        {
            if (i >= 0 && i < CellCountInHeight && j >= 0 && j < CellCountInWidth) return true; else return false;
        }

        bool IsOtherShipsInCell(int i, int j, int[,] shipsField)
        {

            if (shipsField[i, j] != 0) return true;
            if (IsCorrectCell(i - 1, j - 1)) if (shipsField[i - 1, j - 1] != 0) return true;
            if (IsCorrectCell(i - 1, j)) if (shipsField[i - 1, j] != 0) return true;
            if (IsCorrectCell(i - 1, j + 1)) if (shipsField[i - 1, j + 1] != 0) return true;
            if (IsCorrectCell(i + 1, j - 1)) if (shipsField[i + 1, j - 1] != 0) return true;
            if (IsCorrectCell(i + 1, j)) if (shipsField[i + 1, j] != 0) return true;
            if (IsCorrectCell(i + 1, j + 1)) if (shipsField[i + 1, j + 1] != 0) return true;
            if (IsCorrectCell(i, j - 1)) if (shipsField[i, j - 1] != 0) return true;
            if (IsCorrectCell(i, j + 1)) if (shipsField[i, j + 1] != 0) return true;
            return false;
        }

        void SetOurShipsOnField(int[,] shipsField, List<List<Point>> points)
        {
            bool correct = true;
            List<Point> PointsList = new List<Point>();
            for (int lengthShip = 4, count = 1; lengthShip != 0; lengthShip--, count++)
            {
                for (int CountIn = count; CountIn > 0; CountIn--)
                {
                    int j, i, Direction;
                    do
                    {
                        correct = true;
                        Random myRand = new Random();
                        //i = myRand.Next(lengthShip - 1, CellCountInHeight - lengthShip);
                        //j = myRand.Next(lengthShip - 1, CellCountInHeight - lengthShip);

                        i = myRand.Next(0, CellCountInHeight - 1);
                        j = myRand.Next(0, CellCountInHeight - 1);

                        Direction = myRand.Next(0, 3);// 0 - лево 1 - вверх 2 - вправо 3 - вниз

                        switch (Direction)
                        {
                            case 0:
                                if (j - lengthShip >= 0)
                                {
                                    for (int goLeft = 0; goLeft < lengthShip; goLeft++)
                                    {
                                        if (IsOtherShipsInCell(i, j - goLeft, shipsField))
                                        {
                                            correct = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                    correct = false;
                                break;
                            case 1:
                                if (i - lengthShip >= 0)
                                {
                                    for (int goUp = 0; goUp < lengthShip; goUp++)
                                    {
                                        if (IsOtherShipsInCell(i - goUp, j, shipsField))
                                        {
                                            correct = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                    correct = false;
                                break;
                            case 2:
                                if (j + lengthShip < shipsField.GetLength(1))
                                {
                                    for (int goRight = 0; goRight < lengthShip; goRight++)
                                    {
                                        if (IsOtherShipsInCell(i, j + goRight, shipsField))
                                        {
                                            correct = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                    correct = false;
                                break;
                            case 3:
                                if (i + lengthShip < shipsField.GetLength(0))
                                {
                                    for (int goDown = 0; goDown < lengthShip; goDown++)
                                    {
                                        if (IsOtherShipsInCell(i + goDown, j, shipsField))
                                        {
                                            correct = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                    correct = false;
                                break;
                        }

                    } while (!correct);

                    PointsList = new List<Point>();
                    switch (Direction)
                    {
                        case 0:
                            for (int goLeft = 0; goLeft < lengthShip; goLeft++)
                            {
                                shipsField[i, j - goLeft] = 1;
                                PointsList.Add(new Point(i, j - goLeft));
                            }
                            points.Add(PointsList);
                            break;
                        case 1:
                            for (int goUp = 0; goUp < lengthShip; goUp++)
                            {
                                shipsField[i - goUp, j] = 1;
                                PointsList.Add(new Point(i - goUp, j));
                            }
                            points.Add(PointsList);
                            break;
                        case 2:
                            for (int goRight = 0; goRight < lengthShip; goRight++)
                            {
                                shipsField[i, j + goRight] = 1;
                                PointsList.Add(new Point(i, j + goRight));
                            }
                            points.Add(PointsList);
                            break;
                        case 3:
                            for (int goDown = 0; goDown < lengthShip; goDown++)
                            {
                                shipsField[i + goDown, j] = 1;
                                PointsList.Add(new Point(i + goDown, j));
                            }
                            points.Add(PointsList);
                            break;

                    }
                }
            }

        }

        void SetMarginCellOfSinkShip(int i, int j, Button[,] buttons, int[,] ShipsField)
        {

            if (ShipsField[i, j] == 0) buttons[i, j].IsEnabled = false;

            if (IsCorrectCell(i - 1, j - 1)) if (ShipsField[i - 1, j - 1] == 0) buttons[i - 1, j - 1].IsEnabled = false;

            if (IsCorrectCell(i - 1, j)) if (ShipsField[i - 1, j] == 0) buttons[i - 1, j].IsEnabled = false;

            if (IsCorrectCell(i - 1, j + 1)) if (ShipsField[i - 1, j + 1] == 0) buttons[i - 1, j + 1].IsEnabled = false;

            if (IsCorrectCell(i + 1, j - 1)) if (ShipsField[i + 1, j - 1] == 0) buttons[i + 1, j - 1].IsEnabled = false;

            if (IsCorrectCell(i + 1, j)) if (ShipsField[i + 1, j] == 0) buttons[i + 1, j].IsEnabled = false;

            if (IsCorrectCell(i + 1, j + 1)) if (ShipsField[i + 1, j + 1] == 0) buttons[i + 1, j + 1].IsEnabled = false;

            if (IsCorrectCell(i, j - 1)) if (ShipsField[i, j - 1] == 0) buttons[i, j - 1].IsEnabled = false;

            if (IsCorrectCell(i, j + 1)) if (ShipsField[i, j + 1] == 0) buttons[i, j + 1].IsEnabled = false;

        }

        void SetMarginForShip(List<Point> ShipsCoordinate, Button[,] ButtonsField, int[,] ShipsField)
        {
            int shipCoordI, shipCoordJ;// сделать метод для обоих полей
            for (int k = 0; k < ShipsCoordinate.Count(); k++)
            {
                shipCoordI = ShipsCoordinate[k].X;
                shipCoordJ = ShipsCoordinate[k].Y;
                SetMarginCellOfSinkShip(shipCoordI, shipCoordJ, ButtonsField, ShipsField);
                ButtonsField[shipCoordI, shipCoordJ].IsHitTestVisible = false;
            }
        }

        void SetUnavailableCell(int i, int j, int[,] shipsField)
        {

            shipsField[i, j] = -1;

            if (IsCorrectCell(i - 1, j - 1)) shipsField[i - 1, j - 1] = -1;

            if (IsCorrectCell(i - 1, j)) shipsField[i - 1, j] = -1;

            if (IsCorrectCell(i - 1, j + 1)) shipsField[i - 1, j + 1] = -1;

            if (IsCorrectCell(i + 1, j - 1)) shipsField[i + 1, j - 1] = -1;

            if (IsCorrectCell(i + 1, j)) shipsField[i + 1, j] = -1;

            if (IsCorrectCell(i + 1, j + 1)) shipsField[i + 1, j + 1] = -1;

            if (IsCorrectCell(i, j - 1)) shipsField[i, j - 1] = -1;

            if (IsCorrectCell(i, j + 1)) shipsField[i, j + 1] = -1;

        }

        void SetUnavailableCells(List<Point> ShipsCoordinate, int[,] ShipsField)
        {
            for (int i = 0; i < ShipsCoordinate.Count; i++)
                SetUnavailableCell(ShipsCoordinate[i].X, ShipsCoordinate[i].Y, ShipsField);
        }

        Color GetColor()
        {
            return Color.FromRgb((byte)MyRand.Next(0, 255), (byte)MyRand.Next(0, 255), (byte)MyRand.Next(0, 255));
        }

        void SetIconOfKill(List<Point> ship, Button[,] buttonsField)
        {
            foreach (Point x in ship)
                buttonsField[x.X, x.Y].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/KilledIcon.ico")) };
        }

        void PushInStackFreeCells(int i, int j, Stack<Point> points)
        {
            List<Point> listForRandPoints = new List<Point>();
            Point timePoint;
            try { if (ShipsFieldMatrixLeft[i - 1, j] != -1) listForRandPoints.Add(new Point(i - 1, j)); } catch { }
            try { if (ShipsFieldMatrixLeft[i + 1, j] != -1) listForRandPoints.Add(new Point(i + 1, j)); } catch { }
            try { if (ShipsFieldMatrixLeft[i, j - 1] != -1) listForRandPoints.Add(new Point(i, j - 1)); } catch { }
            try { if (ShipsFieldMatrixLeft[i, j + 1] != -1) listForRandPoints.Add(new Point(i, j + 1)); } catch { }

            for (int i1 = 0; i1 < 10; i1++)
            {
                int FirstElemIndex = MyRand.Next(0, listForRandPoints.Count), SecondElemIndex = MyRand.Next(0, listForRandPoints.Count);
                Point FirstElem = listForRandPoints.ElementAt(FirstElemIndex), SecondElem = listForRandPoints.ElementAt(SecondElemIndex);

                timePoint = FirstElem;
                listForRandPoints[FirstElemIndex] = listForRandPoints[SecondElemIndex];
                listForRandPoints[SecondElemIndex] = FirstElem;
            }
            foreach (Point x in listForRandPoints)
                points.Push(x);
        }

        internal void StartNewGame()
        {
            
            //sqlConnection = new SqlConnection("123");
            iFromRandMatrix = 0; jFromRandMatrix = 0;
            //wrapPanelLeft.IsHitTestVisible = true;
            wrapPanelRight.IsHitTestVisible = true;

            LeftShipsCoordinate = new List<List<Point>>();
            RightShipsCoordinateRight = new List<List<Point>>();
            ShipsFieldMatrixLeft = new int[CellCountInWidth, CellCountInHeight];
            ShipsFieldMatrixRight = new int[CellCountInWidth, CellCountInHeight];

            SetOurShipsOnField(ShipsFieldMatrixLeft, LeftShipsCoordinate);
            ButtonsFieldLeft = CreateButtons(CellCountInWidth, CellCountInHeight, wrapPanelLeft, false);

            SetOurShipsOnField(ShipsFieldMatrixRight, RightShipsCoordinateRight);
            ButtonsFieldRight = CreateButtons(CellCountInWidth, CellCountInHeight, wrapPanelRight, true);

            SetShipsOnField(ButtonsFieldLeft, ShipsFieldMatrixLeft, Colors.Green);
            //SetShipsOnField(ButtonsFieldRight, ShipsFieldMatrixRight, Colors.Green);

            AddToWrapPanel(ButtonsFieldLeft, wrapPanelLeft);
            AddToWrapPanel(ButtonsFieldRight, wrapPanelRight);


            int k = 0;
            RightShipsCoordinateRightCopy = new List<List<Point>>();
            foreach (var PointList in RightShipsCoordinateRight)
                RightShipsCoordinateRightCopy.Add(RightShipsCoordinateRight[k++].GetRange(0, PointList.Count()));
            LeftShipsCoordinateCopy = new List<List<Point>>();
            k = 0;
            foreach (var PointList in LeftShipsCoordinate)
                LeftShipsCoordinateCopy.Add(LeftShipsCoordinate[k++].GetRange(0, PointList.Count()));

            for (int i = 0; i < CellCountInHeight; i++)
                for (int j = 0; j < CellCountInWidth; j++)
                    RandCellsMatrix[i, j] = new Point(i, j);

            CountLostShips = 0;
            SetRandomMatrix(RandCellsMatrix, 100);

            LeftStatistics = new Statistics(LabelFourCellsShipCount, LabelThreeCellsShipCount, LabelTwoCellsShipCount, LabelOneCellShipCount,
                ShotsCountLabel2);
            LeftStatistics.UpDate();

            RightStatistics = new Statistics(LabelFourCellsShipCount1, LabelThreeCellsShipCount1, LabelTwoCellsShipCount1, LabelOneCellShipCount1,
                ShotsCountLabel4);
            RightStatistics.UpDate();

            //timer = new DispatcherTimer();
            //timer.Tick += new EventHandler(StepOfPC);
            //timer.Interval = new TimeSpan(0, 0, 0,0 ,500);

            SecondsCount = 0; MinutsCount = 0;
            //boardtimer = new DispatcherTimer();
            //boardtimer.Tick += new EventHandler(boardtimer_Tick);
            //boardtimer.Interval = new TimeSpan(0, 0, 0, 1);
            //boardtimer.Start();

            //if (MyRand.Next(2) == 1)
            //{
            //NextUserStep = false;
            //timer.Start();
            //StepOfPC(null, null);
                
            //}
            
        }

        const int CellCountInWidth = 10, CellCountInHeight = 10;
        //static int[,] ShipsFieldMatrixLeft = new int[CellCountInWidth, CellCountInHeight], ShipsFieldMatrixRight = new int[CellCountInWidth, CellCountInHeight];

        int[,] ShipsFieldMatrixLeft, ShipsFieldMatrixRight;

        List<List<Point>> RightShipsCoordinateRight, RightShipsCoordinateRightCopy, LeftShipsCoordinate, LeftShipsCoordinateCopy;

        Button[,] ButtonsFieldLeft, ButtonsFieldRight;
        Point[,] RandCellsMatrix = new Point[CellCountInWidth, CellCountInHeight];
        Stack<Point> StackOfCellsAfterInjury = new Stack<Point>();

        Random MyRand = new Random();

        public MainWindow()
        {
            InitializeComponent();

            

            //Application.Run(new StartWindow());

            //this.Visibility = Visibility.Hidden;
            StartWindow startWindow = new StartWindow(this);
            startWindow.Show();
            //Close();
        }
        
        public void SetShipsOnField(Button[,] buttons, int[,] ShipsField, Color shipColor) {
            for (int i = 0; i < CellCountInHeight; i++)
                for (int j = 0; j < CellCountInWidth; j++)
                    if (ShipsField[i, j] == 1)
                        buttons[i, j].Background = new SolidColorBrush(shipColor);
        }

        private DispatcherTimer timer = null, boardtimer = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //Line line = new Line { X1 = 0, Y1 = 125, X2 = this.ActualHeight, Y2 = 125, Stroke = new SolidColorBrush(Colors.Black)};
            
            LabelTurn.Foreground = new SolidColorBrush(Colors.Green);

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(StepOfPC);
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            StartNewGame();

            
            boardtimer = new DispatcherTimer();
            boardtimer.Tick += new EventHandler(boardtimer_Tick);
            boardtimer.Interval = new TimeSpan(0, 0, 0, 1);
            boardtimer.Start();
        }

        public int SecondsCount = 0, MinutsCount = 0;

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void boardtimer_Tick(object sender, EventArgs e)
        {
            SecondsCount++;
            if (SecondsCount == 60) { MinutsCount++; SecondsCount = 0; }
            if (SecondsCount < 10)
                LabelTime.Content = MinutsCount + ":" + 0 + SecondsCount;
            else
                LabelTime.Content = MinutsCount + ":" + SecondsCount;


        }

        public StartWindow GetStartForm
        {
            get;set;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            //GetStartForm.Focus();
            GetStartForm.Visibility = Visibility.Visible;

        }

        public SqlConnection sqlConnection;

        private /*async*/ void StepOfPC(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = E:\SEA BATTLE С#\Sea battle\MainForm\Database1.mdf;Integrated Security=True";
            //sqlConnection = new SqlConnection(connectionString);
            //await sqlConnection.OpenAsync();

            //SqlCommand command = new SqlCommand("INSERT INTO [Statistics] (Продолжительность партии,Дата партии, [Победитель]) values (@Продолжительность партии,@Дата партии,@Победитель)", sqlConnection);
            //command.Parameters.AddWithValue("Продолжительность партии", "1:45");
            //command.Parameters.AddWithValue("Дата партии", "13.12.2019");
            //command.Parameters.AddWithValue("Победитель", "пк");
            //await command.ExecuteNonQueryAsync();

            if (NextUserStep == false)// Если пользователь промахнулся, значит ход ПК
                //do
                //{
                //Label_Computer.Foreground = new SolidColorBrush(Colors.Yellow);
                //Label_You.Foreground = new SolidColorBrush(Colors.Gray);
            IsInjury = false;
            LeftStatistics.AddShot();
                    if (StackOfCellsAfterInjury.Count == 0)//если стэк пустой, иначе есть раненый корабль, но не добитый
                    {
                        do
                        {
                            iRandOfShip = RandCellsMatrix[iFromRandMatrix, jFromRandMatrix].X;// координата следующего случайного корабля i
                            jRandOfShip = RandCellsMatrix[iFromRandMatrix, jFromRandMatrix].Y;//координата следующего случайного корабля j

                            jFromRandMatrix++;
                            if (jFromRandMatrix == CellCountInWidth) { iFromRandMatrix++; jFromRandMatrix = 0; };
                            if (iFromRandMatrix == 10)
                                //{
                                if (MessageBox.Show("Увы, но компьютер уделал тебя. Нажмите ок, чтобы начать новую игру", "Конец игры", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    StartNewGame();
                                   
                                

                                goto Label1;
                        }
                                else
                                    //{
                                    //wrapPanelLeft.IsHitTestVisible = false;
                                    wrapPanelRight.IsHitTestVisible = false;
                            //}
                            //}
                        }
                        while (ShipsFieldMatrixLeft[iRandOfShip, jRandOfShip] == -1);// пока не найдем пустую точку

                        NextUserStep = true;
                        if (ShipsFieldMatrixLeft[iRandOfShip, jRandOfShip] == 1)// Если ПК попал в первый раз в корабль
                        {

                            for (int i = 0; i < LeftShipsCoordinate.Count(); i++)// Проходим по всем кораблям user'a
                                for (int j = 0; j < LeftShipsCoordinate[i].Count(); j++)
                                    if (LeftShipsCoordinate[i][j].X == iRandOfShip && LeftShipsCoordinate[i][j].Y == jRandOfShip)
                                        if (LeftShipsCoordinate[i].Count > 1)
                                        {
                                            ListClosedCellsOfShip.Add(new Point(iRandOfShip, jRandOfShip));
                                            PushInStackFreeCells(iRandOfShip, jRandOfShip, StackOfCellsAfterInjury);
                                            //ButtonsFieldLeft[iRandOfShip, jRandOfShip].Content = CenterLabel.Content = NumForBut.ToString();
                                            ButtonsFieldLeft[iRandOfShip, jRandOfShip].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/InjuredIcon.ico")) };

                                            //ShipsCoordinateLeft[i].Remove(new Point(iRandOfShip,jRandOfShip));
                                            for (int q = 0; q < LeftShipsCoordinate[i].Count; q++)
                                                AllCoordOfInjuredShip.Add(LeftShipsCoordinate[i][q]);

                                            for (int q = 0; q < LeftShipsCoordinate[i].Count; q++)
                                                if (LeftShipsCoordinate[i][q].X != iRandOfShip || LeftShipsCoordinate[i][q].Y != jRandOfShip)
                                                    ListRemainingCellsOfShip.Add(LeftShipsCoordinate[i][q]);

                                            //ShipsCoordinateLeft[i].Remove(ShipsCoordinateLeft[i][j]);
                                            //Thread.Sleep(3000);

                                            IsInjury = true;
                                        }
                                        else if (LeftShipsCoordinate[i].Count == 1) //Если корабль сразу убит, то блокируем кнопки вокруг корабля
                                        {
                                            //CenterLabel.Content = NumForBut.ToString();
                                            //ButtonsFieldLeft[iRandOfShip, jRandOfShip].Content = CenterLabel.Content = NumForBut.ToString();
                                            IsInjury = true;
                                            //SetMarginForShip(ShipsCoordinateLeft[i], ButtonsFieldLeft, ShipsFieldMatrixLeft);
                                            SetUnavailableCells(LeftShipsCoordinate[i], ShipsFieldMatrixLeft);
                                            ButtonsFieldLeft[iRandOfShip, jRandOfShip].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/KilledIcon.ico")) };
                                            LeftStatistics.DeleteShipCount(1);
                                        }
                        }
                        else
                        {
                            //ButtonsFieldLeft[iRandOfShip, jRandOfShip].IsEnabled = false;
                            ShipsFieldMatrixLeft[iRandOfShip, jRandOfShip] = -1;
                            //CenterLabel.Content = NumForBut.ToString();

                            ButtonsFieldLeft[iRandOfShip, jRandOfShip].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/PointForMissIcon.ico")) }; ;

                        }
                    }
                    else// если стэк не пустой, значит есть не потопленный корабль
                    {
                        int iShipFromStack, jShipFromStack;
                        Point nextShip;
                        do
                        {
                            nextShip = StackOfCellsAfterInjury.Pop(); // Берем преполагаемую точку, чтобы добить корабль
                            iShipFromStack = int.Parse(nextShip.X.ToString());
                            jShipFromStack = int.Parse(nextShip.Y.ToString()); //Координаты предполагаемого корабля

                        }
                        while (ShipsFieldMatrixLeft[iShipFromStack, jShipFromStack] == -1);// Идем по стэку клеток, пока не найдем пустую клетку

                        NextUserStep = true; IsInjury = false;
                        if (ShipsFieldMatrixLeft[iShipFromStack, jShipFromStack] == 1) // Если мы попали в корабль, исходя из матрицы расстановки кораблей
                        {
                            //ButtonsFieldLeft[iShipFromStack, jShipFromStack].Background = new SolidColorBrush(Colors.Yellow);
                            // CenterLabel.Content = NumForBut.ToString();

                            ShipsFieldMatrixLeft[iShipFromStack, jShipFromStack] = -1;
                            for (int i = 0; i < ListRemainingCellsOfShip.Count; i++) //Проходим по списку координат точек, которые остались в раненом корабле
                                if (ListRemainingCellsOfShip[i].X == iShipFromStack && ListRemainingCellsOfShip[i].Y == jShipFromStack) //Если попали, то
                                {
                                    NextUserStep = false;
                                    //Thread.Sleep(3000);
                                    ListRemainingCellsOfShip.Remove(nextShip); // Удаляем клетку, в которую уже попали из списка оставшихся
                                    if (ListRemainingCellsOfShip.Count == 0) // Если список, оставшихся клеток, пуст, то блокируем клетки на поле пользователя
                                    {
                                        //SetMarginForShip(ListCoordOfInjuredShip, ButtonsFieldLeft, ShipsFieldMatrixLeft);
                                        SetIconOfKill(AllCoordOfInjuredShip, ButtonsFieldLeft);
                                        // ButtonsFieldLeft[iShipFromStack, jShipFromStack].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/ЗначокУбил.ico")) };
                                        SetUnavailableCells(AllCoordOfInjuredShip, ShipsFieldMatrixLeft);
                                        ListClosedCellsOfShip.Clear();
                                        StackOfCellsAfterInjury.Clear();
                                        LeftStatistics.DeleteShipCount(AllCoordOfInjuredShip.Count);
                                        AllCoordOfInjuredShip.Clear();
                                        IsInjury = true;
                                        
                            }
                                    else if (ListRemainingCellsOfShip.Count > 0) // Если список, оставшихся клеток, не пуст значит корабль состоит более двух клеток
                                    // и нужно добавлять две клетки в стэк
                                    {
                                        ButtonsFieldLeft[iShipFromStack, jShipFromStack].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/InjuredIcon.ico")) };
                                        ListClosedCellsOfShip.Add(new Point(iShipFromStack, jShipFromStack));
                                        StackOfCellsAfterInjury.Clear();
                                        int Count = ListClosedCellsOfShip.Count - 1;

                                        if (ListClosedCellsOfShip[0].X == ListClosedCellsOfShip[1].X)//Если корабль стоит вертикально
                                        {
                                            ListClosedCellsOfShip.Sort(new CompareXY());
                                            double firstX = ListClosedCellsOfShip[0].X, firstY = ListClosedCellsOfShip[0].Y;
                                            double secondX = ListClosedCellsOfShip[Count].X, secondY = ListClosedCellsOfShip[Count].Y;
                                            StackOfCellsAfterInjury.Push(new Point(firstX, firstY - 1));
                                            StackOfCellsAfterInjury.Push(new Point(secondX, secondY + 1));
                                            IsInjury = true;
                                        }
                                        else if (ListClosedCellsOfShip[0].Y == ListClosedCellsOfShip[1].Y)//Если корабль стоит горизонтально
                                        {
                                            ListClosedCellsOfShip.Sort(new CompareXY());
                                            double firstX = ListClosedCellsOfShip[0].X, firstY = ListClosedCellsOfShip[0].Y;
                                            double secondX = ListClosedCellsOfShip[Count].X, secondY = ListClosedCellsOfShip[Count].Y;

                                            StackOfCellsAfterInjury.Push(new Point(firstX - 1, firstY));
                                            StackOfCellsAfterInjury.Push(new Point(secondX + 1, secondY));
                                            IsInjury = true;
                                        }


                                    }
                                    //if (StackOfCellsAfterInjury.Count == 0)
                                    //{
                                    //    SetMarginCellOfSinkShip(iShip, jShip, ButtonsFieldLeft, ShipsFieldMatrixLeft);
                                    //    IsInjury = false;
                                    //}

                                    //StackOfCellsAfterInjury.Clear();

                                }
                            //else //Иначе даем ход пользователю
                            //{

                            //}


                        }// компьютер не попал клетой из стэка
                        else
                        {
                            ButtonsFieldLeft[iShipFromStack, jShipFromStack].Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/PointForMissIcon.ico")) };
                            ShipsFieldMatrixLeft[iShipFromStack, jShipFromStack] = -1;
                            IsInjury = false;
                        }
                    }
            if (!IsInjury)
            {
                timer.Stop();
                LabelTurn.Foreground = new SolidColorBrush(Colors.Green);
                LabelTurn.Content = "ВАШ ХОД";
                //Label_Computer.Foreground = new SolidColorBrush(Colors.Gray);
                //Label_You.Foreground = new SolidColorBrush(Colors.Yellow);

            }
                //} while (IsInjury);
            Label1:;
            wrapPanelRight.IsHitTestVisible = true;
            
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        bool IsInjury = false, NextUserStep = true;
        int iFromRandMatrix, jFromRandMatrix, iRandOfShip, jRandOfShip, /*numForBut = 1,*/ CountLostShips = 0;
        List<Point> ListRemainingCellsOfShip = new List<Point>(), AllCoordOfInjuredShip = new List<Point>(), ListClosedCellsOfShip = new List<Point>();
        //int NumForBut
        //{
        //   get { return numForBut++; }
        //}
        Button LastButton = new Button();
        bool LastKill = false;

        private void ClickButtonFromRightField(object sender, RoutedEventArgs e)
        {
            Button CurButt = sender as Button;
            if (NextUserStep)// Если ход пользователя
            {
                NextUserStep = false;
                int buttonCoordI = int.Parse(CurButt.Name[1].ToString()), buttonCoordJ = int.Parse(CurButt.Name[2].ToString());

                for (int i = 0; i < RightShipsCoordinateRight.Count(); i++)// Проходим по всем кораблям ПК
                    for (int j = 0; j < RightShipsCoordinateRight[i].Count(); j++)
                        if (RightShipsCoordinateRight[i][j].X == buttonCoordI && RightShipsCoordinateRight[i][j].Y == buttonCoordJ)// Если пользователь попал в корабль
                        {
                            //RightStatistics.CloseOneCell();
                            RightStatistics.AddShot();
                            CurButt.IsEnabled = false;
                            LastButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/PointForMissBlack.ico")) };
                            RightShipsCoordinateRight[i].Remove(RightShipsCoordinateRight[i][j]);
                            if (RightShipsCoordinateRight[i].Count > 0)//Если корабль только ранен
                            {

                                NextUserStep = true;
                                LastKill =  true;
                                CurButt.Background = new SolidColorBrush(Colors.LightBlue);
                                CurButt.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/InjuredIcon.ico")) };
                                //RightStatistics.AddShot();
                                
                                //LastButton = new Button();
                            }
                            else if (RightShipsCoordinateRight[i].Count == 0)// Если корабль сразу убит
                            {

                                CurButt.Background = new SolidColorBrush(Colors.LightBlue);
                                CurButt.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/KilledIcon.ico")) };
                                SetIconOfKill(RightShipsCoordinateRightCopy[i], ButtonsFieldRight);
                                CountLostShips++;
                                SetMarginForShip(RightShipsCoordinateRightCopy[i], ButtonsFieldRight, ShipsFieldMatrixRight);
                                NextUserStep = true;
                                LastKill = true;
                                RightStatistics.DeleteShipCount(RightShipsCoordinateRightCopy[i].Count);
                                
                            }
                        }
                if (CountLostShips == 10)// Если все корабли потоплены
                                         //{
                    if (MessageBox.Show("Поздравляю, вы выиграли. Нажмите ок, чтобы начать новую игру", "Конец игры", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        StartNewGame();
                        goto Label1;
                    }
                    else
                        wrapPanelRight.IsHitTestVisible = false;
            }
            if (NextUserStep == false)
            {
                RightStatistics.AddShot();

                LastButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/PointForMissBlack.ico")) };

                CurButt.IsHitTestVisible = false;
                CurButt.Background = new SolidColorBrush(Colors.FloralWhite);
                CurButt.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Icons/PointForMissYellow.ico")) };
                timer.Start();
                wrapPanelRight.IsHitTestVisible = false;
                //Label_Computer.Foreground = new SolidColorBrush(Colors.Yellow);
                //Label_You.Foreground = new SolidColorBrush(Colors.Gray);
                LabelTurn.Foreground = new SolidColorBrush(Colors.Red);
                LabelTurn.Content = "ХОД КОМПЬЮТЕРА";
                LastButton = CurButt;
            }

            
        Label1:;

            
        }
    }
}
