# PhBook

Коротко: небольшой ASP.NET Core Web API для работы с телефонной книгой.

## Что реализовано

* ASP.NET Core Web API
* REST endpoints (работа с контактами и справочниками)
* PostgreSQL для хранения данных
* Разделение на Controller / Service / Repository
* Dependency injection
* Swagger/OpenAPI
* SQL-скрипты для инициализации схемы БД

## Architecture

Текущий поток выполнения запросов:
`Controller → Service → Repository → PostgreSQL`

Каждый слой выполняет свою задачу: Controller принимает HTTP запросы, Service реализует бизнес-логику, Repository инкапсулирует работу с базой данных.

## Tech stack

* C#
* ASP.NET Core 8.0
* PostgreSQL
* Swagger/OpenAPI

## Project structure

* `Controlers/` - API контроллеры
* `Services/` - Бизнес-логика
* `Repository/` - Работа с базой данных
* `Contracts/` - Модели запросов и ответов (DTO)
* `BDSqlScripts/` - SQL-скрипты создания схемы БД

## Running locally

1. Установите .NET 8.0 SDK (или новее).
2. Поднимите локальный сервер PostgreSQL.
3. Выполните SQL-скрипт `BDSqlScripts/create.sql` в вашей БД.
4. Задайте connection string безопасным способом. Например, укажите свой логин и пароль в `appsettings.json` или используйте `dotnet user-secrets`.
5. Выполните `dotnet restore` для восстановления зависимостей.
6. Выполните `dotnet run` для запуска проекта.
7. Откройте Swagger в браузере (URL можно найти в консоли после старта, обычно `/swagger`).

## API

| METHOD | PATH | DESCRIPTION |
|--------|------|-------------|
| POST | `/api/PhoneBook/AddOrUpdateToTable` | Добавить или обновить запись в справочнике |
| POST | `/api/PhoneBook/DeleteFromTable` | Удалить запись из справочника |
| POST | `/api/PhoneBook/GetLikeFromTable` | Поиск по справочнику |
| POST | `/api/PhoneBook/AddOrUpdateContact` | Добавить или обновить контакт |
| GET | `/api/PhoneBook/DeleteContact` | Удалить контакт по ID |
| POST | `/api/PhoneBook/GetContacts` | Получить список контактов |

## Notes

Это небольшой учебный / portfolio backend проект.
