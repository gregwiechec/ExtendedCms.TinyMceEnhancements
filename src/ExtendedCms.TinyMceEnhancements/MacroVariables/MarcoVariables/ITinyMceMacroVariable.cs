namespace ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;

public interface ITinyMceMacroVariable
{
    public string Key { get; }

    public string DisplayName => Key;

    public string GetValue();

    public int Rank => 100;
}