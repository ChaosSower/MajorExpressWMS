using System.ComponentModel.DataAnnotations;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка компании таблицы <see cref="ApplicationContext.Companies"/>
    /// </summary>
    internal class Company : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Название компании
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство компаний по заявкам
        /// </summary>
        public ICollection<Request>? Requests { get; set; }
    }
}