using EPiServer.Security;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;

public class UserMacroVariable(IPrincipalAccessor principalAccessor): ITinyMceMacroVariable
{
    public string Key => "USER_NAME";
    
    public string GetValue()
    {
        return principalAccessor?.Principal?.Identity?.Name ?? "";
    }
}