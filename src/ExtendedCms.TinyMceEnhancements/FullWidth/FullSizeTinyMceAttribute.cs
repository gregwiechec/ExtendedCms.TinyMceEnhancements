using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ExtendedCms.TinyMceEnhancements.FullWidth;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FullSizeTinyMceAttribute: Attribute, IDisplayMetadataProvider
{
    public WidthType EditorWidth { get; set; } = WidthType.Full;

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var extendedMetadata = context.DisplayMetadata.AdditionalValues[ExtendedMetadata.ExtendedMetadataDisplayKey] as ExtendedMetadata;

        if (extendedMetadata == null)
        {
            return;
        }

        extendedMetadata.EditorConfiguration["fullWithTinyMce"] = true;
        extendedMetadata.EditorConfiguration["tinyMceWith"] = EditorWidth.ToString();
    }
}

public enum WidthType
{
    /// <summary>
    ///
    /// </summary>
    Full,

    /// <summary>
    ///
    /// </summary>
    Centered
}
