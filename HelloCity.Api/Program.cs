using HelloCity.Api.Middlewares.GlobalException;
using HelloCity.IRepository;
using HelloCity.IServices;
using HelloCity.Models;
using HelloCity.Models.Profiles;
using HelloCity.Repository;
using HelloCity.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;


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
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Configure Serilog
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        // Add services to the container.

        builder.Services.AddControllers();



        builder.Services.Configure<ApiConfigs>(builder.Configuration.GetSection("ApiConfigs"));
        // Only for test purpose, can be deleted when we start development
        builder.Services.AddScoped<ITestUserService, TestUserService>();


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Register Repository and Services
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        // Add AppDbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("HelloCity.Api")));
        //Add AutoMapper 

        builder.Services.AddAutoMapper(
            cfg => { },
            typeof(UserProfile).Assembly
        );
        
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseCors("AllowReactApp");

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}