using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskManagementSystem.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Read the WebAPI base URL from configuration.
var webApiBaseUrl = builder.Configuration["WebApiBaseUrl"];

// Add MVC services.
builder.Services.AddControllersWithViews();

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
