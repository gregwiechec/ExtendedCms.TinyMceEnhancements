using EPiServer.Framework.TypeScanner;
using EPiServer.Shell.Modules;
using Microsoft.Extensions.FileProviders;

namespace ExtendedCms.TinyMceEnhancements;

public class TinyMceEnhancementsModule : ShellModule
{
    public TinyMceEnhancementsModule(string name, string routeBasePath, string resourceBasePath) : base(name,
        routeBasePath, resourceBasePath)
    {
    }

    public TinyMceEnhancementsModule(string name, string routeBasePath, string resourceBasePath,
        ITypeScannerLookup typeScannerLookup, IFileProvider virtualPathProvider) : base(name, routeBasePath,
        resourceBasePath, typeScannerLookup, virtualPathProvider)
    {
    }
}