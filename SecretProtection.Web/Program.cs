using Microsoft.Data.SqlClient;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// secrets.json
string connectionString = builder.Configuration["ConnectionStrings:SqlConnectionstring"];

var dbConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

dbConnectionStringBuilder.Password = builder.Configuration["Passwords:SqlPassword"];

string conString = dbConnectionStringBuilder.ConnectionString;

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
