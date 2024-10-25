using System.Windows.Controls;
using System.Windows;

using Microsoft.EntityFrameworkCore;

using MajorExpressWMS.Models;
using MajorExpressWMS.ViewModels;

namespace MajorExpressWMS.Views
{
    /// <summary>
    /// Страница создания/редактирования заявки
    /// </summary>
    public partial class CreateOrEditRequestPage : Page
    {
        /// <summary>
        /// Поле авторизованного пользователя, использующего страницу <see cref="CreateOrEditRequestPage"/>
        /// </summary>
        private readonly User User;

        /// <summary>
        /// ID заявки (для поиска в случае, если редактируется существующая)
        /// </summary>
        private readonly int? RequestID;

        /// <summary>
        /// Конструктор страницы создания/редактирования заявки
        /// </summary>
        /// <param name="User">Пользователь, выполняющий действия на странице</param>
        /// <param name="RequestID">ID заявки (в случае, если редактируется существующая)</param>
        internal CreateOrEditRequestPage(User User, int? RequestID = null)
        {
            InitializeComponent();

            this.User = User;
            this.RequestID = RequestID;

            TypeComboBox.ItemsSource = MainWindow.ApplicationContext?.RequestTypes
                .Select(RequestType => RequestType.Type)
                .ToList();

            StatusComboBox.ItemsSource = MainWindow.ApplicationContext?.RequestStatuses
                .Select(RequestStatus => RequestStatus.Status)
                .ToList();

            ExecutorComboBox.ItemsSource = MainWindow.ApplicationContext?.RequestExecutors
                .Include(RequestExecutor => RequestExecutor.User)
                .Select(RequestExecutor => string.Join(" ", new[] { RequestExecutor.User!.Surname, RequestExecutor.User.Name, RequestExecutor.User.Patronymic }).TrimEnd())
                .ToList();

            CompanyComboBox.ItemsSource = MainWindow.ApplicationContext?.Companies
                .Select(Company => Company.Name)
                .ToList();

            if (RequestID.HasValue)
            {
                var request = MainWindow.ApplicationContext?.Requests.FirstOrDefault(r => r.ID == this.RequestID);
                PageTitleTextBlock.Text = "Редакция заявки";
                CreateOrEditRequestButton.Content = "Изменить заявку";

                if (request != null)
                {
                    NumberTextBox.Text = request.Number;
                    TypeComboBox.SelectedIndex = request.RequestTypeID - 1;
                    ExecutorComboBox.SelectedIndex = request.ExecutorID - 1;
                    CompanyComboBox.SelectedIndex = request.CompanyID - 1;

                    if (request.RequestStatusID != 1)
                    {
                        NumberTextBox.IsReadOnly = true;
                        TypeComboBox.IsEnabled = false;
                        ExecutorComboBox.IsEnabled = false;
                        CompanyComboBox.IsEnabled = false;
                    }

                    StatusComboBox.SelectedIndex = request.RequestStatusID - 1;
                }
            }

            else
            {
                StatusStackPanel.Visibility = Visibility.Collapsed;

                PageTitleTextBlock.Text = "Создание заявки";
                CreateOrEditRequestButton.Content = "Создать заявку";
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

        /// <summary>
        /// Событие нажатия на кнопку создания/редакции заявки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateOrEditRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumberTextBox.Text) || string.IsNullOrWhiteSpace(TypeComboBox.Text) || string.IsNullOrWhiteSpace(ExecutorComboBox.Text) || string.IsNullOrWhiteSpace(CompanyComboBox.Text))
            {
                return;
            }

            try
            {
                _ = new RequestCreationOrEditionViewModel
                (
                    RequestID,
                    NumberTextBox.Text,
                    TypeComboBox.SelectedIndex + 1,
                    StatusComboBox.SelectedIndex == -1 ? StatusComboBox.SelectedIndex : StatusComboBox.SelectedIndex + 1,
                    User,
                    ExecutorComboBox.Text,
                    CompanyComboBox.Text
                );

                string MessageBoxText = RequestID.HasValue ? "изменена" : "зарегистрирована и добавлена в базу данных";

                MessageBox.Show($"Заявка была успешно {MessageBoxText}!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow._MainWindowFrame?.GoBack();
            }

            catch
            {
                string MessageBoxText = RequestID.HasValue ? "изменить" : "зарегистрировать";
                string MessageBoxCaption = RequestID.HasValue ? "редактирования" : "добавления";

                MessageBox.Show($"Не удалось {MessageBoxText} заявку!", $"Ошибка {MessageBoxCaption} заявки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}