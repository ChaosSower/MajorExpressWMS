using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка пользователя таблицы <see cref="ApplicationContext.Users"/>
    /// </summary>
    [Index(nameof(Login), IsUnique = true)]
    internal class User : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Surname { get; set; }

        /// <summary>
        /// Отчество пользователя (при наличии)
        /// </summary>
        [MaxLength(50)]
        public string? Patronymic { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [MaxLength(50)]
        [Required]
        public required string Password { get; set; }

        /// <summary>
        /// ID роли пользователя
        /// </summary>
        [Required]
        public required int UserRoleID { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство роли пользователя
        /// </summary>
        public UserRole? UserRole { get; set; }
    }
}