using EPiServer.Core.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Microsoft.AspNetCore.Http;

namespace ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

[ModuleDependency(typeof(InitializationModule))]
public class UrlProcessingInitialization : IInitializableModule
{
    private void UrlProcessingInitialization_GeneratedUrl(object? sender, UrlGeneratorEventArgs e)
    {
        if (e.Context.ContextMode != ContextMode.Default)
        {
            return;
        }

        if (e.Context.QueryCollection.AllKeys.Contains("detectFormatOptimization"))
        {
            e.Context.QueryCollection.Remove("detectFormatOptimization");

            var httpContext = ServiceLocator.Current.GetInstance<IHttpContextAccessor>().HttpContext;
            if (httpContext != null)
            {
                if (httpContext.Request.Headers.Accept.ToString().Contains("image/webp"))
                {
                    e.Context.QueryCollection.Add("format", "webp");
                    e.IsCacheable = false;
                }
            }
        }
    }

    public void Initialize(InitializationEngine context)
    {
        if (ServiceLocator.Current.GetInstance<TinyMceEnhancementsOptions>().DetectFormatOptimization)
        {
            ServiceLocator.Current.GetInstance<IContentUrlGeneratorEvents>().GeneratedUrl +=
                UrlProcessingInitialization_GeneratedUrl;            
                
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
    }
}