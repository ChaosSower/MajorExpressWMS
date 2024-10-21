using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка архивной заявки таблицы <see cref="ApplicationContext.ArchiveRequests"/>
    /// </summary>
    internal class ArchiveRequest : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Номер архивной заявки заявки
        /// </summary>
        [Required]
        public required string RequestNumber { get; set; }

        /// <summary>
        /// Дата помещения в архив
        /// </summary>
        [Column(TypeName = "date")]
        [Required]
        public required DateTime ArchiveDate { get; set; }

        /// <summary>
        /// Описание заявки (комментарий)
        /// </summary>
        public string? Description { get; set; }
    }
}