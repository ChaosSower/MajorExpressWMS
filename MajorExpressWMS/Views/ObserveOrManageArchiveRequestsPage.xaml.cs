using MajorExpressWMS.Models;
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
    /// Страница просмотра/управления архивными заявками
    /// </summary>
    public partial class ObserveOrManageArchiveRequestsPage : Page
    {
        internal ObserveOrManageArchiveRequestsPage(User? User = null)
        {
            InitializeComponent();

            if (User != null)
            {
                PageTitleTextBlock.Text = "Управление архивными заявками";
                ObserveOrManageArchiveRequestsPageDataGrid.Columns[^1].Visibility = Visibility.Visible;
            }

            else
            {
                PageTitleTextBlock.Text = "Просмотр архивных заявок";
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