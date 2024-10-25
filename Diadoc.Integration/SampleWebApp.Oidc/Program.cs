using Diadoc.Api;
using Diadoc.Api.Cryptography;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
        {
            options.Authority = "https://identity.kontur.ru";
            options.ClientId = builder.Configuration["Oidc:ClientId"];
            options.ClientSecret = builder.Configuration["Oidc:ClientSecret"];
            options.Scope.Add("Diadoc.PublicAPI.Staging");

            //options.SignedOutRedirectUri = "/Home/Goodbye";

            // "code id_token" indicates Hybrid flow
            options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            options.SaveTokens = true;

            // see https://github.com/aspnet/Security/issues/1376
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
        }
    );
var diadocApi = new DiadocApi(builder.Configuration["Oidc:ClientId"], "https://diadoc-api-test.kontur.ru", new WinApiCrypt());
diadocApi.UseOidc();
builder.Services.AddSingleton<IDiadocApi>(diadocApi);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();