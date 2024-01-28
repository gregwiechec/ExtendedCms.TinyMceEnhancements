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
}