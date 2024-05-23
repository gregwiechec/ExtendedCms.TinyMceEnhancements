namespace ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;

public interface ITinyMceMacroVariable
{
    public string Key { get; }

    public string GetValue();

    public int Rank { get; }
}