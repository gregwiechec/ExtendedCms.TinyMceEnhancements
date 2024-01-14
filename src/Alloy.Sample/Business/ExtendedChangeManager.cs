using System.Collections.Generic;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest;
using EPiServer.Cms.Shell.Workspace.Committers;
using EPiServer.Core;
using EPiServer.Data.Entity;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AlloyTemplates.Business;

[ServiceConfiguration(typeof(InlineBlocksCopier))]
public class InlineBlocksCopier
{
    private readonly IContentRepository _contentRepository;
    private readonly IBlockPropertyFactory _blockPropertyFactory;

    public InlineBlocksCopier(IContentRepository contentRepository,
        IBlockPropertyFactory blockPropertyFactory)
    {
        _contentRepository = contentRepository;
        _blockPropertyFactory = blockPropertyFactory;
    }

    public void CopyInlineBlocks(ContentReference masterLanguageId, ContentReference translatedReference)
    {
        var master = _contentRepository.Get<IContent>(masterLanguageId);
        var translated = _contentRepository.Get<IContent>(translatedReference);
        if (translated is IReadOnly)
        {
            translated = (IContent)((IReadOnly) translated).CreateWritableClone();
        }

        var needSave = false;
        foreach (var propertyData in master.Property)
        {
            if (propertyData.PropertyValueType != typeof(ContentArea))
            {
                continue;
            }

            if (propertyData.Value == null)
            {
                continue;
            }
            
            if (!propertyData.IsLanguageSpecific)
            {
                continue;
            }

            var contentArea = (ContentArea)propertyData.Value;
            foreach (var contentAreaItem in contentArea.Items)
            {
                if (ContentReference.IsNullOrEmpty(contentAreaItem.ContentLink))
                {
                    var translatedPropertyData = translated.Property[propertyData.Name];
                    if (translatedPropertyData.Value == null)
                    {
                        translatedPropertyData.Value = new ContentArea();
                    }

                    var translatedContentArea = (ContentArea) translatedPropertyData.Value;

                    var blockData = _blockPropertyFactory.CreateFromSharedInstance(contentAreaItem.InlineBlock);
                    
                    translatedContentArea.Items.Add(new ContentAreaItem
                    {
                        InlineBlock = blockData,
                        RenderSettings = contentAreaItem.RenderSettings
                    });
                    needSave = true;
                } 
            }
        }

        if (needSave)
        {
            _contentRepository.Save(translated, SaveAction.ForceCurrentVersion, AccessLevel.NoAccess);
        }
    }
}

public class ExtendedChangeManager: IContentChangeManager
{
    private readonly IContentChangeManager _defaultContentChangeManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly InlineBlocksCopier _inlineBlocksCopier;

    public ExtendedChangeManager(IContentChangeManager defaultContentChangeManager) : this(defaultContentChangeManager,
        ServiceLocator.Current.GetInstance<IHttpContextAccessor>(),
        ServiceLocator.Current.GetInstance<InlineBlocksCopier>())
    {
        _defaultContentChangeManager = defaultContentChangeManager;
    }

    public ExtendedChangeManager(IContentChangeManager defaultContentChangeManager,
        IHttpContextAccessor httpContextAccessor,
        InlineBlocksCopier inlineBlocksCopier)
    {
        _defaultContentChangeManager = defaultContentChangeManager;
        _httpContextAccessor = httpContextAccessor;
        _inlineBlocksCopier = inlineBlocksCopier;
    }

    public CommitResult Commit(ContentReference contentReference, SaveAction action)
    {
        return _defaultContentChangeManager.Commit(contentReference, action);
    }

    public ContentReference Create(ContentReference parentLink, int contentTypeId, long? resourceFolderId, bool createAsLocalAsset,
        ILocalAssetNameGenerator nameGenerator, IDictionary<string, object> properties)
    {
        return _defaultContentChangeManager.Create(parentLink,  contentTypeId,  resourceFolderId,  createAsLocalAsset,
             nameGenerator,  properties);
    }

    public ContentReference Create(ContentReference parentLink, int contentTypeId, long? resourceFolderId, bool createAsLocalAsset,
        string name, IDictionary<string, object> properties)
    {
        return _defaultContentChangeManager.Create(parentLink,  contentTypeId,  resourceFolderId,  createAsLocalAsset,
             name,  properties);
    }

    public ContentReference GetOrCreateContentAssetsFolder(ContentReference parentLink)
    {
        return _defaultContentChangeManager.GetOrCreateContentAssetsFolder(parentLink);
    }

    public ContentReference CreateLanguageBranch(ContentReference contentReference, string name, string languageBranch,
        IDictionary<string, object> properties)
    {
        var copyInlineBlocks = false;
        var fakeKey = "ichangetrackable_createdby";
        if (properties.ContainsKey(fakeKey))
        {
            copyInlineBlocks = properties[fakeKey]?.ToString() == "true";
            properties.Remove(fakeKey);
        }

        var result = _defaultContentChangeManager.CreateLanguageBranch(contentReference,  name,  languageBranch, properties);

        if (copyInlineBlocks)
        {
            _inlineBlocksCopier.CopyInlineBlocks(contentReference, result);
        }
        
        return result;
    }

    public ContentReference Copy(IContent source, IContent destination, bool createAsLocalAsset)
    {
        return _defaultContentChangeManager.Copy(source, destination, createAsLocalAsset);
    }

    public ContentReference Move(IContent source, IContent destination, bool createAsLocalAsset)
    {
        return _defaultContentChangeManager.Move(source, destination, createAsLocalAsset);
    }

    public IEnumerable<ContentReference> TranslateAncestors(IContent content)
    {
        return _defaultContentChangeManager.TranslateAncestors(content);
    }

    public PropertiesUpdateResult UpdateContentProperties(ContentReference contentReference, IDictionary<string, string> properties)
    {
        return _defaultContentChangeManager.UpdateContentProperties( contentReference, properties);
    }

    public PropertiesUpdateResult UpdateContentProperties(ContentReference contentReference, IDictionary<string, string> properties,
        SaveAction saveAction)
    {
        return _defaultContentChangeManager.UpdateContentProperties(contentReference, properties, saveAction);
    }
}

[ModuleDependency(typeof(EPiServer.Web.InitializationModule))] 
public class ChangeManagerInitializer : IConfigurableModule 
{ 
    public void ConfigureContainer(ServiceConfigurationContext context) 
    { 
        context.Services.Intercept<IContentChangeManager>( 
            (locator, defaultChangeManager) => new ExtendedChangeManager(defaultChangeManager)); 
    } 
    public void Initialize(InitializationEngine context) 
    { 
    } 
    public void Uninitialize(InitializationEngine context) 
    { 
    } 
} 