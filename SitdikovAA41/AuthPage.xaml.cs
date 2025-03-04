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

namespace SitdikovAA41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = log_TextChanged;
            string password = par_TextChanged;
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            User user = Sitdi41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                log_TextChanged = "";
                par_TextChanged = "";
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                loginBtn.IsEnabled = false;
                loginBtn.IsEnabled = true;
            }
        }

        private void log_TextChanged(object sender, TextChangedEventArgs e)
        {

            string username = log.Text; // Получаем текст из TextBox

            // Проверяем, не пустой ли логин
            if (!string.IsNullOrWhiteSpace(username))
            {
                bool isValid = CheckUsernameInDatabase(username); // Проверяем наличие логина в базе данных
                loginBtn.IsEnabled = isValid; // Включаем или отключаем кнопку входа
            }
            else
            {
                loginBtn.IsEnabled = false; // Если поле пустое, отключаем кнопку
            }
        }

        private bool CheckUsernameInDatabase(string username)
        {
            using (var context = new Sitdi41Entities()) // Создаем экземпляр контекста базы данных
            {
                // Проверяем, существует ли пользователь с таким логином
                return context.User.Any(u => u.Username == username);
            }
        }
    }
        

        private void par_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
