using PaySpace.Calculator.Web.Services;
using PaySpace.Calculator.Web.Services.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCalculatorHttpServices();
builder.Services.AddHttpClient();
builder.Services.Configure<CalculatorSettings>
    (builder.Configuration.GetSection("CalculatorSettings"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Calculator}/{action=Index}/{id?}");
//app.MapControllerRoute(name: "default", pattern: "{controller=Calculator}/{action=History}");

app.Run();