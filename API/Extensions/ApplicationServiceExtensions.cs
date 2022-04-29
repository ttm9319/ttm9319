using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.SignalR;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
      public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
      {
          services.AddSingleton<PresenceTracker>();
           services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
           services.AddScoped<ITokenService, TokenService>();
           services.AddScoped<IphotoService, PhotoService>();
           services.AddScoped<ILikesRepository, LikesRepository>();
           services.AddScoped<IMessageRepository, MessageRepository>();
           services.AddScoped<LogUserActivity>();
           services.AddScoped<IUserRepository, UserRepository>();
           services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(Options =>
            {
              Options.UseSqlite(config.GetConnectionString("Defaultconnection"));
            });
            return services;
      } 
    }
}