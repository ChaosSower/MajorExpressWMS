using System.Windows.Controls;
using System.Windows;

namespace MajorExpressWMS.Views
{
    /// <summary>
    /// Страница главного окна <see cref="MainWindow"/>
    /// </summary>
    public partial class MainWindowPage : Page
    {
        public MainWindowPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие входа в аккаунт по кнопке "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._MainWindowFrame?.Navigate(new AuthorizationPage());
        }

        /// <summary>
        /// Событие создания аккаунта по кнопке "Зарегистрироваться"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._MainWindowFrame?.Navigate(new RegistrationPage());
        }
    }
}