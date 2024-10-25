using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка создателя заявки таблицы <see cref="ApplicationContext.RequestCreators"/>
    /// </summary>
    internal class RequestCreator : IDatabase
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

        /// <summary>
        /// Навигационное свойство архивных заявок у пользователя
        /// </summary>
        public ICollection<ArchiveRequest>? ArchiveRequests { get; set; }
    }
}