# LibraryWebApp

## Описание

Маленький пет-проект на тему электронной библиотеки.

## Требования

- [Node.js](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [MS SQL](https://www.microsoft.com/en-us/sql-server)
- [.NET SDK](https://dotnet.microsoft.com/en-us/)
- И Visual Studio 

## Установка и запуск

1. Склонируйте репозиторий
2. Установите зависимости
3. Настройте СУБД
4. Настройте пути подключения СУБД в микросервисах: AuthService, AuthorService, BookService
5. Выполните миграцию из AuthService
6. Запустите микросервисы с обязательным запуском ApiGatewayYARP (Например, через AppHost)
7. Запустите AngularProject используя ng serve
8. Наслождайтесь криво-косо сделанным фронтендом!
   
## Описания Angular

Всего есть следующие пути:
'register',
'login',
'my-books',
'books',
'books/:isbn',
'admin' - для Администраторов,
'' redirectTo: '/books'
и 2 модальных окна:
для создания книги,
для редактирования книги
