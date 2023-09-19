using Database;
using Repository;
using Microsoft.EntityFrameworkCore;
using app.Services;
using app.Services.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ITradingLogicRepository, TradingLogicRepository>();
builder.Services.AddTransient<ITradingLogicService, TradingLogicService>();
builder.Services.AddTransient<ITradingLogicClientService, TradingLogicClientService>();
builder.Services.AddTransient<IHttpService, HttpService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddDbContext<TradingLogicDbContext>(options => options.UseSqlServer("Server=localhost;Database=TradingLogic;User=SA;Password=Passw0rd;TrustServerCertificate=True;"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
    app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
