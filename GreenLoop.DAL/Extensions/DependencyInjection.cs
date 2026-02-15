using Microsoft.Extensions.DependencyInjection;

namespace GreenLoop.DAL.Extensions
{
    public static class DependencyInjection 
    {
        public static IServiceProvider AddDAL(this IServiceProvider service)
        {
            return service;
        }
    }
}
