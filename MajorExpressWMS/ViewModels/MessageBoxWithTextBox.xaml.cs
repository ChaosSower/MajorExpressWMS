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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace MajorExpressWMS.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxWithTextBox.xaml
    /// </summary>
    public partial class MessageBoxWithTextBox : Window
    {
        public bool Action { get; private set; }
        public string? Text { get; private set; }

        public MessageBoxWithTextBox()
        {
            InitializeComponent();
        }

        public async Task<(bool Action, string? Text)> ShowDialogAsync()
        {
            var tcs = new TaskCompletionSource<(bool, string?)>();
            this.Closed += (sender, args) => tcs.TrySetResult((Action, Text));
            this.ShowDialog();
            return await tcs.Task;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Action = false;
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Action = true;
            Text = InputTextBox.Text; // Предполагается наличие TextBox с именем InputTextBox
            this.Close();
        }
    }
}