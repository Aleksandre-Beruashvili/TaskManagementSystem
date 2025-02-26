using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskManagementSystem.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Read the WebAPI base URL from configuration.
var webApiBaseUrl = builder.Configuration["WebApiBaseUrl"];

// Add MVC services.
builder.Services.AddControllersWithViews();

// Configure cookie authentication with custom expiration settings.
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to login if unauthorized.
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expires after 30 minutes.
        options.SlidingExpiration = true; // Renew cookie if user is active.
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    });

// Register a simple token service (to store/retrieve the JWT token).
builder.Services.AddSingleton<ITokenService, TokenService>();

// Register our DelegatingHandler to attach the JWT token to outgoing requests.
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Register typed HttpClient services to call WebAPI endpoints and add the JWT handler.
builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<IProjectApiService, ProjectApiService>(client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<ITaskApiService, TaskApiService>(client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<ILabelApiService, LabelApiService>(client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// Register the Profile API service so that the ProfileController can resolve IProfileApiService.
builder.Services.AddHttpClient<IProfileApiService, ProfileApiService>(client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
