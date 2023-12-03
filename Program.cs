using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop_Mvc.Data;
using Pomelo.EntityFrameworkCore.MySql;
using Shop_Mvc.Controllers;
using Shop_Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MyDbContext"),
        new MySqlServerVersion(new Version(8, 0, 23)) 
    )
);
builder.Services.AddScoped<IDatabaseServise, DatabaseServise>();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Categories}/{action=ProductsView}/{id?}");
//});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Categories}/{action=SubcategoriesView}/{id?}");
//});

app.Run();