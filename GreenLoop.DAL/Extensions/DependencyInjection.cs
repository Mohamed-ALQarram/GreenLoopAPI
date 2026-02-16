using GreenLoop.DAL.Interfaces;
using GreenLoop.DAL.Data;
using GreenLoop.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenLoop.DAL.Extensions;

public static class DependencyInjection 
{
    public static IServiceCollection AddDAL(this IServiceCollection service, IConfiguration configuration)
    {
        
        service.AddDbContext<GreenLoopDbContext>((options) => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });

        // Repositories
        service.AddScoped<IAuthRepository, AuthRepository>();
        service.AddScoped<IRequestRepository, RequestRepository>();

        return service;
    }
}
