using System.ComponentModel.DataAnnotations;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка статуса заявки таблицы <see cref="ApplicationContext.RequestStatuses"/>
    /// </summary>
    internal class RequestStatus : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        [MaxLength(50)]
        public required string Status { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство заявок со статусами
        /// </summary>
        public ICollection<Request>? Requests { get; set; }
    }
}