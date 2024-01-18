namespace ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

public class ImageAttributeSettings
{
    public IEnumerable<ImageQueryStringAttribute> StaticAttributes { get; set; }
}

public class ImageQueryStringAttribute
{
    public string Name { get; set; }
    public string Value { get; set; }
}