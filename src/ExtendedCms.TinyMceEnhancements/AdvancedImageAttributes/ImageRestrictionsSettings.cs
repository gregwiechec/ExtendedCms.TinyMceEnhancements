namespace ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

public class ImageRestrictionsSettings
{
    public int MaxWidth { get; set; } = 0;

    public int MaxHeight { get; set; } = 0;

    public bool KeepRatio { get; set; } = false;
}