using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка исполнителя заявки таблицы <see cref="ApplicationContext.RequestExecutors"/>
    /// </summary>
    internal class RequestExecutor : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// ID пользователя из таблицы <see cref="ApplicationContext.Users"/>
        /// </summary>
        public int UserID { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство пользователя
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Навигационное свойство заявок у пользователя
        /// </summary>
        public ICollection<Request>? Requests { get; set; }
    }
}