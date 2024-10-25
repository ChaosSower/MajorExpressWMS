using MajorExpressWMS.Data;
using MajorExpressWMS.Models;
using MajorExpressWMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MajorExpressWMS.ViewModels
{
    public class RequestViewModel
    {
        public string Number { get; set; }
        public string RequestType { get; set; }
        public string RequestStatus { get; set; }
        public string Creator { get; set; }
        public string CreationDate { get; set; }
        public string Executor { get; set; }
        public string Company { get; set; }

        private User? User;

        public ICommand ReadRequestCommand { get; }
        public ICommand UpdateRequestCommand { get; }
        public ICommand DeleteRequestCommand { get; }

        internal RequestViewModel(User? User = null)
        {
            this.User = User;
            ReadRequestCommand = new RelayCommand(ReadRequest);
            UpdateRequestCommand = new RelayCommand(UpdateRequest);
            DeleteRequestCommand = new RelayCommand(DeleteRequest);
        }

        private void ReadRequest(object? parameter)
        {
            // Логика для чтения информации о заявке
        }

        private void UpdateRequest(object? parameter)
        {
            var request = MainWindow.ApplicationContext?.Requests.FirstOrDefault(r => r.Number == this.Number);

            MainWindow._MainWindowFrame.Navigate(new CreateOrEditRequestPage(User, request.ID));
        }

        private async void DeleteRequest(object? parameter)
        {
            var messageBox = new MessageBoxWithTextBox();
            var (delete, text) = await messageBox.ShowDialogAsync();

            if (delete)
            {
                var request = MainWindow.ApplicationContext?.Requests.FirstOrDefault(r => r.Number == this.Number);
                if (request != null && User != null)
                {
                    // Проверка существования RequestArchiver
                    var archiver = MainWindow.ApplicationContext?.RequestArchivers.FirstOrDefault(a => a.UserID == User.ID);
                    if (archiver == null)
                    {
                        // Создание нового RequestArchiver
                        archiver = new RequestArchiver { UserID = User.ID };
                        MainWindow.ApplicationContext?.RequestArchivers.Add(archiver);
                        MainWindow.ApplicationContext?.SaveChanges();
                    }

                    // Создаём архивную запись
                    var archiveRequest = new ArchiveRequest
                    {
                        RequestNumber = request.Number,
                        RequestTypeID = request.RequestTypeID,
                        CreatorID = request.CreatorID,
                        CreationDate = request.CreationDate,
                        ArchiverID = archiver.ID, // Убедись, что поле ArchiverID заполняется правильно
                        ArchiveDate = DateTime.Now,
                        Description = text
                    };

                    MainWindow.ApplicationContext?.ArchiveRequests.Add(archiveRequest);

                    MainWindow.ApplicationContext?.Requests.Remove(request);
                    MainWindow.ApplicationContext?.SaveChanges();

                    MessageBox.Show("Заявка удалена.");
                }
            }
        }
    }
}