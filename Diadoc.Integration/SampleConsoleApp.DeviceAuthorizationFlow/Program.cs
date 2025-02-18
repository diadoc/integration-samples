using System.Diagnostics;
using System.Runtime.InteropServices;
using Diadoc.Api;
using Diadoc.Api.Cryptography;
using Duende.IdentityModel.Client;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var clientId = configuration["Oidc:ClientId"]!;
var clientSecret = configuration["Oidc:ClientSecret"];

var httpClient = new HttpClient();

var deviceAuthorizationResponse = await httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
{
    Address = "https://identity.kontur.ru/connect/deviceauthorization",
    Scope = "openid profile email Diadoc.PublicAPI.Staging",
    ClientId = clientId,
    ClientSecret = clientSecret
});

if (deviceAuthorizationResponse.IsError)
{
    throw new Exception(deviceAuthorizationResponse.Error);
}

Console.WriteLine($"Url for user authorization: {deviceAuthorizationResponse.VerificationUriComplete}");

OpenBrowser(deviceAuthorizationResponse.VerificationUriComplete!);

var accessToken = await PollAccessToken();

var diadoc = new DiadocApi(clientId, "https://diadoc-api-staging.kontur.ru", new WinApiCrypt());

diadoc.UseOidc();

var organizations = diadoc.GetMyOrganizations(accessToken);

Console.WriteLine($"Successfully got {organizations.Organizations.Count} organizations");

static void OpenBrowser(string url)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        Process.Start("xdg-open", url);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        Process.Start("open", url);
    }
}

async Task<string> PollAccessToken()
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(deviceAuthorizationResponse.ExpiresIn!.Value));

    Console.WriteLine($"Device code expires in: {deviceAuthorizationResponse.ExpiresIn} seconds");

    while (!cts.IsCancellationRequested)
    {
        var tokenResponse = await httpClient.RequestDeviceTokenAsync(new DeviceTokenRequest
        {
            Address = "https://identity.kontur.ru/connect/token",
            DeviceCode = deviceAuthorizationResponse.DeviceCode!,
            ClientId = clientId,
            ClientSecret = clientSecret
        });

        if (IsUserAuthorizationInProgress(tokenResponse))
        {
            await Task.Delay(TimeSpan.FromSeconds(deviceAuthorizationResponse.Interval));
        }
        else
        {
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            Console.WriteLine($"Access token expires in: {tokenResponse.ExpiresIn} seconds");

            return tokenResponse.AccessToken!;
        }
    }

    throw new Exception("Device code expired");

    bool IsUserAuthorizationInProgress(ProtocolResponse tokenResponse)
    {
        return tokenResponse.Error is "authorization_pending" or "slow_down";
    }
}