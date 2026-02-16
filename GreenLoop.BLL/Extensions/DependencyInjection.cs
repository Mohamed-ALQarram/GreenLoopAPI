
using GreenLoop.BLL.Interfaces;
using GreenLoop.BLL.Interfaces.IServices;
using GreenLoop.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenLoop.BLL.Interfaces;
using GreenLoop.BLL.Services;

namespace GreenLoop.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLL(this IServiceCollection service)
        {
            // Services
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IRequestService, RequestService>();

            service.AddScoped<IDriverService, Services.DriverService>();
            service.AddScoped<IWalletService, Services.WalletService>();
            return service;
        }
    }
}

