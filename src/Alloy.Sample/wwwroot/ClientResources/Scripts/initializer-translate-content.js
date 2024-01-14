define([
    "dojo/_base/declare",
    "dojo/when",
    "epi/_Module",
    "epi",
    "epi/dependency",
    "epi/shell/DialogService",
    "epi-cms/contentediting/CreateLanguageBranch",
    "epi-cms/contentediting/viewmodel/CreateLanguageBranchViewModel",
    "epi/i18n!epi/nls/admin.convertpagetype"
], function (
    declare,
    when,
    _Module,
    epi,
    dependency,
    dialogService,
    CreateLanguageBranch,
    CreateLanguageBranchViewModel,
    adminResources
) {
    var ExtendedCreateLanguageBranchViewModel = declare([CreateLanguageBranchViewModel], {
        buildContentObject: function() {
            var result = this.inherited(arguments);
            if (typeof this.convertLocalBlocks !== "undefined") {
                result.properties["ichangetrackable_createdby"] = this.convertLocalBlocks.toString();
            }
            return result;
        }
    })
    
    function patchSaveMethod() {
        var originalOnSave = CreateLanguageBranch.prototype._onSave;
        CreateLanguageBranch.prototype._onSave = function (e) {
            var self = this;
            hasInlineBlocks(this.model.masterLanguageVersionId).then(result => {
                if (!result) {
                    self.model.convertLocalBlocks = undefined;
                    originalOnSave.apply(self, arguments);
                    return;
                }
                dialogService.confirmation({
                    title: "Confirm",
                    description: "Should copy all ContentArea inline blocks?",
                    confirmActionText: adminResources.convert,
                    cancelActionText: epi.resources.action.ignore
                }).then(function () {
                    self.model.convertLocalBlocks = true;
                    originalOnSave.apply(self, arguments);
                }).otherwise(function() {
                    self.model.convertLocalBlocks = false;
                    originalOnSave.apply(self, arguments);
                });                
            });
        }
        CreateLanguageBranch.prototype._onSave.nom = "_onSave";
    }
    
    // check if content contains contentarea with inline blocks
    let metadataManager, contentDataStore;
    function hasInlineBlocks(contentLink) {
        metadataManager = metadataManager || dependency.resolve("epi.shell.MetadataManager");
        return new Promise(resolve => {
            metadataManager.getMetadataForType("EPiServer.Core.ContentData", {contentLink: contentLink}).then((metadata) => {
                const contentAreaProperties = metadata.properties
                    .filter(x => x.modelType === "EPiServer.Core.ContentArea" && x.settings && x.settings.isLanguageSpecific)
                    .map(x => x.name);
                if (contentAreaProperties.length === 0) {
                    resolve(false);
                    return;
                }

                contentDataStore = contentDataStore || dependency.resolve("epi.storeregistry").get("epi.cms.contentdata");
                when(contentDataStore.get(contentLink)).then(function (content) {
                    const hasInlineBlocks = contentAreaProperties.some(x => {
                        const propertyName = Object.keys(content.properties).find(key=> key.toLowerCase() === x.toLowerCase());
                        const property = content.properties[propertyName];
                        if (property && property.some(p => p.inlineBlockData)) {
                            return true;
                        }
                        return false;
                    });
                    resolve(hasInlineBlocks);
                });
            });
        });
    }
    
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);
            CreateLanguageBranch.prototype.modelType = ExtendedCreateLanguageBranchViewModel;
            patchSaveMethod();
        }
    });
});