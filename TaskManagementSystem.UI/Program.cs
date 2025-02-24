using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskManagementSystem.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Read the WebAPI base URL from configuration.
var webApiBaseUrl = builder.Configuration["WebApiBaseUrl"];

builder.Services.AddControllersWithViews();

// Register a simple token service.
builder.Services.AddSingleton<ITokenService, TokenService>();

// Register a DelegatingHandler to attach the JWT token automatically.
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Register typed HttpClient services for API consumption using the correct base URL.
builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
})
// Since you're using HTTP (not HTTPS), you don't need to bypass certificate validation.
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

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
