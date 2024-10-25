const express = require('express');
const request = require('request-promise-native');
const { auth } = require('express-openid-connect');

const app = express();

app.use(
  auth({
    authorizationParams: {
      response_type: 'code id_token',
      audience: 'Diadoc.PublicAPI.Staging',
      scope: 'openid profile email Diadoc.PublicAPI.Staging',
      redirect_uri: 'http://localhost:5000/signin-oidc'
    },
    routes: {
        callback: '/signin-oidc'
    },
    clientAuthMethod: 'client_secret_post'
  })
);

app.get('/', async (req, res) => {
  let { token_type, access_token, isExpired, refresh } = req.oidc.accessToken;
  if (isExpired()) {
    ({ access_token } = await refresh());
  }
  const organizations = await request.get(`https://diadoc-api-staging.kontur.ru/GetMyOrganizations`, {
    headers: {
      Authorization: `Bearer ${access_token}`,
      Accept: 'application/json'
    },
    json: true,
  });
  res.header('Content-Type', 'application/json');
  res.send(JSON.stringify(organizations, null, 2));
});

app.listen(5000, () => console.log('listening at http://localhost:5000/'));