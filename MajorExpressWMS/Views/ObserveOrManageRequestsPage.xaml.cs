using MajorExpressWMS.Models;
using MajorExpressWMS.ViewModels;
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

namespace MajorExpressWMS.Views
{
    /// <summary>
    /// Страница просмотра/управления заявками
    /// </summary>
    public partial class ObserveOrManageRequestsPage : Page
    {
        internal ObserveOrManageRequestsPage(User? User = null)
        {
            InitializeComponent();

            if (User != null)
            {
                PageTitleTextBlock.Text = "Управление заявками";
                ObserveOrManageRequestsPageDataGrid.Columns[^1].Visibility = Visibility.Visible;
            }

            else
            {
                PageTitleTextBlock.Text = "Просмотр заявок";
            }

            var requestsData = MainWindow.ApplicationContext?.Requests
                .Select(request => new RequestViewModel(User)
                {
                    Number = request.Number,
                    RequestType = request.RequestType.Type,
                    RequestStatus = request.RequestStatus.Status,
                    Creator = request.Creator.User.Surname + " " + request.Creator.User.Name + " " + request.Creator.User.Patronymic,
                    CreationDate = request.CreationDate.ToString("yyyy.MM.dd"),
                    Executor = request.Executor.User.Surname + " " + request.Executor.User.Name + " " + request.Executor.User.Patronymic,
                    Company = request.Company.Name
                }).ToList();

            ObserveOrManageRequestsPageDataGrid.ItemsSource = requestsData;
            //ObserveOrManageRequestsPageDataGrid.ItemsSource = ???;
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