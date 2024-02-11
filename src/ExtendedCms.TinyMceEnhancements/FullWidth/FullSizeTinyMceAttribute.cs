using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ExtendedCms.TinyMceEnhancements.FullWidth;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FullSizeTinyMceAttribute: Attribute, IDisplayMetadataProvider
{
    public WidthType EditorWidth { get; set; } = WidthType.Full;

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        if (context.DisplayMetadata.AdditionalValues[ExtendedMetadata.ExtendedMetadataDisplayKey] is not ExtendedMetadata extendedMetadata)
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
    /// Set TinyMCE full width
    /// </summary>
    Full,

    /// <summary>
    /// Set TinyMCE editor centered width
    /// </summary>
    Centered
}
