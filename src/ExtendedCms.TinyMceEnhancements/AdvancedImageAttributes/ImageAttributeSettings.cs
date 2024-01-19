namespace ExtendedCms.TinyMceEnhancements.AdvancedImageAttributes;

public class ImageAttributeSettings
{
    public IEnumerable<ImageQueryStringAttribute> StaticAttributes { get; set; }

    public ImageSizeSettings ImageSizeSettings { get; set; }
}

public class ImageQueryStringAttribute
{
    public string Name { get; set; }
    public string Value { get; set; }
}

public class ImageSizeSettings
{
    public bool SetWidth { get; set; }
    public string WidthName { get; set; }
    public bool SetHeight { get; set; }
    public string HeightName { get; set; }
}