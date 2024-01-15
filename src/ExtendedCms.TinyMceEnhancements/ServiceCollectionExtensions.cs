using EPiServer.Shell.Modules;
using ExtendedCms.TinyMceEnhancements.AdvancedImageResolution;
using Microsoft.Extensions.DependencyInjection;

namespace ExtendedCms.TinyMceEnhancements;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTinyMceEnhancements(this IServiceCollection services)
    {
        services.Configure<ProtectedModuleOptions>(
            pm =>
            {
                if (!pm.Items.Any(i =>
                        i.Name.Equals("ExtendedCms.TinyMceEnhancements", StringComparison.OrdinalIgnoreCase)))
                {
                    pm.Items.Add(new ModuleDetails { Name = "ExtendedCms.TinyMceEnhancements", Assemblies = { typeof(ServiceCollectionExtensions).Assembly.GetName().Name }  });
                }
            });

        services.AddAdvancedImagePlugin();

        return services;
    }
}
