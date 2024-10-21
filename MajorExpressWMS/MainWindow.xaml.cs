using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MajorExpressWMS.Data;
using MajorExpressWMS.Models;

namespace MajorExpressWMS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using ApplicationContext ApplicationContext = new();
            List<User> p = [.. ApplicationContext.Users];
            foreach (var user in p) { Debug.WriteLine(user.Login); }
        }
    }
}