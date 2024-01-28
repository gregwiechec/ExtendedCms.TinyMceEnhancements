define([
    "dojo/_base/declare",
    "dojo/aspect",

    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",

    "epi/dependency",
    "epi/Url",

    "epi/shell/widget/dialog/Dialog",

    "epi-addon-tinymce/ContentService",

    "extended-cms-tinymce-enhancements/settings",

    "dijit/form/TextBox",

    "xstyle/css!./show-alt-text-dialog.css"
], function (
    declare,
    aspect,

    _Widget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,

    dependency,
    Url,

    Dialog,

    ContentService,

    settings
) {
    var DialogContent = declare([_Widget, _TemplatedMixin, _WidgetsInTemplateMixin], {
        templateString: "<div><div data-dojo-attach-point='textbox' data-dojo-type='dijit/form/TextBox'></div></div>",

        getText: function () {
            return this.textbox.get("value");
        },

        setText: function (value) {
            this.textbox.set("value", value);
        }
    });

    var store;

    return function showDialog(img) {
        var contentService = new ContentService();

        var dialogContent = new DialogContent();

        var dialog = new Dialog({
            title: "Image ALT text",
            dialogClass: "show-alt-text-dialog",
            content: dialogContent,
            defaultActionsVisible: true,
            confirmActionText: "Update ALT text"
        });

        dialog.own(dialogContent);

        dialog.startup();

        function getDefaultAltText() {
            if (!settings.imageAltTextSettings ||
                !settings.imageAltTextSettings.imageAltAttributes ||
                settings.imageAltTextSettings.imageAltAttributes.length === 0) {
                return new Promise(resolve => resolve(""));
            }

            var url = new Url(img.src);
            return new Promise(resolve => {
                contentService.getContentFromUrl(url.path).then((content) => {
                    store = store || dependency.resolve("epi.storeregistry").get("epi.cms.contentdata");
                    store.get(content.contentLink).then((contentItem) => {
                        var hasDefaultAltText = settings.imageAltTextSettings.imageAltAttributes.some(x => {
                            if (contentItem.properties[x]) {
                                resolve(contentItem.properties[x]);
                                return true;
                            }
                            return false;
                        });
                        if (!hasDefaultAltText) {
                            resolve("");
                        }
                    });
                }).otherwise(function () {
                    resolve("");
                });
            });
        }

        return new Promise((resolve, reject) => {
            getDefaultAltText().then((altTextPropertyValue) => {
                dialog.own(
                    aspect.after(dialog, "onExecute", () => resolve(dialogContent.getText()), true),
                    aspect.after(dialog, "onCancel", () => reject(), true)
                );
                dialog.show();
                dialogContent.setText(altTextPropertyValue);
            });
        });
    }
});
