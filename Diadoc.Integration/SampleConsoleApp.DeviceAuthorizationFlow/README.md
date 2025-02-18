# Консольное приложение с OIDC-аутентификацией по Device Flow

Это простейшее консольное приложение, предназначенное для локального запуска.

Демонстрируемые возможности:

* Аутентификация через [Контур.ID](https://developer.kontur.ru/Docs/html/index.html#id) по протоколу OpenID Connect по [Device Flow](https://developer.kontur.ru/Docs/html/schemes/device_flow.html).
* Интеграция с OpenID Connect с помощью пакета [Duende.IdentityModel](https://www.nuget.org/packages/Duende.IdentityModel).
* Использование полученного access_token для запросов в АПИ Диадока.

Инструкция по запуску:

* Создайте приложение в Кабинете интегратора. Получите client_id и client_secret.
* Укажите client_id и client_secret в настройках:

```
dotnet user-secrets set "Oidc:ClientId" "MyClientId"
dotnet user-secrets set "Oidc:ClientSecret" "MyClientSecret"
```

* Запустите приложение: `dotnet run`. Приложение получит код устройства и откроет браузер для аутентификации пользователя.
* В случае успешной аутентификации приложение получит access_token, сделает с ним запрос в метод [GetMyOrganizations](https://developer.kontur.ru/docs/diadoc-api/http/GetMyOrganizations.html) и выведет результат на консоль.
