using System.Windows.Controls;
using System.Windows;

using MajorExpressWMS.ViewModels;

namespace MajorExpressWMS.Views
{
    /// <summary>
    /// Страница регистрации пользователя
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();

            RoleComboBox.ItemsSource = MainWindow.ApplicationContext?.UserRoles
                .Select(UserRole => UserRole.Role)
                .ToList();
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
        /// Событие создания аккаунта по кнопке "Создать аккаунт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(SurnameTextBox.Text) || string.IsNullOrWhiteSpace(RoleComboBox.Text))
            {
                return;
            }

            try
            {
                _ = new UserRegistrationViewModel
                (
                    NameTextBox.Text,
                    SurnameTextBox.Text,
                    PatronymicTextBox.Text,
                    LoginTextBox.Text,
                    PasswordTextBox.Text,
                    RoleComboBox.SelectedIndex + 1
                );

                MessageBox.Show("Пользователь был успешно зарегистрирован и добавлен в базу данных!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow._MainWindowFrame?.GoBack();
            }

            catch
            {
                MessageBox.Show("Не удалось зарегистрировать пользователя!\n\nВозможно, данный логин уже занят", "Ошибка регистрации пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}