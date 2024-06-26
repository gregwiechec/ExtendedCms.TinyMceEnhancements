﻿using EPiServer.Cms.TinyMce.Core;
using EPiServer.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

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
            defaultSettings.ConfigureImagePlugin();
        });

        return services;
    }

    public static TinyMceSettings ConfigureImagePlugin(this TinyMceSettings tinyMceSettings)
    {
        const string path = "ClientResources/Scripts/advanced-image-plugin.js";

        var pluginUrl = Paths.ToClientResource(typeof(ServiceCollectionExtensions).Assembly, path);
        tinyMceSettings
            .AddExternalPlugin(PluginName, pluginUrl, (settings, content, propertyName) => { });
        return tinyMceSettings;
    }
}