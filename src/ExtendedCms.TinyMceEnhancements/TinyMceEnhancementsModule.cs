using EPiServer.Framework.TypeScanner;
using EPiServer.Framework.Web.Resources;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using ExtendedCms.TinyMceEnhancements.MacroVariables;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace ExtendedCms.TinyMceEnhancements;

public class TinyMceEnhancementsModule : ShellModule
{
    private TinyMceEnhancementsOptions _tinyMceEnhancementsOptions;
    private IEnumerable<ITinyMceMacroVariable> _macroVariables;
    private readonly IOptions<MacroVariablesOptions> _macroVariablesOptions;

    public TinyMceEnhancementsModule(string name, string routeBasePath, string resourceBasePath) : base(name,
        routeBasePath, resourceBasePath)
    {
        _tinyMceEnhancementsOptions = ServiceLocator.Current.GetInstance<TinyMceEnhancementsOptions>();
        _macroVariablesOptions = ServiceLocator.Current.GetInstance<IOptions<MacroVariablesOptions>>();
        _macroVariables = ServiceLocator.Current.GetAllInstances<ITinyMceMacroVariable>();
    }

    public TinyMceEnhancementsModule(string name, string routeBasePath, string resourceBasePath,
        ITypeScannerLookup typeScannerLookup, IFileProvider virtualPathProvider) : base(name, routeBasePath,
        resourceBasePath, typeScannerLookup, virtualPathProvider)
    {
    }

    public override ModuleViewModel CreateViewModel(ModuleTable moduleTable, IClientResourceService clientResourceService)
    {
        var viewModel = new TinyMceEnhancementsViewModel(this, clientResourceService)
        {
            TinyMceEnhancementsOptions = _tinyMceEnhancementsOptions,
            MacroValues = GetMacroValues()
        };
        return viewModel;
    }

    private IEnumerable<KeyValuePair<string, string>> GetMacroValues()
    {
        return _macroVariables
            .Select(x => new KeyValuePair<string, string>(x.DisplayName,
                _macroVariablesOptions.Value.MacroPrefix + x.Key + _macroVariablesOptions.Value.MacroPostfix))
            .OrderBy(x => x.Key);
    }
}

public class TinyMceEnhancementsViewModel(ShellModule module, IClientResourceService clientResourceService)
    : ModuleViewModel(module, clientResourceService)
{
    public TinyMceEnhancementsOptions TinyMceEnhancementsOptions { get; set; }

    public IEnumerable<KeyValuePair<string, string>> MacroValues { get; set; }
}