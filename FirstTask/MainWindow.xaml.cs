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
using System.Data.Common;

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
            if (String.IsNullOrEmpty(DatabaseName))
                return false;
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
            Connection = new MySqlConnection(connstring);
            try
            {
                Connection.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
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
        DBConnection dbCon = DBConnection.Instance(); /* Класс соединения с БД */
        int level = 0; /* 0 - города, 1 - улицы, 2 - дома, 3 - квартиры */
        int id = 0; /* Id записи */

        public MainWindow()
        {
            InitializeComponent();          

            dbCon.Server = "127.0.0.1";
            dbCon.DatabaseName = "wpf";
            dbCon.UserName = "root";
            dbCon.Password = "";

            FillGrid(dbCon);
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

        public class streets
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }

            public streets(int Id, string Name, int Count)
            {
                this.Id = Id;
                this.Name = Name;
                this.Count = Count;
            }
        }

        public class houses
        {
            public int Id { get; set; }
            public string Number { get; set; }
            public int Count { get; set; }

            public houses(int Id, string Number, int Count)
            {
                this.Id = Id;
                this.Number = Number;
                this.Count = Count;
            }
        }

        public class apartments
        {
            public int Id { get; set; }
            public float area { get; set; }
            public int houseId { get; set; }
            
            public apartments(int Id, float area, int houseId)
            {
                this.Id = Id;
                this.area = area;
                this.houseId = houseId;
            }
        }

        /* Заполнить таблицу 1 уровня */
        public void FillGrid(DBConnection dbCon)
        {
           string sql = "SELECT cities.id AS 'Id города', cities.name AS 'Название города', COUNT(streets.city_id) AS 'Количество улиц' " +
                "FROM cities, streets " +
                "WHERE cities.id = streets.city_id " +
                "GROUP BY cities.id";

            ExecuteQuery(sql);
        }

        /* Заполнить таблицу 2 уровня */
        public void FillDrid2(DBConnection dbCon)
        {
            string sql = "SELECT streets.id AS 'Id улицы', streets.name AS 'Название улицы', COUNT(houses.id) AS 'Количество домов' " +
                    "FROM streets, houses " +
                    "WHERE streets.city_id = " + id + " AND houses.street_id = streets.id " +
                    "GROUP BY streets.id;";

            ExecuteQuery(sql);
        }

        /* Заполнить таблицу 3 уровня */
        public void FillDrid3(DBConnection dbCon)
        {

        }

        /* Заполнить таблицу 4 уровня */
        public void FillDrid4(DBConnection dbCon)
        {

        }

        public void ExecuteQuery(string sql)
        {
            if (dbCon.IsConnect())
            {
                DataSet dataSet = new DataSet();
                DataAdapter dataAdapter = new MySqlDataAdapter(sql, dbCon.Connection.ConnectionString);
                dataAdapter.Fill(dataSet);
                Table.ItemsSource = dataSet.Tables[0].DefaultView;

                dbCon.Close();
            }
            else
            {
                MessageBox.Show("Нет соединения с БД", "Ошибка");
            }
        }

        /* Двойное нажатие по DataGrid для получения id строки */
        private void Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = Table.SelectedItem as DataRowView;
            //MessageBox.Show(row.Row.ItemArray[0].ToString());
            id = Convert.ToInt32(row.Row.ItemArray[0]);
            level += 1;

            switch (level) 
            {
                case 0:
                    {
                        FillGrid(dbCon);
                        break;
                    }
                case 1:
                    {
                        FillDrid2(dbCon); 
                        break;
                    }
                case 2: 
                    { 
                        FillDrid3(dbCon);
                        break;
                    }
                case 3:
                    {
                        FillDrid4(dbCon);
                        break;
                    }
            }
        }
    }
}