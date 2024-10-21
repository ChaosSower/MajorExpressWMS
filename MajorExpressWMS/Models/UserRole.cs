using System.ComponentModel.DataAnnotations;

using MajorExpressWMS.Data;
using MajorExpressWMS.Interfaces;

namespace MajorExpressWMS.Models
{
    /// <summary>
    /// Строка роли пользователя таблицы <see cref="ApplicationContext.UserRoles"/>
    /// </summary>
    internal class UserRole : IDatabase
    {
        public int ID { get; private set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        [MaxLength(50)]
        public required string Role { get; set; }

        // Навигационные свойства //

        /// <summary>
        /// Навигационное свойство пользователей с ролями
        /// </summary>
        public ICollection<User>? Users { get; set; }
    }
}