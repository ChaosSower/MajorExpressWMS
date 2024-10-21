using System.ComponentModel.DataAnnotations;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка типа заявки таблицы <see cref="ApplicationContext.RequestTypes"/>
    /// </summary>
    internal class RequestType : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Type { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство заявок с типами
        /// </summary>
        public ICollection<Request>? Requests { get; set; }
    }
}