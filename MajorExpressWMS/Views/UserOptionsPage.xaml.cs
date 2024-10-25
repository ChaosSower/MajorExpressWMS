using System.Windows.Controls;
using System.Collections.Frozen;
using System.Windows;

using MajorExpressWMS.Models;

namespace MajorExpressWMS.Views
{
    /// <summary>
    /// Страница опций пользователя
    /// </summary>
    public partial class UserOptionsPage : Page
    {
        internal UserOptionsPage(User User)
        {
            InitializeComponent();

            UserDataTextBlock.Text = $"Вы авторизовались как: {string.Join(" ", [User.Surname, User.Name, User.Patronymic]).TrimEnd()}";
            UserRoleTextBlock.Text = $"Ваша роль: {User.UserRole?.Role}";

            // Кнопки управления (отсортированы в порядке полномочий) //

            Button ObserveRequestsButton = new() { Content = "Просмотр заявок", Margin = new(25), FontSize = 16 };
            Button ObserveArchiveRequestsButton = new() { Content = "Просмотр архивных заявок", Margin = new(25), FontSize = 16 };
            Button CreateRequestButton = new() { Content = "Создать заявку", Margin = new(25), FontSize = 16 };
            Button ManageRequestsButton = new() { Content = "Управление заявками", Margin = new(25), FontSize = 16 };
            Button ManageArchiveRequestsButton = new() { Content = "Управление архивными заявками", Margin = new(25), FontSize = 16 };
            Button ManageUsersButton = new() { Content = "Управление пользователями", Margin = new(25), FontSize = 16 };

            ObserveRequestsButton.Click += (sender, e) => MainWindow._MainWindowFrame?.Navigate(new ObserveOrManageRequestsPage());
            CreateRequestButton.Click += (sender, e) => MainWindow._MainWindowFrame?.Navigate(new CreateOrEditRequestPage(User));
            ManageRequestsButton.Click += (sender, e) => MainWindow._MainWindowFrame?.Navigate(new ObserveOrManageRequestsPage(User));
            ManageUsersButton.Click += (sender, e) => MainWindow._MainWindowFrame?.Navigate(new ManageUsersPage());

            Dictionary<string, List<Button>> _UserOptionButtonDictionary = new()
            {
                { "Пользователь", [ ObserveRequestsButton ] },
                { "Курьер", [ ObserveRequestsButton ] },
                { "Отправитель", [ ObserveRequestsButton, ObserveArchiveRequestsButton, CreateRequestButton, ManageRequestsButton ] },
                { "Получатель", [ ObserveRequestsButton, ObserveArchiveRequestsButton, ManageRequestsButton ] },
                { "Администратор", [ ObserveRequestsButton, ManageRequestsButton, ManageArchiveRequestsButton, ManageUsersButton ] }
            };

            FrozenDictionary<string, List<Button>> UserOptionButtonDictionary = _UserOptionButtonDictionary.ToFrozenDictionary();

            if (User.UserRole?.Role != null && UserOptionButtonDictionary.TryGetValue(User.UserRole.Role, out List<Button>? Buttons))
            {
                foreach (Button Button in Buttons)
                {
                    UserOptionsUniformGrid.Children.Add(Button);
                }
            }
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
    }
}