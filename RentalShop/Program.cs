using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RentalShop.Application.Facades;
using RentalShop.Application.Factories;
using RentalShop.Application.Services;
using RentalShop.Infrastructure.Persistence;
using RentalShop.Infrastructure.Repositories;
using RentalShop.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RentalShopSettings>(
    builder.Configuration.GetSection(RentalShopSettings.Section));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath         = "/Account/Login";
        options.LogoutPath        = "/Account/Logout";
        options.AccessDeniedPath  = "/Account/Login";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan    = TimeSpan.FromHours(8);
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<RentalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<EfCoreCatalogRepository>();
builder.Services.AddScoped<ICatalogRepository>(sp =>
    new CachingCatalogRepository(
        sp.GetRequiredService<EfCoreCatalogRepository>(),
        sp.GetRequiredService<IMemoryCache>(),
        sp.GetRequiredService<IOptions<RentalShopSettings>>(),
        sp.GetRequiredService<ILogger<CachingCatalogRepository>>()));

builder.Services.AddScoped<ToolFactory>();
builder.Services.AddScoped<GearFactory>();

builder.Services.AddScoped<IPricingStrategy, StandardPricingStrategy>();
builder.Services.AddScoped<PricingCalculator>();

builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<BillingService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ContractDocument>();
builder.Services.AddScoped<ReceiptDocument>();
builder.Services.AddScoped<DocumentService>(sp =>
    new DocumentService(
        sp.GetRequiredService<ReceiptDocument>(),
        sp.GetRequiredService<ILogger<DocumentService>>()));

builder.Services.AddScoped<RentalShopFacade>(sp => new RentalShopFacade(
    sp.GetRequiredService<InventoryService>(),
    sp.GetRequiredService<BillingService>(),
    sp.GetRequiredService<PaymentService>(),
    sp.GetRequiredService<DocumentService>(),
    new DocumentRenderers(
        sp.GetRequiredService<ContractDocument>(),
        sp.GetRequiredService<ReceiptDocument>()),
    sp.GetRequiredService<ToolFactory>(),
    sp.GetRequiredService<GearFactory>(),
    sp.GetRequiredService<ILogger<RentalShopFacade>>()));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Login");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "catalog",
    pattern: "catalog/{action=Index}/{id?}",
    defaults: new { controller = "Item" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
