using EPiServer.ServiceLocation;
using ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

namespace ExtendedCms.TinyMceEnhancements;

[Options]
public class TinyMceEnhancementsOptions
{
    public ImageAttributeSettings ImageAttributes  { get; set; }
}