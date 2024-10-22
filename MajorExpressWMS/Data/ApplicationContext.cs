using System.IO;
using System.Windows;

using Microsoft.EntityFrameworkCore;

using MajorExpressWMS.Models;

namespace MajorExpressWMS.Data
{
    /// <summary>
    /// Класс контекста из БД
    /// </summary>
    internal class ApplicationContext : DbContext
    {
        /// <summary>
        /// Таблица "Роли пользователей"
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Таблица "Пользователи"
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица "Типы заявок"
        /// </summary>
        public DbSet<RequestType> RequestTypes { get; set; }

        /// <summary>
        /// Таблица "Статусы заявок"
        /// </summary>
        public DbSet<RequestStatus> RequestStatuses { get; set; }

        /// <summary>
        /// Таблица "Компании"
        /// </summary>
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Таблица "Заявки"
        /// </summary>
        public DbSet<Request> Requests { get; set; }

        /// <summary>
        /// Таблица "Архивные заявки"
        /// </summary>
        public DbSet<ArchiveRequest> ArchiveRequests { get; set; }

        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private string? DBPath { get; }

        public ApplicationContext()
        {
            string PathToExe = AppDomain.CurrentDomain.BaseDirectory;
            string FileOfDB = Path.Join(PathToExe, "MajorExpressDB_WMS/MajorExpressDB_WMS.txt");

            if (File.Exists(FileOfDB))
            {
                DBPath = File.ReadAllLines(FileOfDB)[0];

                Database.Migrate();
                Seed(this);
            }

            else
            {
                MessageBox.Show($"Похоже, что файл {FileOfDB.Split("/")[^1]} со строкой подключения отсутствует по полному пути:\n\n{FileOfDB}\n\nНе удалось создать/подключиться к базе данных", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Защищённый метод подключения к БД через строку подключения
        /// </summary>
        /// <param name="Options">Параметры для <see cref="DbContextOptionsBuilder"/></param>
        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            Options.UseSqlServer(DBPath);
        }

        /// <summary>
        /// Метод добавления данных по умолчанию в таблицы
        /// </summary>
        /// <param name="ApplicationContext"></param>
        private static void Seed(ApplicationContext ApplicationContext)
        {
            try
            {
                // Заполнение таблицы "Роли пользователей"
                if (!ApplicationContext.UserRoles.Any())
                {
                    List<UserRole> UserRoles =
                    [
                        new() { Role = "Пользователь" },
                        new() { Role = "Курьер" },
                        new() { Role = "Отправитель" },
                        new() { Role = "Получатель" },
                        new() { Role = "Администратор" }
                    ];

                    ApplicationContext.UserRoles.AddRange(UserRoles);

                    ApplicationContext.SaveChanges();
                }

                // Заполнение таблицы "Пользователи"
                if (!ApplicationContext.Users.Any())
                {
                    List<User> Users =
                    [
                        new() { Name = "Кирилл", Surname = "Евтюхин", Patronymic = "Олегович", Login = "EvtyKO", Password = "12345", UserRoleID = 1 },
                        new() { Name = "Иван", Surname = "Иванов", Patronymic = "Иванович", Login = "IvanII", Password = "12345", UserRoleID = 1 },
                        new() { Name = "Никита", Surname = "Лупахин", Patronymic = "Андреевич", Login = "LypaNA", Password = "23456", UserRoleID = 2 },
                        new() { Name = "Дмитрий", Surname = "Захаров", Patronymic = "Максимович", Login = "ZahaDM", Password = "23456", UserRoleID = 2 },
                        new() { Name = "Тимур", Surname = "Гнутин", Patronymic = "Иванович", Login = "GnutTI", Password = "34567", UserRoleID = 3 },
                        new() { Name = "Андрей", Surname = "Шелест", Patronymic = "Михаилович", Login = "ShelAM", Password = "34567", UserRoleID = 3 },
                        new() { Name = "Егор", Surname = "Бревнов", Patronymic = "Романович", Login = "BrevER", Password = "45678", UserRoleID = 4 },
                        new() { Name = "Константин", Surname = "Яносов", Patronymic = "Игоревич", Login = "YanoKI", Password = "45678", UserRoleID = 4 },
                        new() { Name = "Роман", Surname = "Щукин", Patronymic = "Дмитриевич", Login = "ChukRD", Password = "56789", UserRoleID = 5 },
                        new() { Name = "Анастасия", Surname = "Ромина", Patronymic = "Андреевна", Login = "RomiAA", Password = "56789", UserRoleID = 5 }
                    ];

                    ApplicationContext.Users.AddRange(Users);

                    ApplicationContext.SaveChanges();
                }

                // Заполнение таблицы "Компании"
                if (!ApplicationContext.Companies.Any())
                {
                    List<Company> Companies =
                    [
                        new() { Name = "Молот и наковальня" },
                        new() { Name = "Химера Экспресс" },
                        new() { Name = "Логистик Технолоджис" }
                    ];

                    ApplicationContext.Companies.AddRange(Companies);

                    ApplicationContext.SaveChanges();
                }

                // Заполнение таблицы "Типы заявок"
                if (!ApplicationContext.RequestTypes.Any())
                {
                    List<RequestType> RequestTypes =
                    [
                        new() { Type = "Отправка" },
                        new() { Type = "Доставка" },
                        new() { Type = "Получение" }
                    ];

                    ApplicationContext.RequestTypes.AddRange(RequestTypes);

                    ApplicationContext.SaveChanges();
                }

                // Заполнение таблицы "Статусы заявок"
                if (!ApplicationContext.RequestStatuses.Any())
                {
                    List<RequestStatus> RequestStatuses =
                    [
                        new() { Status = "Новая" },
                        new() { Status = "Передана на выполнение" },
                        new() { Status = "Выполнена" },
                        new() { Status = "Отменена" },
                        new() { Status = "Удалена" }
                    ];

                    ApplicationContext.RequestStatuses.AddRange(RequestStatuses);

                    ApplicationContext.SaveChanges();
                }

                // Заполнение таблицы "Архивные заявки"
                if (!ApplicationContext.ArchiveRequests.Any())
                {
                    List<ArchiveRequest> ArchiveRequests =
                    [
                        new() { RequestNumber = "2341_AP_DD_899", ArchiveDate = new(2022, 01, 12) },
                        new() { RequestNumber = "412_LP_901", ArchiveDate = new(2022, 07, 29), Description = "Отменена по причине: \"Груз повреждён при транспортировке\"" },
                        new() { RequestNumber = "117892_SA_LU_9", ArchiveDate = new(2023, 08, 05) },
                        new() { RequestNumber = "15789_F_9", ArchiveDate = new(2023, 10, 25) },
                        new() { RequestNumber = "87_LO_PO_83299", ArchiveDate = new(2024, 02, 17), Description = "Отменена по причине: \"Компания-контрагент рассторгнула договор\"" }
                    ];

                    ApplicationContext.ArchiveRequests.AddRange(ArchiveRequests);

                    ApplicationContext.SaveChanges();
                }

                // Заполнение таблицы "Заявки"
                if (!ApplicationContext.Requests.Any())
                {
                    List<Request> Requests =
                    [
                        new() { Number = "123_RU_313", RequestTypeID = 1, RequestStatusID = 1, CreationDate = new(2024, 01, 17), CompanyID = 1, ExecutorID = 1 },
                        new() { Number = "6313_RW_1583", RequestTypeID = 2, RequestStatusID = 3, CreationDate = new(2024, 01, 29), CompanyID = 3, ExecutorID = 5 },
                        new() { Number = "72_RL_OY_23", RequestTypeID = 3, RequestStatusID = 2, CreationDate = new(2024, 02, 15), CompanyID = 2, ExecutorID = 2 },
                        new() { Number = "6709134_UZ_59723", RequestTypeID = 2, RequestStatusID = 1, CreationDate = new(2024, 02, 11), CompanyID = 2, ExecutorID = 6 },
                        new() { Number = "735304_KE_9", RequestTypeID = 2, RequestStatusID = 5, CreationDate = new(2024, 03, 24), CompanyID = 3, ExecutorID = 7 },
                        new() { Number = "7_YT_ED_313", RequestTypeID = 2, RequestStatusID = 4, CreationDate = new(2024, 03, 27), CompanyID = 3, ExecutorID = 9 },
                        new() { Number = "8998_UK_424", RequestTypeID = 3, RequestStatusID = 4, CreationDate = new(2024, 04, 05), CompanyID = 2, ExecutorID = 10 },
                        new() { Number = "8_UZ_KO_2", RequestTypeID = 3, RequestStatusID = 3, CreationDate = new(2024, 08, 20), CompanyID = 1, ExecutorID = 3 },
                        new() { Number = "432_K_9", RequestTypeID = 1, RequestStatusID = 2, CreationDate = new(2024, 09, 01), CompanyID = 3, ExecutorID = 4 },
                        new() { Number = "3_PY_JO_01", RequestTypeID = 1, RequestStatusID = 5, CreationDate = new(2021, 10, 19), CompanyID = 1, ExecutorID = 8 }
                    ];

                    ApplicationContext.Requests.AddRange(Requests);

                    ApplicationContext.SaveChanges();
                }
            }

            catch
            {
                // Пересоздание БД в случае ошибок //

                ApplicationContext.Database.EnsureDeleted();

                ApplicationContext.Database.Migrate();
                Seed(ApplicationContext);
            }
        }
    }
}