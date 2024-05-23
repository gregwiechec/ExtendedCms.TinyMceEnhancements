using EPiServer.Cms.TinyMce.Core;
using EPiServer.Shell;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables;

public static class ServiceCollectionExtensions
{
    private const string PluginName = "macro-variables-plugin";
    
    public static IServiceCollection AddTinyMceMacroVariables(this IServiceCollection services, bool registerUserMacro = true)
    {
        services.Configure<MacroVariablesOptions>(o =>
        {
            o.Enabled = true;
        });
        
        services.Configure<TinyMceConfiguration>(config =>
        {
            var defaultSettings = config.Default();
            defaultSettings.ConfigureMacroVariablesPlugin();
            defaultSettings.AppendToolbar("macro-variables-button");
        });

        if (registerUserMacro)
        {
            services.TryAddTransient<ITinyMceMacroVariable, UserMacroVariable>();
        }
        
        return services;
    }
    
    public static TinyMceSettings ConfigureMacroVariablesPlugin(this TinyMceSettings tinyMceSettings)
    {
        const string path = "ClientResources/Scripts/macro-variables-plugin.js";

        var pluginUrl = Paths.ToClientResource(typeof(ServiceCollectionExtensions).Assembly, path);
        tinyMceSettings
            .AddExternalPlugin(PluginName, pluginUrl, (settings, content, propertyName) => { });
        return tinyMceSettings;
    }
}

//TODO: add plugin that displays macro variables list