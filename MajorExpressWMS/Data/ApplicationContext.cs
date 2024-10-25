﻿using System.IO;
using System.Windows;

using Microsoft.EntityFrameworkCore;

using MajorExpressWMS.Models;

namespace MajorExpressWMS.Data
{
    /// <summary>
    /// Класс контекста БД
    /// </summary>
    internal class ApplicationContext : DbContext
    {
        /// <summary>
        /// Путь к файлу со строкой подключения к БД
        /// </summary>
        private static string? PathToFileOfDB;

        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private readonly string? DBConnection;

        // Пользователи //

        /// <summary>
        /// Таблица "Роли пользователей"
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Таблица "Пользователи"
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица создателей заявок
        /// </summary>
        public DbSet<RequestCreator> RequestCreators { get; set; }

        /// <summary>
        /// Таблица "Исполнители" (курьеры)
        /// </summary>
        public DbSet<RequestExecutor> RequestExecutors { get; set; }

        /// <summary>
        /// Таблица "Архивщики" (инициаторы архивации заявки)
        /// </summary>
        public DbSet<RequestArchiver> RequestArchivers { get; set; }

        // Заявки //

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
        /// Пустой конструктор класса контекста БД (для создания миграций)
        /// </summary>
        public ApplicationContext()
        {
            string PathToExe = AppDomain.CurrentDomain.BaseDirectory;
            PathToFileOfDB = Path.Join(PathToExe, "MajorExpressDB_WMS/MajorExpressDB_WMS.txt");

            if (File.Exists(PathToFileOfDB))
            {
                DBConnection = File.ReadAllLines(PathToFileOfDB)[0];

                int RetryCount = 0;
                InitializeDatabase(this, ref RetryCount);
            }

            else
            {
                MessageBox.Show($"Похоже, что файл {PathToFileOfDB.Split("/")[^1]} со строкой подключения отсутствует по полному пути:\n\n{PathToFileOfDB}\n\nНе удалось создать/подключиться к базе данных", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Конструктор класса контекста БД
        /// </summary>
        /// <param name="IsDatabaseCreated">out bool-переменная, указывающая, была ли создана БД</param>
        public ApplicationContext(out bool IsDatabaseCreated)
        {
            IsDatabaseCreated = false;

            string PathToExe = AppDomain.CurrentDomain.BaseDirectory;
            PathToFileOfDB = Path.Join(PathToExe, "MajorExpressDB_WMS/MajorExpressDB_WMS.txt");

            if (File.Exists(PathToFileOfDB))
            {
                DBConnection = File.ReadAllLines(PathToFileOfDB)[0];

                int RetryCount = 0;
                IsDatabaseCreated = InitializeDatabase(this, ref RetryCount);
            }

            else
            {
                MessageBox.Show($"Похоже, что файл {PathToFileOfDB.Split("/")[^1]} со строкой подключения отсутствует по полному пути:\n\n{PathToFileOfDB}\n\nНе удалось создать/подключиться к базе данных", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Защищённый метод подключения к БД через строку подключения
        /// </summary>
        /// <param name="Options">Параметры для <see cref="DbContextOptionsBuilder"/></param>
        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            Options.UseSqlServer(DBConnection);
        }

        /// <summary>
        /// Статический метод создания базы данных с таблицами
        /// </summary>
        /// <param name="ApplicationContext"></param>
        private static bool InitializeDatabase(ApplicationContext ApplicationContext, ref int RetryCount)
        {
            try
            {
                ApplicationContext.Database.Migrate();

                Seed(ApplicationContext);

                return true;
            }

            catch
            {
                // Пересоздание БД в случае ошибок //

                if (RetryCount < 1)
                {
                    try
                    {
                        ApplicationContext.Database.EnsureDeleted();
                    }

                    catch
                    {
                        MessageBox.Show($"Не удалось подключиться к базе данных!\n\nУбедитесь, что:\n\n1) В файле {PathToFileOfDB?.Split("/")[^1]} по полному пути:\n\n{PathToFileOfDB}\n\nуказана верная строка подключения.\n\n2) У вас есть доступ к базе данных.", "Ошибка подключения к базе данных", MessageBoxButton.OK, MessageBoxImage.Error);

                        return false;
                    }

                    RetryCount++;
                    InitializeDatabase(ApplicationContext, ref RetryCount);

                    return true;
                }

                else
                {
                    MessageBox.Show("Не удалось инициализировать/изменить базу данных после нескольких попыток.\n\nЭто может свидетельствовать об ошибках миграции.", "Ошибка взаимодействия с базой данных", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }

        /// <summary>
        /// Статический метод добавления данных по умолчанию в таблицы
        /// </summary>
        /// <param name="ApplicationContext"></param>
        private static void Seed(ApplicationContext ApplicationContext)
        {
            List<UserRole> UserRoles =
            [
                new() { Role = "Пользователь" },
                new() { Role = "Курьер" },
                new() { Role = "Отправитель" },
                new() { Role = "Получатель" },
                new() { Role = "Администратор" }
            ];

            // Заполнение таблицы "Роли пользователей"
            if (!ApplicationContext.UserRoles.Any())
            {
                ApplicationContext.UserRoles.AddRange(UserRoles);

                ApplicationContext.SaveChanges();
            }

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
                new() { Name = "Анастасия", Surname = "Ромина", Login = "RomiAN", Password = "56789", UserRoleID = 5 }
            ];

            // Заполнение таблицы "Пользователи"
            if (!ApplicationContext.Users.Any())
            {
                ApplicationContext.Users.AddRange(Users);

                ApplicationContext.SaveChanges();
            }

            List<RequestCreator> RequestCreators =
            [
                new() { UserID = 1 },
                new() { UserID = 2 },
                new() { UserID = 3 },
                new() { UserID = 4 },
                new() { UserID = 5 }
            ];

            // Заполнение таблицы создателей заявок
            if (!ApplicationContext.RequestCreators.Any())
            {
                ApplicationContext.RequestCreators.AddRange(RequestCreators);

                ApplicationContext.SaveChanges();
            }

            List<RequestExecutor> RequestExecutors =
            [
                new() { UserID = 6 },
                new() { UserID = 7 },
                new() { UserID = 8 },
                new() { UserID = 9 },
                new() { UserID = 10 }
            ];

            // Заполнение таблицы "Исполнители"
            if (!ApplicationContext.RequestExecutors.Any())
            {
                ApplicationContext.RequestExecutors.AddRange(RequestExecutors);

                ApplicationContext.SaveChanges();
            }

            List<RequestArchiver> RequestArchivers =
            [
                new() { UserID = 2 },
                new() { UserID = 4 },
                new() { UserID = 6 },
                new() { UserID = 8 },
                new() { UserID = 10 }
            ];

            // Заполнение таблицы "Архивщики"
            if (!ApplicationContext.RequestArchivers.Any())
            {
                ApplicationContext.RequestArchivers.AddRange(RequestArchivers);

                ApplicationContext.SaveChanges();
            }

            List<RequestType> RequestTypes =
            [
                new() { Type = "Отправка" },
                new() { Type = "Доставка" },
                new() { Type = "Получение" }
            ];

            // Заполнение таблицы "Типы заявок"
            if (!ApplicationContext.RequestTypes.Any())
            {
                ApplicationContext.RequestTypes.AddRange(RequestTypes);

                ApplicationContext.SaveChanges();
            }

            List<RequestStatus> RequestStatuses =
            [
                new() { Status = "Новая" },
                new() { Status = "Передана на выполнение" },
                new() { Status = "Выполнена" },
                new() { Status = "Отменена" },
                new() { Status = "Удалена" }
            ];

            // Заполнение таблицы "Статусы заявок"
            if (!ApplicationContext.RequestStatuses.Any())
            {
                ApplicationContext.RequestStatuses.AddRange(RequestStatuses);

                ApplicationContext.SaveChanges();
            }

            List<Company> Companies =
            [
                new() { Name = "Молот и наковальня" },
                new() { Name = "Химера Экспресс" },
                new() { Name = "Логистик Технолоджис" }
            ];

            // Заполнение таблицы "Компании"
            if (!ApplicationContext.Companies.Any())
            {
                ApplicationContext.Companies.AddRange(Companies);

                ApplicationContext.SaveChanges();
            }

            List<ArchiveRequest> ArchiveRequests =
            [
                new() { RequestNumber = "2341_AP_DD_899", RequestTypeID = 1, CreatorID = 1, CreationDate = new(2021, 01, 25), ArchiverID = 5, ArchiveDate = new(2021, 01, 26) },
                new() { RequestNumber = "412_LP_901", RequestTypeID = 2, CreatorID = 5, CreationDate = new(2022, 05, 14), ArchiverID = 1, ArchiveDate = new(2022, 05, 20), Description = "Отменена по причине: \"Груз повреждён при транспортировке\"" },
                new() { RequestNumber = "117892_SA_LU_9", RequestTypeID = 3, CreatorID = 4, CreationDate = new(2023, 08, 01), ArchiverID = 3, ArchiveDate = new(2023, 08, 05) },
                new() { RequestNumber = "15789_F_9", RequestTypeID = 3, CreatorID = 3, CreationDate = new(2023, 10, 05), ArchiverID = 2, ArchiveDate = new(2023, 10, 25) },
                new() { RequestNumber = "87_LO_PO_83299", RequestTypeID = 1, CreatorID = 2, CreationDate = new(2024, 02, 11), ArchiverID = 4, ArchiveDate = new(2024, 02, 17), Description = "Отменена по причине: \"Компания-контрагент рассторгнула договор\"" }
            ];

            // Заполнение таблицы "Архивные заявки"
            if (!ApplicationContext.ArchiveRequests.Any())
            {
                ApplicationContext.ArchiveRequests.AddRange(ArchiveRequests);

                ApplicationContext.SaveChanges();
            }

            List<Request> Requests =
            [
                new() { Number = "123_RU_313", RequestTypeID = 1, RequestStatusID = 1, CreatorID = 1, CreationDate = new(2024, 01, 17), ExecutorID = 1, CompanyID = 1 },
                new() { Number = "6313_RW_1583", RequestTypeID = 2, RequestStatusID = 3, CreatorID = 2, CreationDate = new(2024, 01, 29), ExecutorID = 2, CompanyID = 3 },
                new() { Number = "72_RL_OY_23", RequestTypeID = 3, RequestStatusID = 2, CreatorID = 3, CreationDate = new(2024, 02, 15), ExecutorID = 5, CompanyID = 2 },
                new() { Number = "6709134_UZ_59723", RequestTypeID = 2, RequestStatusID = 1, CreatorID = 4, CreationDate = new(2024, 02, 11), ExecutorID = 4, CompanyID = 2 },
                new() { Number = "735304_KE_9", RequestTypeID = 2, RequestStatusID = 5, CreatorID = 5, CreationDate = new(2024, 03, 24), ExecutorID = 3, CompanyID = 3 },
                new() { Number = "7_YT_ED_313", RequestTypeID = 2, RequestStatusID = 4, CreatorID = 5, CreationDate = new(2024, 03, 27), ExecutorID = 2, CompanyID = 3 },
                new() { Number = "8998_UK_424", RequestTypeID = 3, RequestStatusID = 4, CreatorID = 4, CreationDate = new(2024, 04, 05), ExecutorID = 4, CompanyID = 2 },
                new() { Number = "8_UZ_KO_2", RequestTypeID = 3, RequestStatusID = 3, CreatorID = 3, CreationDate = new(2024, 08, 20), ExecutorID = 3, CompanyID = 1 },
                new() { Number = "432_K_9", RequestTypeID = 1, RequestStatusID = 2, CreatorID = 2, CreationDate = new(2024, 09, 01), ExecutorID = 1, CompanyID = 3 },
                new() { Number = "3_PY_JO_01", RequestTypeID = 1, RequestStatusID = 5, CreatorID = 1, CreationDate = new(2021, 10, 19), ExecutorID = 5, CompanyID = 1 }
            ];

            // Заполнение таблицы "Заявки"
            if (!ApplicationContext.Requests.Any())
            {
                ApplicationContext.Requests.AddRange(Requests);

                ApplicationContext.SaveChanges();
            }

            List<(bool IsDataInTableExpected, Action IfDataInTableUnexpectedAction)> TableDataChecks =
            [
                (IsDataInTableExpected(ApplicationContext, UserRoles, UserRole => UserRole.Role), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, Users, User => new { User.Name, User.Surname, User.Patronymic, User.Login, User.Password, User.UserRoleID }), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, RequestCreators, RequestCreator => RequestCreator.UserID), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, RequestExecutors, RequestExecutor => RequestExecutor.UserID), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, RequestArchivers, RequestArchiver => RequestArchiver.UserID), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, RequestTypes, RequestType => RequestType.Type), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, RequestStatuses, RequestStatus => RequestStatus.Status), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, Companies, Company => Company.Name), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, ArchiveRequests, ArchiveRequest => new { ArchiveRequest.RequestNumber, ArchiveRequest.RequestTypeID, ArchiveRequest.CreatorID, ArchiveRequest.CreationDate, ArchiveRequest.ArchiverID, ArchiveRequest.ArchiveDate, ArchiveRequest.Description }), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); }),
                (IsDataInTableExpected(ApplicationContext, Requests, Request => new { Request.Number, Request.RequestTypeID, Request.RequestStatusID, Request.CreatorID, Request.CreationDate, Request.ExecutorID, Request.CompanyID }), () => { ClearTables(ApplicationContext); throw new Exception("Данные не соответствуют ожидаемым."); })
            ];

            // Сверка изначальных значений с ожидаемыми (пересоздание БД в случае различий)

            foreach ((bool Condition, Action Action) in TableDataChecks)
            {
                if (!Condition)
                {
                    Action();
                }
            }
        }

        /// <summary>
        /// Статический метод проверки соответствия данных таблицы ожидаемым
        /// </summary>
        /// <typeparam name="T">Сверяемая таблица</typeparam>
        /// <param name="ApplicationContext"></param>
        /// <param name="ExpectedData">Ожидаемые данные</param>
        /// <param name="Selector">Параметры сверки</param>
        /// <returns><see cref="bool"/> значение соответствия фактических и ожидаемым данных</returns>
        private static bool IsDataInTableExpected<T>(ApplicationContext ApplicationContext, List<T> ExpectedData, Func<T, object> Selector) where T : class
        {
            List<object> ActualData = ApplicationContext.Set<T>().Take(ExpectedData.Count).Select(Selector).ToList();

            List<object> _ExpectedData = ExpectedData.Select(Selector).ToList();

            bool IsEqual = true;

            for (int i = 0; i < _ExpectedData.Count && i < ActualData.Count; i++)
            {
                if (!_ExpectedData[i].Equals(ActualData[i]))
                {
                    IsEqual = false;

                    break;
                }
            }

            return IsEqual;
        }

        /// <summary>
        /// Статический метод удаления данных таблиц
        /// </summary>
        /// <param name="ApplicationContext"></param>
        private static void ClearTables(ApplicationContext ApplicationContext)
        {
            List<Action> TablesToClear =
            [
                () => ClearTable<UserRole>(ApplicationContext),
                () => ClearTable<User>(ApplicationContext),
                () => ClearTable<RequestCreator>(ApplicationContext),
                () => ClearTable<RequestExecutor>(ApplicationContext),
                () => ClearTable<RequestArchiver>(ApplicationContext),
                () => ClearTable<RequestType>(ApplicationContext),
                () => ClearTable<RequestStatus>(ApplicationContext),
                () => ClearTable<Company>(ApplicationContext),
                () => ClearTable<ArchiveRequest>(ApplicationContext),
                () => ClearTable<Request>(ApplicationContext)
            ];

            foreach (Action ClearTableAction in TablesToClear)
            {
                ClearTableAction();
            }

            ApplicationContext.SaveChanges();
        }

        /// <summary>
        /// Статический метод удаления данных таблицы
        /// </summary>
        /// <typeparam name="T">Очищаемая таблица</typeparam>
        /// <param name="ApplicationContext"></param>
        private static void ClearTable<T>(ApplicationContext ApplicationContext) where T : class
        {
            ApplicationContext.Set<T>().RemoveRange(ApplicationContext.Set<T>());
        }
    }
}