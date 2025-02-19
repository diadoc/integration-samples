# ASP.NET Core приложение с OIDC-аутентификацией

Это обычное шаблонное ASP.NET Core приложение. Предназначено для локального запуска.

Демонстрируемые возможности:

* Аутентификация через сервис [OpenID Connect](https://developer.kontur.ru/Docs/html/index.html#id).
* Интеграция OpenID Connect с ASP.NET Core с помощью пакета [Microsoft.AspNetCore.Authentication.OpenIdConnect](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect).
* Использование полученного access_token для запросов в АПИ Диадока.

Инструкция по запуску:

* Создайте приложение в Кабинете интегратора. Получите client_id и client_secret.
* Укажите client_id и client_secret в настройках:

```
dotnet user-secrets set "Oidc:ClientId" "MyClientId"
dotnet user-secrets set "Oidc:ClientSecret" "MyClientSecret"
```

* Запустите приложение: `dotnet run`. В случае успеха в консоли должно появиться сообщение `Now listening on: http://localhost:5000`.
* Перейдите в приложение по ссылке http://localhost:5000. В случае успешной аутентификации приложение получит access_token, сделает с ним запрос в метод [GetMyOrganizations](https://developer.kontur.ru/docs/diadoc-api/http/GetMyOrganizations.html) и выведет результат на страничке.