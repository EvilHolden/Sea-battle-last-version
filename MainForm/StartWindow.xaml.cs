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
using System.Windows.Shapes;

namespace MainForm
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        MainWindow mainWindow;
        bool isFirstStart = true;
        public StartWindow(MainWindow mainWindow )
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button_StartGame_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Visibility = Visibility.Visible;
            mainWindow.GetStartForm = this;
            mainWindow.MinutsCount = mainWindow.SecondsCount =  0;
            if (!isFirstStart)
            {
                mainWindow.StartNewGame();
               
            }
            else
                isFirstStart = false;
            Visibility = Visibility.Hidden;
            //mainWindow.Focus();
        }

        private void Button__Click(object sender, RoutedEventArgs e)
        {
            GameRulesForm gameRulesForm = new GameRulesForm();
            gameRulesForm.ShowDialog();
        }

        private void Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
            mainWindow.Close();
        }

        private void Button_StartGame_Copy_Click(object sender, RoutedEventArgs e)
        {
            AboutProgramFrom aboutProgramFrom = new AboutProgramFrom();
            aboutProgramFrom.ShowDialog();
        }

    }
}
