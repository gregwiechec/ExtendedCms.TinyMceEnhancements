using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using ExtendedCms.TinyMceEnhancements.FullWidth;

namespace AlloyTemplates.Models.Pages;

[ContentType(GUID = "A5B4B89D-0BE5-49D5-A7D6-EEC41F8C0C82", GroupName = Global.GroupNames.Specialized)]
[AvailableContentTypes(Availability.Specific, IncludeOn = new[] { typeof(StartPage) })]
public class FullSizeTinyMCETestPage: SitePageData
{
    [Display(Name = "HTML Editor", GroupName = "Test", Order = 10)]
    public virtual XhtmlString HtmlEditorStandard { get; set; }

    [Display(Name = "HTML Editor 2", GroupName = "Test2", Order = 20)]
    [FullSizeTinyMce(EditorWidth = WidthType.Centered)]
    public virtual XhtmlString HtmlEditorCentered { get; set; }

    [Display(Name = "HTML Editor 3", GroupName = "Test3", Order = 20)]
    [FullSizeTinyMce(EditorWidth = WidthType.Full)]
    public virtual XhtmlString HtmlEditorFullWidth { get; set; }
}
