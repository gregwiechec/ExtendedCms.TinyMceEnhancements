using EPiServer.Shell.Modules;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAlt;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;
using ExtendedCms.TinyMceEnhancements.MacroVariables;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;
using ExtendedCms.TinyMceEnhancements.VideoFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTinyMceMacroVariables(this IServiceCollection services, bool registerUserMacro = true)
    {
        services.Configure<MacroVariablesOptions>(o =>
        {
            o.Enabled = true;
        });

        if (registerUserMacro)
        {
            services.TryAddTransient<ITinyMceMacroVariable, UserMacroVariable>();
        }
        
        return services;
    }
}
