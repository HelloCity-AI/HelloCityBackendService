using AutoMapper;
using HelloCity.IRepository;
using HelloCity.IServices;
using HelloCity.Models;
using HelloCity.Models.DTOs.Users;
using HelloCity.Models.Entities;
using HelloCity.Repository;
using HelloCity.Services;
using Microsoft.EntityFrameworkCore;

namespace HelloCity.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        
        

        builder.Services.Configure<ApiConfigs>(builder.Configuration.GetSection("ApiConfigs"));
        // Only for test purpose, can be deleted when we start development
        builder.Services.AddScoped<ITestUserService,TestUserService>();
        
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add AppDbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        //Add AutoMapper 
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<Users, UserDto>();
        });

        var app = builder.Build();

        //Register Repository and Services
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}