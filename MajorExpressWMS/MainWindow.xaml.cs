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
        internal static ApplicationContext? _ApplicationContext;

        public MainWindow()
        {
            InitializeComponent();

            _ApplicationContext = new();

            _MainWindowFrame = MainWindowFrame;

            MainWindowFrame.Navigate(new MainWindowPage());
        }
    }
}