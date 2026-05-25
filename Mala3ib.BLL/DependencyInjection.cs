using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Mala3ib.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLLValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            return services;
        }
        public static IServiceCollection AddBLLMapping(this IServiceCollection services)
        {
            var MappingConfig = TypeAdapterConfig.GlobalSettings;
            MappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(MappingConfig));
            return services;
        }
    }
}
