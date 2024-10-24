using MajorExpressWMS.Models;

namespace MajorExpressWMS.ViewModels
{
    /// <summary>
    /// Модель представления регистрации пользователя
    /// </summary>
    internal class UserRegistrationViewModel
    {
        /// <summary>
        /// Конструктор модели представления регистрации пользователя
        /// </summary>
        /// <param name="UserData">Данные регистрируемого пользователя</param>
        public UserRegistrationViewModel(params object[] UserData)
        {
            User NewUser = new()
            {
                Name = (string)UserData[0],
                Surname = (string)UserData[1],
                Patronymic = string.IsNullOrEmpty((string)UserData[2]) ? null : (string)UserData[2],
                Login = (string)UserData[3],
                Password = (string)UserData[4],
                UserRoleID = (int)UserData[5]
            };

            MainWindow.ApplicationContext?.Users.Add(NewUser);
            MainWindow.ApplicationContext?.SaveChanges();
        }
    }
}