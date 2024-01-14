using EPiServer.Cms.TinyMce.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AlloyTemplates.Business.Initialization;

public static class TinyMceConfigurationInitialization
{
    public static IServiceCollection CustomizeTinyMce(this IServiceCollection services)
    {
        services.Configure<TinyMceConfiguration>(config =>
        {
            config.Default().AddPlugin("code").AppendToolbar("code");
        });

        return services;
    }
}