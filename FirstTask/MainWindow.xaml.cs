using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
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
        public string Charset { get; set; }

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
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}; charset={4};", Server, DatabaseName, UserName, Password, Charset);
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

        List<int> id_stack = new List<int>(); /* Симуляция стека для обращения к предыдущему уровню интерфейса по id */

        public MainWindow()
        {
            InitializeComponent();          

            dbCon.Server = "127.0.0.1";
            dbCon.DatabaseName = "wpf";
            dbCon.UserName = "root";
            dbCon.Password = "";
            dbCon.Charset = "utf8";

            FillGrid(dbCon);
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
                    "WHERE streets.city_id = " + id_stack.Last() + " AND houses.street_id = streets.id " +
                    "GROUP BY streets.id;";

            ExecuteQuery(sql);
        }

        /* Заполнить таблицу 3 уровня */
        public void FillDrid3(DBConnection dbCon)
        {
            string sql = "SELECT houses.id AS 'Id дома', houses.number AS 'Номер дома', COUNT(apartments.id) AS 'Количество квартир', " +
                "SUM(apartments.area) AS 'Общая площадь квартир' " +
                "FROM houses, apartments " +
                "WHERE houses.id = apartments.house_id AND houses.id = " + id_stack.Last() + " " +
                "GROUP BY houses.id";

            ExecuteQuery(sql);
        }

        /* Заполнить таблицу 4 уровня */
        public void FillDrid4(DBConnection dbCon)
        {
            string sql = "SELECT apartments.id AS 'Id квартиры', apartments.area AS 'Площадь квартиры' " +
                "FROM apartments, houses " +
                "WHERE apartments.area BETWEEN 0 AND 300 " +
                "AND houses.id = apartments.house_id;";

            ExecuteQuery(sql);
        }

        /* Выполнение SQL-запроса и вывод данных в DataGrid */
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

        /* Вызов отображения интерфейса в зависимости от уровня */
        public void ExecuteOutputTable(int lvl)
        {
            switch (lvl)
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

        /* Двойное нажатие по DataGrid для получения id строки */
        private void Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = Table.SelectedItem as DataRowView;
            //MessageBox.Show(row.Row.ItemArray[0].ToString());
            id = Convert.ToInt32(row.Row.ItemArray[0]);
            id_stack.Add(id);

            level += 1;

            if(level > 0)
            {
                BackButton.Visibility = Visibility.Visible;
            }
            if(level > 3)
            {
                level= 3;
            }            
            if(level == 3)
            {
                lbl1.Visibility= Visibility.Visible;
                lbl2.Visibility= Visibility.Visible;
                txtFrom.Visibility= Visibility.Visible;
                txtTo.Visibility= Visibility.Visible;
                diapason.Visibility= Visibility.Visible;
            }            

            ExecuteOutputTable(level);
        }

        /* Кнопка возврата интерфейса на уровень назад */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            level -= 1;
            id_stack.RemoveAt(id_stack.Count - 1);

            if (level < 0)
            {
                level = 0;
            }
            if (level == 0)
            {
                BackButton.Visibility = Visibility.Hidden;
            }
            if (level < 3)
            {
                lbl1.Visibility = Visibility.Hidden;
                lbl2.Visibility = Visibility.Hidden;
                txtFrom.Visibility = Visibility.Hidden;
                txtTo.Visibility = Visibility.Hidden;
                diapason.Visibility= Visibility.Hidden;

                txtFrom.Text= string.Empty;
                txtTo.Text= string.Empty;
            }

            ExecuteOutputTable(level);
        }

        /* Кнопка отобрать квартиры согласно диапазону */
        private void diapason_Click(object sender, RoutedEventArgs e)
        {
            double begin = 0, end = 1000000;
            try
            {
                begin = Convert.ToDouble(txtFrom.Text);
                end = Convert.ToDouble(txtTo.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }

            if(end < begin)
            {
                MessageBox.Show("Конец диапазона не может быть меньше начала", "Ошибка");
                return;
            }

            string sql = "SELECT apartments.id AS 'Id квартиры', apartments.area AS 'Площадь квартиры' " +
                "FROM apartments, houses " +
                "WHERE apartments.area BETWEEN " + Math.Floor(begin) + " AND " + Math.Floor(end) + " " +
                "AND houses.id = apartments.house_id;";

            ExecuteQuery(sql);
        }
    }
}