using GreenLoop.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenLoop.DAL.Extensions;

public static class DependencyInjection 
{
    public static IServiceCollection AddDAL(this IServiceCollection service, IConfiguration configuration)
    {
        
        service.AddDbContext<GreenLoopDbContext>((options) => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });

        service.AddScoped<GreenLoop.DAL.Interfaces.IRepositories.IDriverRepository, GreenLoop.DAL.Repositories.DriverRepository>();
        service.AddScoped<GreenLoop.DAL.Interfaces.IRepositories.IWalletRepository, GreenLoop.DAL.Repositories.WalletRepository>();

        return service;
    }
}
