using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TLS.BHL.Web.AdminHandlers.AutoMapper;
using TLS.Core.Web.Validation;

namespace TLS.BHL.Web.AdminHandlers
{
    public static class WebAdminHandlersRegistration
    {
        public static IServiceCollection AddWebAdminHandlers(this IServiceCollection services)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(config =>
            {
                config.AddProfile<WebAdminMapperProfile>();
            });
            //services.AddAutoMapper(typeof(WebAdminMapperProfile));

            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(WebAdminHandlersRegistration).GetTypeInfo().Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
