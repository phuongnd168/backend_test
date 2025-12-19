using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// Clear mặc định
builder.Logging.ClearProviders();

// Log ra console
builder.Logging.AddConsole();
// 1. Add YARP
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
//2. Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();
// 2. Add Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        // Limit theo IP
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,        
                Window = TimeSpan.FromSeconds(60),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value;

        // public route
        if (path.StartsWith("/api/auth"))
        {
            await next();
            return;
        }

        // private route
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.StatusCode = 401;
            return;
        }

        await next();
    });
});
app.Use(async (context, next) =>
{
    var logger = context.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("GatewayLogger");

    logger.LogInformation(
        "Incoming request: {method} {path} from {ip}",
        context.Request.Method,
        context.Request.Path,
        context.Connection.RemoteIpAddress
    );

    await next();

    logger.LogInformation(
        "Response status: {statusCode}",
        context.Response.StatusCode
    );
});
app.Run();
