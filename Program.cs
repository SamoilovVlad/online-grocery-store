using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop_Mvc.Data;
using Pomelo.EntityFrameworkCore.MySql;
using Shop_Mvc.Controllers;
using Shop_Mvc.Services;
using Microsoft.AspNetCore.CookiePolicy;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using Shop_Mvc.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

string encryptedConnectionString = builder.Configuration.GetConnectionString("MyDbContext");
byte[] key = new byte[] { 204, 19, 62, 166, 246, 42, 91, 86, 216, 49, 131, 212, 247, 61, 249, 19, 197, 198, 220, 211, 177, 130, 80, 243, 163, 48, 121, 128, 98, 20, 177, 57 };
string decryptedConnectionString = DecryptAES(encryptedConnectionString, key);


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        decryptedConnectionString,
        new MySqlServerVersion(new Version(8, 0, 23))
    )
);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IDatabaseServise, DatabaseServise>();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.None;
    options.Secure = CookieSecurePolicy.None;
});

static string DecryptAES(string ciphertextBase64, byte[] key)
{
    byte[] ciphertext = Convert.FromBase64String(ciphertextBase64);

    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = key;
        aesAlg.IV = new byte[16]; // Initialization Vector (IV)

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msDecrypt = new MemoryStream(ciphertext))
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}


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
//            pattern: "{controller=Home}/{action=UserProfileView}");
//});

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
