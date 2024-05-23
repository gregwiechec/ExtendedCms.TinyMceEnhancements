using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;

[ServiceConfiguration(typeof(ITinyMceMacroVariable))]
public class UserMacroVariable(IPrincipalAccessor principalAccessor): ITinyMceMacroVariable
{
    public string Key => "USER_NAME";
    
    public string GetValue()
    {
        return principalAccessor?.Principal?.Identity?.Name ?? "";
    }

    public int Rank => 100;
}