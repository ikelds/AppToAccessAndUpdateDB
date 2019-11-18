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
using System.Data;

namespace WpfApp13
{
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {      
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDb;
                Initial Catalog=ITPark;Integrated Security=True");
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                String query = "SELECT COUNT(1) FROM users " +
                    "WHERE login = @Username AND pass = @Password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", txtPass.Password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверные имя пользователя или пароль", "Подключение к БД");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удается подключиться к БД. Проверьте существование базы данных ITPark.\n\nОписание ошибки:\n" + ex.Message, "Ошибка при подключении к БД");
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}
