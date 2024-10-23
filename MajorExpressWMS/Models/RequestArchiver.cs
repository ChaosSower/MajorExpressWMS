using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка архиватора заявки таблицы <see cref="ApplicationContext.RequestArchiver"/>
    /// </summary>
    internal class RequestArchiver : IDatabase
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
        /// Навигационное свойство архивных заявок пользователя
        /// </summary>
        public ICollection<ArchiveRequest>? ArchivedRequests { get; set; }
    }
}