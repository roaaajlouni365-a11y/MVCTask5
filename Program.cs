using Microsoft.EntityFrameworkCore;
using CafeteriaManagement.Models;
using CafeteriaManagement.CodeFirst.Data;

var builder = WebApplication.CreateBuilder(args);

// Database First
builder.Services.AddDbContext<CafeteriaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Code First
builder.Services.AddDbContext<CafeteriaEmployeesContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EmployeesConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();