using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using ICMServer.Configuration;
using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Interfaces;
using ICMServer.Managers;
using ICMServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Connection DB
// =====================
var connStrIIS = builder.Configuration["ConnectionStrings:iis"];
builder.Services.AddDbContextPool<ICMDBContext>(options => options.UseSqlServer(connStrIIS));

// =====================
// Services
// =====================
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
    .AddScoped<IRepository, Repository>()
    .AddMvc()
    .AddSessionStateTempDataProvider();

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ICMServer API",
        Version = "v1"
    });
});

builder.Services.AddHostedService<MinuteTickerService>();
builder.Services.AddSingleton<ProcessQueueService>();
builder.Services.AddScoped<AiAnalysisService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ProcessQueueService>());

// Add Memory Cache
builder.Services.AddMemoryCache();

// Register AdjustmentImportCache
builder.Services.AddSingleton<AdjustmentImportCache>();

// Register ProcessStateService (singleton to track ICM process state across requests)
builder.Services.AddSingleton<IProcessStateService, ProcessStateService>();

// Register calculation services
builder.Services.AddCalculationServices();

// Register OrderService
builder.Services.AddScoped<IOrderService, OrderService>();

// Register EmployeeService
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register ReportService
builder.Services.AddScoped<IReportService, ReportService>();

// =====================
// CORS
// =====================
var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/icm-.log",
        rollingInterval: RollingInterval.Day,
        flushToDiskInterval: TimeSpan.FromSeconds(1),
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

// =====================
// Build app
// =====================
var app = builder.Build();

// Middleware pour gérer les preflight OPTIONS (SignalR)
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        var origin = context.Request.Headers["Origin"].ToString();
        if (allowedOrigins.Contains(origin))
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With, X-SignalR-User-Agent");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.StatusCode = 200;
            return;
        }
    }
    await next();
});

app.UseCors("AllowSpecificOrigins");

app.UseRouting();

if (app.Environment.IsEnvironment("UAT") || app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ICMServer API V1");
});

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
