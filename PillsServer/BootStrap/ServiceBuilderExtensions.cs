using FluentValidation;
using MediatR;
using PillsServer.Persistancy;
using PillsServer.Common;
using System.Reflection;

namespace PillsServer.BootStrap
{
    public static class ServiceBuilderExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepo<>), typeof(Repo<>));
        }
        public static void RegisterMediatr(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
