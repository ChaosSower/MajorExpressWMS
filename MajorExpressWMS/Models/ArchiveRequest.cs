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
        /// ID типа заявки
        /// </summary>
        [Required]
        public required int RequestTypeID { get; set; }

        /// <summary>
        /// ID создателя заявки
        /// </summary>
        [Required]
        public required int CreatorID { get; set; }

        /// <summary>
        /// Дата создания заявки
        /// </summary>
        [Column(TypeName = "date")]
        [Required]
        public required DateTime CreationDate { get; set; }

        /// <summary>
        /// ID инициатора архивации заявки
        /// </summary>
        [Required]
        public required int ArchiverID { get; set; }

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

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство типа заявки
        /// </summary>
        public RequestType? RequestType { get; set; }

        /// <summary>
        /// Навигационное свойство создателя заявки
        /// </summary>
        public RequestCreator? Creator { get; set; }

        /// <summary>
        /// Навигационное свойство инициатора архивации заявки
        /// </summary>
        public RequestArchiver? Archiver { get; set; }
    }
}