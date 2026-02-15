
using Microsoft.Extensions.DependencyInjection;

namespace GreenLoop.BLL
{
    public static class DependencyInjection
    {
        public static IServiceProvider AddBLL(this IServiceProvider service)
        {
            return service;
        }
    }
}
