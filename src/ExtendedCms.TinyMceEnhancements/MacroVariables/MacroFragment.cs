using EPiServer.Core.Html.StringParsing;
using EPiServer.Core.Transfer;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables;

public class MacroFragment(
    StaticFragment fragment,
    MacroVariablesOptions options,
    IEnumerable<ITinyMceMacroVariable> macroVariables) : IStringFragment, IReferenceMap
{
    public void RemapPermanentLinkReferences(IDictionary<Guid, Guid> idMap)
    {
        fragment.RemapPermanentLinkReferences(idMap);
    }

    public IList<Guid> ReferencedPermanentLinkIds => fragment.ReferencedPermanentLinkIds;
    
    public string GetEditFormat()
    {
        return fragment.GetEditFormat();
    }

    public string GetViewFormat()
    {
        if (!macroVariables.Any())
        {
            return fragment.GetViewFormat();
        }
        var result = fragment.GetViewFormat();

        macroVariables = macroVariables.OrderBy(x => x.Rank).ToList();
        foreach (var macroVariable in macroVariables)
        {
            var macro = options.MacroPrefix + macroVariable.Key + options.MacroPostfix;
            if (result.Contains(macro))
            {
                result = result.Replace(macro, macroVariable.GetValue());
            }
        }
        
        return result;
    }

    public string InternalFormat => fragment.InternalFormat;
}