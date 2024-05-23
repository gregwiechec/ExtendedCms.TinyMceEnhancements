using EPiServer.Core.Html.StringParsing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables;

[InitializableModule]
[ModuleDependency(typeof(FrameworkInitialization))]
public class ShellInitialization : IConfigurableModule
{
    public void Initialize(InitializationEngine context)
    {
        
    }

    public void Uninitialize(InitializationEngine context)
    {
    }

    public void ConfigureContainer(ServiceConfigurationContext context)
    {
        context.Services.Intercept<IFragmentParser>((locator, instance) =>
            locator.GetInstance<MacroVariablesOptions>().Enabled
                ? new CustomFragmentParser(instance, locator.GetInstance<IOptions<MacroVariablesOptions>>(), locator.GetAllInstances<ITinyMceMacroVariable>())
                : instance);
    }
}

public class CustomFragmentParser(
    IFragmentParser fragmentParser,
    IOptions<MacroVariablesOptions> options,
    IEnumerable<ITinyMceMacroVariable> macroVariables) : IFragmentParser
{
    public StringFragmentCollection Parse(string content, FragmentParserMode parserMode, bool evaluateHash)
    {
        if (!macroVariables.Any())
        {
            return fragmentParser.Parse(content, parserMode, evaluateHash);
        }

        var result = fragmentParser.Parse(content, parserMode, evaluateHash);

        for (var index = 0; index < result.Count; index++)
        {
            var stringFragment = result[index];
            if (stringFragment is StaticFragment fragment)
            {
                var stringFragmentInternalFormat = stringFragment.InternalFormat;
                if (ContainsMacro(stringFragmentInternalFormat))
                {
                    result[index] = new MacroFragment(fragment, options.Value, macroVariables);
                }
            }
        }

        return result;
    }

    private bool ContainsMacro(string stringFragmentInternalFormat)
    {
        return stringFragmentInternalFormat.Contains(options.Value.MacroPrefix) &&
               stringFragmentInternalFormat.Contains(options.Value.MacroPostfix);
    }
}