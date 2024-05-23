using AlloyTemplates.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;

namespace AlloyTemplates.Business;

[ServiceConfiguration(typeof(ITinyMceMacroVariable))]
public class TinyMcePhoneMacro(IContentLoader contentLoader): ITinyMceMacroVariable
{
    public string Key => "CONTACT_PHONE";

    public string DisplayName => "Contact phone";

    public string GetValue()
    {
        return contentLoader.Get<StartPage>(ContentReference.StartPage).ContactPhone;
    }
}