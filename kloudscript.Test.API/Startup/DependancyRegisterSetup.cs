using kloudscript.Test.API.Services;

namespace kloudscript.Test.API.Startup
{
    public static class DependancyRegisterSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IMemeoryConfigService, MemeoryConfigService>();
            services.AddSingleton<IUrlshorteningService, UrlshorteningService>();
            services.AddSingleton<IStateByZipService, StateByZipService>();
            services.AddSingleton<ICsvParserService, CsvParserService>();
            services.AddSingleton<IDsAlgoService, DsAlgoService>();  
            return services;
        }
    }
}
