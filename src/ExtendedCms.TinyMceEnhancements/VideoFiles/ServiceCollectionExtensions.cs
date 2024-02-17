using EPiServer.Cms.TinyMce.Core;
using EPiServer.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ExtendedCms.TinyMceEnhancements.VideoFiles;

public static class ServiceCollectionExtensions
{
    private const string PluginName = "video-files-plugin";

    public static IServiceCollection AddVideoFilesPlugin(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.Configure<TinyMceConfiguration>(config =>
        {
            var defaultSettings = config.Default();
            defaultSettings.ConfigureVideoFilesPlugin();
        });

        return services;
    }

    public static TinyMceSettings ConfigureVideoFilesPlugin(this TinyMceSettings tinyMceSettings)
    {
        const string path = "ClientResources/Scripts/video-files-plugin.js";

        var pluginUrl = Paths.ToClientResource(typeof(ServiceCollectionExtensions).Assembly, path);
        tinyMceSettings
            .AddExternalPlugin(PluginName, pluginUrl, (settings, content, propertyName) => { });
        return tinyMceSettings;
    }
}
