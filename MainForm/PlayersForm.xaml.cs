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
    /// Логика взаимодействия для PlayersForm.xaml
    /// </summary>
    public partial class PlayersForm : Window
    {
        public PlayersForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = textboxPlayer.Text;
            StackPlayers.Children.Add(textBlock);
        }
    }
}
