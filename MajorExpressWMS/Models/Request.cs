using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка заявки таблицы <see cref="ApplicationContext.Requests"/>
    /// </summary>
    internal class Request : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Номер заявки
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Number { get; set; }

        /// <summary>
        /// ID типа заявки
        /// </summary>
        [Required]
        public required int RequestTypeID { get; set; }

        /// <summary>
        /// ID статуса заявки
        /// </summary>
        [Required]
        public required int RequestStatusID { get; set; }

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
        /// ID исполнителя заявки
        /// </summary>
        [Required]
        public required int ExecutorID { get; set; }

        /// <summary>
        /// ID компании
        /// </summary>
        [Required]
        public required int CompanyID { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство типа заявки
        /// </summary>
        public RequestType? RequestType { get; set; }

        /// <summary>
        /// Навигационное свойство статуса заявки
        /// </summary>
        public RequestStatus? RequestStatus { get; set; }

        /// <summary>
        /// Навигационное свойство создателя заявки (пользователь)
        /// </summary>
        public RequestCreator? Creator { get; set; }

        /// <summary>
        /// Навигационное свойство исполнителя заявки (пользователь)
        /// </summary>
        public RequestExecutor? Executor { get; set; }

        /// <summary>
        /// Навигационное свойство компании
        /// </summary>
        public Company? Company { get; set; }
    }
}