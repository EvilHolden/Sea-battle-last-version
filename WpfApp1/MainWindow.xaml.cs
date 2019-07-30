using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();

            boardtimer = new DispatcherTimer();
            boardtimer.Tick += new EventHandler(timer_Tick);
            boardtimer.Interval = new TimeSpan(0, 0, 0,0,1);
            boardtimer.Start();
            //(sender as Button).Background = new SolidColorBrush(Colors.Black);

            //Button1_Click(sender,e);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = new SolidColorBrush(Colors.Yellow);
        }
        private DispatcherTimer timer = null, boardtimer = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        Random MyRand = new Random();
        int x = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            button1.Background = new SolidColorBrush(Color.FromRgb((byte)MyRand.Next(0,255), (byte)MyRand.Next(0, 255), (byte)MyRand.Next(0, 255)));
            x++;
            if (x == 5)
                timer.Stop();
        }
    }
}
