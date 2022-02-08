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
using System.Data;
using System.Data.SqlClient;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string connectionString = @"Server=.\SQLEXPRESS;Database=America;Integrated Security=True";

        string sqlCitizens = "SELECT c.id AS 'ID', c.name AS 'ИМЯ', c.surname AS 'ФАМИЛИЯ', c.age AS 'ВОЗРАСТ', c.state_id AS 'ID ШТАТА' FROM Citizen c";
        string sqlStates = "SELECT id AS 'ID ШТАТА', name AS 'НАЗВАНИЕ ШТАТА' FROM States";
        string sqlCities = "SELECT c.id AS 'ID', c.name AS 'НАЗВАНИЕ ГОРОДА', c.foundation_year AS 'ГОД ОСНОВАНИЯ'," +
            " c.population AS 'НАСЕЛЕНИЕ', c.area AS 'ПЛОЩАДЬ В КМ2', c.mayor AS 'МЭР ГОРОДА', c.state_id AS 'ID ШТАТА' FROM Cities c";
        string sqlOldest = "SELECT * FROM oldestCities";

        DataSet ds = new DataSet();
        public MainWindow()
        {
            InitializeComponent();
            Load(null, null);
            ShowCitizens(null, null);
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var adapterCitizens = new SqlDataAdapter(sqlCitizens, connection);
                var adapterStates = new SqlDataAdapter(sqlStates, connection);
                var adapterCities = new SqlDataAdapter(sqlCities, connection);
                var adapterOldest = new SqlDataAdapter(sqlOldest, connection);
                ds.Clear();
                adapterCitizens.Fill(ds, "Citizen");
                adapterStates.Fill(ds, "States");
                adapterCities.Fill(ds, "Cities");
                adapterOldest.Fill(ds, "Oldest");
            }
        }

        private void ShowCitizens(object sender, RoutedEventArgs e)
        {
            mainPage.ItemsSource = ds.Tables["Citizen"].DefaultView;
        }

        private void ShowStates(object sender, RoutedEventArgs e)
        {
            mainPage.ItemsSource = ds.Tables["States"].DefaultView;
        }

        private void ShowCities(object sender, RoutedEventArgs e)
        {
            mainPage.ItemsSource = ds.Tables["Cities"].DefaultView;
        }

        private void TopOldest(object sender, RoutedEventArgs e)
        {
            mainPage.ItemsSource = ds.Tables["Oldest"].DefaultView;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (mainPage.ItemsSource == ds.Tables["Cities"].DefaultView)
                //{
                //    MessageBox.Show("Мы на городах");
                //}
                //else if (mainPage.ItemsSource == ds.Tables["States"])
                //{
                //    MessageBox.Show("Мы на штатах");
                //}
                //else if (mainPage.ItemsSource == ds.Tables["Citizen"].DefaultView)
                //{
                //    MessageBox.Show("Мы на жителях");
                //}

                var adapterCities = new SqlDataAdapter(sqlCities, connectionString);
                var cbCities = new SqlCommandBuilder(adapterCities);

                var adapterStates = new SqlDataAdapter(sqlStates, connectionString);
                var cbStates = new SqlCommandBuilder(adapterStates);

                var adapterCitizen = new SqlDataAdapter(sqlCitizens, connectionString);
                var cbCitizen = new SqlCommandBuilder(adapterCitizen);

                adapterStates.Update(ds.Tables["States"]);
                cbStates.GetUpdateCommand();

                adapterCities.Update(ds.Tables["Cities"]);
                cbCities.GetUpdateCommand();

                adapterCitizen.Update(ds.Tables["Citizen"]);
                cbCitizen.GetUpdateCommand();



                MessageBox.Show("Изменения сохранены!");
                Load(null, null);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewCity(object sender, RoutedEventArgs e)
        {
            InsertCity ic = new InsertCity();
            ic.ShowDialog();
            Load(null, null);
        }

        private void ActivateProc(object sender, RoutedEventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var sqlCommandProc = new SqlCommand("MakeShopGreatAgain", conn);
                sqlCommandProc.CommandType = CommandType.StoredProcedure;
                sqlCommandProc.ExecuteNonQuery();
                MessageBox.Show("Процедура применена. Данные из БД были удалены и заполнены исходными значениями.");
            }
            Load(null, null);
        }
    }
}
