using System.Windows.Controls;
using System.Windows;

using MajorExpressWMS.Models;
using Microsoft.EntityFrameworkCore;

namespace MajorExpressWMS.Views
{
    /// <summary>
    /// Страница авторизации пользователя
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие нажатия на кнопку "Назад"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._MainWindowFrame?.GoBack();
        }

        /// <summary>
        /// Событие входа в аккаунт по кнопке "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) && !string.IsNullOrWhiteSpace(PasswordPasswordBox.Password))
            {
                MessageBox.Show("Пожалуйста, введите логин!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!string.IsNullOrWhiteSpace(LoginTextBox.Text) && string.IsNullOrWhiteSpace(PasswordPasswordBox.Password))
            {
                MessageBox.Show("Пожалуйста, введите пароль!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) && string.IsNullOrWhiteSpace(PasswordPasswordBox.Password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            User? User = MainWindow._ApplicationContext?.Users.Include(User => User.UserRole).FirstOrDefault(User => User.Login == LoginTextBox.Text);

            if (User != null)
            {
                if (User.Password == PasswordPasswordBox.Password)
                {
                    MessageBox.Show($"Добро пожаловать, {string.Join(" ", [User.Surname, User.Name, User.Patronymic]).TrimEnd()}!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow._MainWindowFrame?.Navigate(new UserOptionsPage(User));
                }

                else
                {
                    MessageBox.Show("Неверный пароль!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            else
            {
                MessageBox.Show("Пользователь с таким логином не найден!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}