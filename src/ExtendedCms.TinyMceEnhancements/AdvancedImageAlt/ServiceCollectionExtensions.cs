using EPiServer.Cms.TinyMce.Core;
using EPiServer.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ExtendedCms.TinyMceEnhancements.AdvancedImageAlt;

public static class ServiceCollectionExtensions
{
    private const string PluginName = "advanced-image-alt-plugin";

    public static IServiceCollection AddAdvancedImageAltPlugin(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.Configure<TinyMceConfiguration>(config =>
        {
            var defaultSettings = config.Default();
            defaultSettings.ConfigureImageAltPlugin();
        });

        return services;
    }

    public static TinyMceSettings ConfigureImageAltPlugin(this TinyMceSettings tinyMceSettings)
    {
        const string path = "ClientResources/Scripts/advanced-image-alt-plugin.js";

        var pluginUrl = Paths.ToClientResource(typeof(ServiceCollectionExtensions).Assembly, path);
        tinyMceSettings
            .AddExternalPlugin(PluginName, pluginUrl, (settings, content, propertyName) => { });
        return tinyMceSettings;
    }
}