using System.Windows;
using System.Windows.Controls;

using MajorExpressWMS.Data;
using MajorExpressWMS.Views;

namespace MajorExpressWMS
{
    /// <summary>
    /// Главное окно приложения
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame? _MainWindowFrame;
        internal static ApplicationContext? ApplicationContext { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            bool IsDatabaseCreated;
            ApplicationContext = new(out IsDatabaseCreated);

            _MainWindowFrame = MainWindowFrame;

            MainWindowFrame.Navigate(new MainWindowPage(IsDatabaseCreated));
        }
    }
}