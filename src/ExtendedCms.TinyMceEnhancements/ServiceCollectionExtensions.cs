using EPiServer.Shell.Modules;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAlt;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;
using Microsoft.Extensions.DependencyInjection;

namespace ExtendedCms.TinyMceEnhancements;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTinyMceEnhancements(this IServiceCollection services, bool configureImageAttributes = true,
        bool configureImageAlt = true)
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

        if (configureImageAttributes)
        {
            services.AddAdvancedImagePlugin();
        }

        if (configureImageAlt)
        {
            services.AddAdvancedImageAltPlugin();
        }

        return services;
    }
}
