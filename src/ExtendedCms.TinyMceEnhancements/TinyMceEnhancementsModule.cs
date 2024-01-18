using EPiServer.Framework.TypeScanner;
using EPiServer.Framework.Web.Resources;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using Microsoft.Extensions.FileProviders;

namespace ExtendedCms.TinyMceEnhancements;

public class TinyMceEnhancementsModule : ShellModule
{
    private TinyMceEnhancementsOptions _tinyMceEnhancementsOptions;

    public TinyMceEnhancementsModule(string name, string routeBasePath, string resourceBasePath) : base(name,
        routeBasePath, resourceBasePath)
    {
        _tinyMceEnhancementsOptions = ServiceLocator.Current.GetInstance<TinyMceEnhancementsOptions>();
    }

    public TinyMceEnhancementsModule(string name, string routeBasePath, string resourceBasePath,
        ITypeScannerLookup typeScannerLookup, IFileProvider virtualPathProvider) : base(name, routeBasePath,
        resourceBasePath, typeScannerLookup, virtualPathProvider)
    {
    }

    public override ModuleViewModel CreateViewModel(ModuleTable moduleTable, IClientResourceService clientResourceService)
    {
        var viewModel = new TinyMceEnhancementsViewModel(this, clientResourceService);
        viewModel.TinyMceEnhancementsOptions = _tinyMceEnhancementsOptions;
        return viewModel;
    }
}

public class TinyMceEnhancementsViewModel : ModuleViewModel
{
    public TinyMceEnhancementsOptions TinyMceEnhancementsOptions { get; set; }

    public TinyMceEnhancementsViewModel(ShellModule module, IClientResourceService clientResourceService) : base(module, clientResourceService)
    {
    }
}