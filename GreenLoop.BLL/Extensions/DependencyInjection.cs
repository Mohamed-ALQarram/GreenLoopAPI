
using GreenLoop.BLL.Interfaces;
using GreenLoop.BLL.Interfaces.IServices;
using GreenLoop.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GreenLoop.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLL(this IServiceCollection service)
        {
            // Services
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IRequestService, RequestService>();

            return service;
        }
    }
}

