# Express.js приложение с OIDC-аутентификацией на базе express-openid-connect

Это обычное шаблонное Express.js приложение. Предназначено для локального запуска.

Демонстрируемые возможности:

* Аутентификация через сервис [OpenID Connect](https://developer.kontur.ru/Docs/html/index.html#id).
* Интеграция OpenID Connect с Express.js с помощью пакета [express-openid-connect](https://www.npmjs.com/package/express-openid-connect).
* Использование полученного access_token для запросов в АПИ Диадока.

Инструкция по запуску:

* Создайте приложение в Кабинете интегратора. Получите client_id и client_secret.
* Скопируйте файл .example.oidc.dev в файл .oidc.dev. В файле .oidc.dev заполните параметры `CLIENT_ID` и `CLIENT_SECRET`.
* Выполните `npm i` из этого каталога.
* Выполните `npm start`. Приложение должно запуститься с сообщением `listening at http://localhost:5000/`.
* Перейдите по адресу http://localhost:5000. В случае успешной аутентификации приложение получит access_token, сделает с ним запрос в метод [GetMyOrganizations](https://developer.kontur.ru/docs/diadoc-api/http/GetMyOrganizations.html) в Диадок API и отобразит его ответ.
