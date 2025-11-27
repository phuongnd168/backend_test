using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.BHL.Infra.Business;
using TLS.BHL.Infra.Business.Services;
using TLS.BHL.Infra.Data;
using TLS.BHL.Infra.Data.SQL.Contexts;
using TLS.BHL.Infra.Data.SQL.Repositories;
using TLS.BHL.Web.AdminApi;
using TLS.BHL.Web.AdminHandlers;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Category;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Order;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;
using TLS.Core;
using TLS.Core.Service;
using TLS.Core.Web.Validation;
using TLS.Lib.Exceptional;
using VNPAY.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("exceptional.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"exceptional.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.ConfigureAppCore();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductServie>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(GetListProductHandler).Assembly,
        typeof(GetListCategoryHandler).Assembly,
        typeof(UpdateProductCountHandler).Assembly,
         typeof(CreateOrderProductHandler).Assembly
    )
);
var vnpayConfig = builder.Configuration.GetSection("VNPAY");

builder.Services.AddVnpayClient(config =>
{
    config.TmnCode = vnpayConfig["TmnCode"]!;
    config.HashSecret = vnpayConfig["HashSecret"]!;
    config.CallbackUrl = vnpayConfig["CallbackUrl"]!;
 
});


// Add services to the container.
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Content-Disposition"); // content-disposition is *exposed* (and allowed because of AllowAnyHeader)
}));
builder.Services.AddHealthChecks();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    var contractResolver = new DefaultContractResolver();
    // Configure Newtonsoft.Json options here
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
    options.SerializerSettings.ContractResolver = contractResolver; // For api

    // For other json convert
    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
    {
        ContractResolver = contractResolver
    };
});
builder.Services.AddDbContext<BHLSqlDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    // Only loopback proxies are allowed by default.
    // Clear that restriction because forwarders are enabled by explicit 
    // configuration.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<WebAdminExceptionsHandler>();
builder.Services.AddLibExceptional();
//builder.Services.AddLibRedisCache(true);
builder.Services.AddInfraRepositories();
builder.Services.AddInfraServices();
builder.Services.AddWebAdminHandlers();

var app = builder.Build();
app.UseExceptionHandler(o => { });

// If development mode
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}

// Forwarded all headers
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
//app.MapFallbackToFile("index.html");

app.Run();
