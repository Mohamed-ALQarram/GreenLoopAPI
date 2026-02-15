
using Microsoft.Extensions.DependencyInjection;

namespace GreenLoop.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLL(this IServiceCollection service)
        {
            return service;
        }
    }
}
