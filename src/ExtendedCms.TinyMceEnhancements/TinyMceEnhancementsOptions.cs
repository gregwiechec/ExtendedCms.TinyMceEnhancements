using EPiServer.ServiceLocation;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAlt;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

namespace ExtendedCms.TinyMceEnhancements;

[Options]
public class TinyMceEnhancementsOptions
{
    public ImageAttributeSettings ImageAttributes  { get; set; }

    public ImageRestrictionsSettings ImageRestrictions { get; set; }

    public ImageAltTextSettings ImageAltTextSettings { get; set; }

    /// <summary>
    /// When <see langword="true" /> then full width TinyMCe editor is enabled. Default <see langword="false" /> and FullWidth is not active
    /// </summary>
    public bool FullWidthEnabled { get; set; } = false;

    /// <summary>
    /// When <see langword="true" /> then video tag is enabled in TinyMCe editor. Default <see langword="false" /> and default drop behaviour will be used
    /// </summary>
    public bool VideoFilesEnabled { get; set; } = false;
}
