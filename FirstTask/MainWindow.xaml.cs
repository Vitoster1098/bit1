using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using MySql.Data;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FirstTask
{

    public class DBConnection
    {
        private DBConnection() { }

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public MySqlConnection Connection { get; set; }
        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }

            return true;
        }

        public void Close()
        {
            Connection.Close();
        }
    }

        /// <summary>
        /// Логика взаимодействия для MainWindow.xaml
        /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillGrid();
        }

        public class cities
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }

            public cities(int Id, string Name, int Count)
            {
                this.Id = Id;
                this.Name = Name;
                this.Count = Count;
            }
        }

        public void FillGrid()
        {
            var dbCon = DBConnection.Instance();
            dbCon.Server = "127.0.0.1";
            dbCon.DatabaseName = "wpf";
            dbCon.UserName = "root";
            dbCon.Password = "";

            if(dbCon.IsConnect())
            {
                Console.WriteLine("Connected to db");

                string sql = "SELECT cities.id AS 'Id города', cities.name AS 'Название города', COUNT(streets.city_id) AS 'Количество улиц' " +
                "FROM cities, streets " +
                "WHERE cities.id = streets.city_id " +
                "GROUP BY cities.id";

                List<cities> citiesList = new List<cities>();

                var cmd = new MySqlCommand(sql, dbCon.Connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    citiesList.Add(new cities(Convert.ToInt32(reader.GetString(0)), reader.GetString(1), Convert.ToInt32(reader.GetString(2))));
                }

                Table.ItemsSource = citiesList;
                dbCon.Close();
            }
        }
    }
}