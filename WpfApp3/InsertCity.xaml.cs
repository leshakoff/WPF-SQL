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
using System.Data;
using System.Data.SqlClient;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для InsertCity.xaml
    /// </summary>
    public partial class InsertCity : Window
    {
        DataSet ds = new DataSet();
        const string connectionString = @"Server=.\SQLEXPRESS;Database=America;Integrated Security=True";
        string sqlStates = "SELECT id AS 'ID ШТАТА', name AS 'НАЗВАНИЕ ШТАТА' FROM States";
        public InsertCity()
        {
            InitializeComponent();
            Load(null, null);
        }

        private void ShowStates(object sender, RoutedEventArgs e)
        {
            States.ItemsSource = ds.Tables["States"].DefaultView;
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var adapterStates = new SqlDataAdapter(sqlStates, connection);
                ds.Clear();
                adapterStates.Fill(ds, "States");
            }
        }

        private void DoneForm(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Foundation.Text) < 1500 || Convert.ToInt32(Foundation.Text) > 2021)
                {
                    MessageBox.Show("Вы ввели неправильный год! Попробуйте заново.");
                }
                else
                {
                    string name = CityName.Text;
                    int foundation = Convert.ToInt32(Foundation.Text);
                    int populaion = Convert.ToInt32(Population.Text);
                    int area = Convert.ToInt32(Area.Text);
                    string mayor = MayorName.Text;
                    int stateId = Convert.ToInt32(StateID.Text);

                    string insertCity = "INSERT INTO Cities (name, foundation_year, population, area, mayor, state_id) VALUES" +
                        $"('{name}', {foundation}, {populaion}, {area}, '{mayor}', {stateId})";

                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand(insertCity, conn);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Город добавлен в БД!");
                        Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Вы ввели не число! Попробуйте заново.");
            }
        }
    }
}
