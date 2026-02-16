
using Microsoft.Extensions.DependencyInjection;
using GreenLoop.BLL.Interfaces;
using GreenLoop.BLL.Services;

namespace GreenLoop.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLL(this IServiceCollection service)
        {
            service.AddScoped<IDriverService, Services.DriverService>();
            return service;
        }
    }
}
