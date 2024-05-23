using EPiServer.ServiceLocation;

namespace ExtendedCms.TinyMceEnhancements.MacroVariables;

[Options]
public class MacroVariablesOptions
{
    public bool Enabled { get; set; } = false;

    public string MacroPrefix { get; set; } = "%%%";
    
    public string MacroPostfix { get; set; } = "%%%";
}