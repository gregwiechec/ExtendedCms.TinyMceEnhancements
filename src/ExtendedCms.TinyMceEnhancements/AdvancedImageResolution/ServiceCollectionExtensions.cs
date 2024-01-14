using EPiServer.Cms.TinyMce.Core;
using EPiServer.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ExtendedCms.TinyMceEnhancements.AdvancedImageResolution;

public static class ServiceCollectionExtensions
{
    private const string PluginName = "advanced-image-plugin";

    public static IServiceCollection AddAdvancedImagePlugin(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.Configure<TinyMceConfiguration>(config =>
        {
            var defaultSettings = config.Default();

            const string path = "/ClientResources/Scripts/advanced-image-plugin.js";

            var pluginUrl = Paths.ToClientResource("app", path);
            defaultSettings
                .AddExternalPlugin(PluginName, pluginUrl, (settings, content, propertyName) => {

                })
                .AppendToolbar(PluginName);
        });

        return services;
    }
}