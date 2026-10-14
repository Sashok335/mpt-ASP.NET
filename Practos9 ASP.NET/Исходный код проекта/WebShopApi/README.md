# WebShop API

ASP.NET Core Web API для всех восьми таблиц интернет-магазина. Контроллеры используют
сущности и `WebShopDbContext` из предыдущих практических работ `mpt-ASP.NET`.
API подключается к `WebShopDB` через Entity Framework Core. Скрипты базы
выполняются отдельно.

## Запуск в Windows через Parallels

1. Откройте `WebShopApi.sln` в Visual Studio с компонентами ASP.NET и .NET 10.
   На настроенной виртуальной машине копия решения находится по адресу
   `%LOCALAPPDATA%\WebShopApi\WebShopApi.sln`.
2. Выберите профиль `WebShopApi` около кнопки запуска. Нажмите **Ctrl+F5**.
3. Swagger открывается по адресу `http://localhost:5186/swagger` внутри Windows.

По умолчанию подключение в `appsettings.json` использует Windows-аутентификацию
и экземпляр `(localdb)\MSSQLLocalDB`. На Windows ARM64 приложение находит
текущий именованный канал через установленную утилиту `sqllocaldb.exe`.
Для другого существующего экземпляра
можно задать строку подключения через переменную `ConnectionStrings__WebShop`.

Если на другом ПК старая база отсутствует, выполните из папки проекта
в PowerShell. Скрипт структуры выполняется один раз.

```powershell
sqllocaldb start MSSQLLocalDB
sqlcmd -S '(localdb)\MSSQLLocalDB' -E -b -i .\Database\WebShopDB.sql
sqlcmd -S '(localdb)\MSSQLLocalDB' -E -b -i .\Database\DemoData.sql
```

`WebShopDB.sql` взят из второй практической. `DemoData.sql` добавляет примеры
для восьми таблиц. На настроенной Windows оба скрипта уже выполнены.

## Ресурсы

| Таблица | Маршрут |
| --- | --- |
| Roles | `/api/roles` |
| Users | `/api/users` |
| Categories | `/api/categories` |
| Products | `/api/products` |
| Carts | `/api/carts` |
| CartItems | `/api/cartitems` |
| Orders | `/api/orders` |
| OrderItems | `/api/orderitems` |

Для каждого ресурса доступны одинаковые операции.

| Метод | Маршрут относительно ресурса | Успешный ответ |
| --- | --- | --- |
| GET | `?page=1&pageSize=10` | 200, список по возрастанию ID |
| GET | `/{id}` | 200, одна запись |
| POST | базовый адрес | 201, запись и заголовок Location |
| PUT | `/{id}` | 204, заменены все редактируемые поля |
| PATCH | `/{id}` | 204, применены значения, отличные от null |
| DELETE | `/{id}` | 204, запись удалена |

Идентификаторы выдаёт SQL Server. При создании заказа сервер записывает время
UTC в существующий столбец `OrderDate` и возвращает его как `createdAt`.
Остальные таблицы не содержат столбца даты создания.

POST и PUT пользователя принимают `login`, `password` и `roleId`. PATCH
позволяет менять их по отдельности. Пароль хешируется в формате старого MVC-проекта.
В ответе API есть только `id`, `login` и `roleId`.

Отсутствующая запись или связанный объект дают 404. Ошибки валидации дают 400.
При нарушении уникальности или невозможности удаления из-за внешнего ключа
возвращается 409. Для удаления в Swagger сначала создайте отдельную запись через POST.
Каскадные удаления пользователей, корзин и заказов определены исходной схемой.
