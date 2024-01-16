using EPiServer.Core.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace ExtendedCms.TinyMceEnhancements.AdvancedImageResolution;

[ModuleDependency(typeof(InitializationModule))]
public class UrlProcessingInitialization : IInitializableModule
{
    private void UrlProcessingInitialization_GeneratedUrl(object? sender, UrlGeneratorEventArgs e)
    {
        if (e.Context.ContextMode != ContextMode.Default)
        {
            return;
        }

        if (e.Context.Url.ToString().EndsWith("png"))
        {
            e.IsCacheable = false;
        }

        if (e.Context.Url.QueryCollection.AllKeys.Contains("extendedconfiguration"))
        {
            e.Context.Url.QueryCollection.Remove("extendedconfiguration");
            e.IsCacheable = false;
        }
    }

    public void Initialize(InitializationEngine context)
    {
        ServiceLocator.Current.GetInstance<IContentUrlGeneratorEvents>().GeneratedUrl += UrlProcessingInitialization_GeneratedUrl;
        ServiceLocator.Current.GetInstance<IContentUrlGeneratorEvents>().GeneratingUrl += UrlProcessingInitialization_GeneratingUrl;
    }

    private void UrlProcessingInitialization_GeneratingUrl(object? sender, UrlGeneratorEventArgs e)
    {
        if (e.Context.ContextMode != ContextMode.Default)
        {
            return;
        }
        if (e.Context.Url.QueryCollection.AllKeys.Contains("extendedconfiguration"))
        {
            e.Context.Url.QueryCollection.Remove("extendedconfiguration");
            e.IsCacheable = false;
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
    }
}