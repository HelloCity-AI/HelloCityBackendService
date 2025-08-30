using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using HelloCity.Api.FluentValidations;
using HelloCity.Api.Middlewares.GlobalException;
using HelloCity.Api.Profiles;
using HelloCity.IRepository;
using HelloCity.IServices;
using HelloCity.Models;
using HelloCity.Repository;
using HelloCity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using HelloCity.Api.HealthChecks;
using HelloCity.Services.Options;

namespace HelloCity.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        //Load environment-specific config
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                reloadOnChange: true)
            .AddEnvironmentVariables();

        // Configure Serilog
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        // Add services to the container.
        // Add FluentValidation

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton(new ImageFileValidator());
        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<EditUserDtoValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UploadImageRequestValidator>();
        builder.Services.AddFluentValidationAutoValidation();
        

        //Binding S3ClientOption from appsettings and register ImageStorageService
        builder.Services.Configure<S3ClientOptions>(builder.Configuration.GetSection("AwsS3"));
        builder.Services.AddSingleton<IImageStorageService, ImageStorageService>();

        builder.Services.Configure<ApiConfigs>(builder.Configuration.GetSection("ApiConfigs"));
        // Only for test purpose, can be deleted when we start development
        builder.Services.AddScoped<ITestUserService, TestUserService>();
        builder.Services.AddScoped<ITestUserService, TestUserService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "HelloCity.Api", Version = "v1" });

            // add JWT Bearer support
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "input form Bearer {your JWT token}"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });


        //Register Repository and Services
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();
        builder.Services.AddScoped<IChecklistItemService, ChecklistItemService>();

        // Add AppDbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("HelloCity.Models")));
        //Add AutoMapper 

        builder.Services.AddAutoMapper(
            cfg => { },
            typeof(UserProfile).Assembly
        );

        // JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://hello-city.au.auth0.com"; // auth0 domain
                options.Audience = "https://hellocity.api";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });


        //cors policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // health check
        builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseCors("AllowReactApp");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = HealthCheckResponseWriter.WriteResponse
        });
        app.Run();
    }
}