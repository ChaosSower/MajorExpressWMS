using System.Windows.Controls;

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

            // Кнопки управления, отсортированные в порядке полномочий //

            Button ObserveRequestsButton = new() { Content = "Просмотр заявок", Margin = new(5), FontSize = 16 };
            Button TrackRequestButton = new() { Content = "Ослеживание заявки", Margin = new(5), FontSize = 16 };
            Button CreateRequestButton = new() { Content = "Создать заявку", Margin = new(5), FontSize = 16 };
            Button EditRequestButton = new() { Content = "Изменить заявку", Margin = new(5), FontSize = 16 };
            Button CloseRequestButton = new() { Content = "Закрыть заявку", Margin = new(5), FontSize = 16 };
            Button ManageUsersButton = new() { Content = "Управление пользователями", Margin = new(5), FontSize = 16 };

            Dictionary<string, List<Button>> UserOptionButtonDictionary = new()
            {
                { "Пользователь", [ ObserveRequestsButton, TrackRequestButton ] },
                { "Курьер", [ CreateRequestButton ] },
                { "Отправитель", [ EditRequestButton ] },
                { "Получатель", [ CloseRequestButton ] },
                { "Администратор", [ ObserveRequestsButton, TrackRequestButton, EditRequestButton, ManageUsersButton ] }
            };

            if (User.UserRole?.Role != null && UserOptionButtonDictionary.TryGetValue(User.UserRole.Role, out List<Button>? Buttons))
            {
                foreach (Button Button in Buttons)
                {
                    UserOptionsUniformGrid.Children.Add(Button);
                }
            }
        }
    }
}