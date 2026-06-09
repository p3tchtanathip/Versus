using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Versus.API.Context;
using Versus.API.Data;
using Versus.API.DTOs.Responses;
using Versus.API.Integrations.Search;
using Versus.API.Integrations.Search.LastFm;
using Versus.API.Integrations.Search.SportsDb;
using Versus.API.Integrations.Search.Tmdb;
using Versus.API.Middleware;
using Versus.API.Repositories;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services;
using Versus.API.Services.Interfaces;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("SessionId", new OpenApiSecurityScheme
        {
            Name = "X-Session-Id",
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Description = "Session ID"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "SessionId"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Versus API", Version = "v1" });
        });
    }

    builder.Services.AddDbContext<AppDbContext>(o =>
        o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddHttpContextAccessor();
    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var firstError = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault()
                    ?? "Validation failed";

                return new BadRequestObjectResult(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = firstError
                    });
            };
        });

    builder.Services.AddScoped<ICurrentSession, CurrentSession>();
    builder.Services.AddScoped<ISessionRepository, SessionRepository>();
    builder.Services.AddScoped<ISessionService, SessionService>();
    builder.Services.AddScoped<ITierListRepository, TierListRepository>();
    builder.Services.AddScoped<ITierListService, TierListService>();
    builder.Services.AddScoped<IItemRepository, ItemRepository>();
    builder.Services.AddScoped<IItemService, ItemService>();
    builder.Services.AddScoped<IMatchRepository, MatchRepository>();
    builder.Services.AddScoped<IMatchService, MatchService>();
    builder.Services.AddSingleton<IEloService, EloService>();
    builder.Services.AddSingleton<IPairingService, PairingService>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<ISearchService, SearchService>();
    builder.Services.AddHttpClient<ISearchProvider, TmdbSearchProvider>();
    builder.Services.AddHttpClient<ISearchProvider, LastFmSearchProvider>();
    builder.Services.AddHttpClient<ISearchProvider, SportsDbSearchProvider>();

    var app = builder.Build();

    app.Use((context, next) =>
    {
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        return next();
    });

    if (builder.Configuration.GetValue("Database:RunMigrationsOnStartup", false))
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "Database migration failed on startup");
        }
    }

    app.UseMiddleware<SessionMiddleware>();
    app.UseMiddleware<ExceptionMiddleware>();
    app.MapControllers();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
