using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для GameResults.xaml
    /// </summary>
    /// 
    
    public partial class GameResultsForm
    {
        public SqlConnection sqlConnection;
        public GameResultsForm()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private async void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = E:\SEA BATTLE С#\Sea battle\MainForm\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT *  FROM [statistics]", sqlConnection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    FirstStackPanel.Children.Add(new Label { Content = Convert.ToString(sqlReader["Продолжительность партии"]), FontFamily = new FontFamily("Calibri Light") });
                    SecondStackPanel.Children.Add(new Label { Content = Convert.ToString(sqlReader["Дата партии"]), FontFamily = new FontFamily("Calibri Light") });
                    ThirdStackPanel.Children.Add(new Label { Content = Convert.ToString(sqlReader["Победитель"]), FontFamily = new FontFamily("Calibri Light") });
                }
            }
            catch
            {

            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }
    }
}
