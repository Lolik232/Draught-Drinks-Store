using Core.BussinesLogic.Abstractions.Interfaces;
using Core.BussinesLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.BussinesLogic.Di
{
    public static class ServiceProviderExtensions
    {
        public static void AddBussinesLogicServices(this IServiceCollection services)
        {
            //services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();

        }
    }
}
