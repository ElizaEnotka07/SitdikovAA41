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
using System.Windows.Threading;

namespace SitdikovAA41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private string generatedCaptcha;
        private string txtCaptchaInput;
        private int blockTimer;
        private int failedAttempts = 0;
        public AuthPage()
        {
            InitializeComponent();

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = log.Text.Trim();
            string password = par.Text.Trim();
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            User user = Sitdi41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                login = "";
                password = "";
            }
            else
            {
                failedAttempts++;
                if (failedAttempts == 1)
                {
                    MessageBox.Show("Введены неверные данные! Введите капчу."); GenerateCaptcha();
                    CaptchaPanel.Visibility = Visibility.Visible;
                }
                else if (failedAttempts >= 2)
                {
                    if (cap.Text.Trim() != generatedCaptcha)
                    {
                        MessageBox.Show("Капча введена неверно! Блокировка входа на 10 секунд."); BlockBtn();
                    }
                    else
                    {
                        MessageBox.Show("Успешная проверка капчи, но логин или пароль неверны!");
                    }
                }
            }
           
        }
        

        private void GostBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));
            ResetAuthForm();
        }
        private void GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder captchaBuilder = new StringBuilder();
            Random random = new Random();

            // Генерация 4 случайных символов
            for (int i = 0; i < 4; i++)
            {
                captchaBuilder.Append(chars[random.Next(chars.Length)]);
            }

            generatedCaptcha = captchaBuilder.ToString();

            // Отображение капчи в TextBlock
            captchaOneWord.Text = generatedCaptcha[0].ToString();
            captchaTwoWord.Text = generatedCaptcha[1].ToString();
            captchaThreeWord.Text = generatedCaptcha[2].ToString();
            captchaFourWord.Text = generatedCaptcha[3].ToString();
        }

      private async void BlockBtn()
        {
            loginBtn.IsEnabled = false;
            await Task.Delay(10000);
            loginBtn.IsEnabled = true;
        }

        private void ResetAuthForm()
        {
            log.Clear();
            par.Clear();
            cap.Clear();
            CaptchaPanel.Visibility = Visibility.Collapsed;
            failedAttempts = 0;


        }



    }
}
