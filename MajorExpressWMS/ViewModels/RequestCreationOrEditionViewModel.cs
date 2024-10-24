using Microsoft.EntityFrameworkCore;

using MajorExpressWMS.Models;

namespace MajorExpressWMS.ViewModels
{
    /// <summary>
    /// Модель представления создаваемой/редактируемой заявки
    /// </summary>
    internal class RequestCreationOrEditionViewModel
    {
        /// <summary>
        /// Конструктор модели представления создаваемой/редактируемой заявки
        /// </summary>
        /// <param name="RequestID">ID заявки (если редактируется созданная)</param>
        /// <param name="RequestData">Параметры создаваемой/редактируемой заявки</param>
        public RequestCreationOrEditionViewModel(int? RequestID, params object[] RequestData)
        {
            User? User = RequestData.OfType<User>().FirstOrDefault();

            if (User != null)
            {
                RequestCreator? Creator;

                Creator = MainWindow.ApplicationContext?.RequestCreators
                    .Include(RequestCreator => RequestCreator.User)
                    .FirstOrDefault(RequestCreator => RequestCreator.UserID == User.ID);

                string ExecutorFullName = (string)RequestData[4];

                string[] ExecutorFullNameParts = ExecutorFullName.Split(' ');
                string ExecutorName = ExecutorFullNameParts[1];
                string ExecutorSurname = ExecutorFullNameParts[0];
                string? ExecutorPatronymic = null;

                if (ExecutorFullNameParts.Length == 3)
                {
                    ExecutorPatronymic = ExecutorFullNameParts[2];
                }

                User? ExecuterUser = MainWindow.ApplicationContext?.Users
                    .Where(User => User.Name == ExecutorName && User.Surname == ExecutorSurname && User.Patronymic == ExecutorPatronymic)
                    .FirstOrDefault();

                Company? Company = MainWindow.ApplicationContext?.Companies
                    .Where(_Company => _Company.Name == (string)RequestData[5])
                    .FirstOrDefault();

                if (Creator == null)
                {
                    MainWindow.ApplicationContext?.RequestCreators.Add(new() { UserID = User.ID });
                    MainWindow.ApplicationContext?.SaveChanges();

                    Creator = MainWindow.ApplicationContext?.RequestCreators
                        .Include(RequestCreator => RequestCreator.User)
                        .FirstOrDefault(RequestCreator => RequestCreator.UserID == User.ID);
                }

                if (Creator != null && ExecuterUser != null && Company != null)
                {
                    RequestExecutor? Executor = MainWindow.ApplicationContext?.RequestExecutors
                        .FirstOrDefault(RequestExecutor => RequestExecutor.UserID == ExecuterUser.ID);

                    if (Executor != null)
                    {
                        Request Request = new()
                        {
                            Number = (string)RequestData[0],
                            RequestTypeID = (int)RequestData[1],
                            RequestStatusID = (int)RequestData[2] == -1 ? 1 : (int)RequestData[2],
                            CreatorID = Creator.ID,
                            CreationDate = DateTime.Today,
                            ExecutorID = Executor.ID,
                            CompanyID = Company.ID
                        };

                        if (RequestID.HasValue) // редакция существующей заявки
                        {
                            Request? ExistingRequest = MainWindow.ApplicationContext?.Requests
                                .FirstOrDefault(Request => Request.ID == RequestID);

                            if (ExistingRequest != null)
                            {
                                ExistingRequest.Number = Request.Number;
                                ExistingRequest.RequestTypeID = Request.RequestTypeID;
                                ExistingRequest.RequestStatusID = Request.RequestStatusID;
                                ExistingRequest.CreatorID = Request.CreatorID;
                                ExistingRequest.CreationDate = Request.CreationDate;
                                ExistingRequest.ExecutorID = Request.ExecutorID;
                                ExistingRequest.CompanyID = Request.CompanyID;

                                MainWindow.ApplicationContext?.Requests.Update(ExistingRequest);
                            }
                        }

                        else // добавление новой заявки
                        {
                            MainWindow.ApplicationContext?.Requests.Add(Request);
                        }

                        MainWindow.ApplicationContext?.SaveChanges();
                    }
                }
            }
        }
    }
}