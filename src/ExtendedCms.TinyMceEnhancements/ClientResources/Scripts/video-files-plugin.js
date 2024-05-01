window.tinymce.PluginManager.add("video-files-plugin", function (editor, url) {

    function getExtension(filename) {
        return filename.split('.').pop().toLowerCase();
    }

    var supportedExtensions = ["mp4", "webm",  "ogg"];

    function _insertHtml(html) {
        editor.focus();
        if (editor.execCommand("mceInsertContent", false, html)) {
            editor.fire("change");
        }
    }

    require(["dojo/when",
        "epi/shell/TypeDescriptorManager",
        "extended-cms-tinymce-enhancements/settings",
        "epi-addon-tinymce/plugins/epi-dnd-processor/dndDropProcessor"
    ], function (when, TypeDescriptorManager, settings, DndDropProcessor) {
        if (!settings || !settings.videoFilesEnabled) {
            return;
        }

        setTimeout(function() {
            editor.addCommand("mceEPiProcessDropItem", function (ui, dropItem) {
                when(dropItem.data).then(function (model) {
                    var typeId = model.typeIdentifier;

                    var editorDropBehaviour = TypeDescriptorManager.getValue(typeId, "editorDropBehaviour");

                    if (editorDropBehaviour !== 2) {
                        var processor = new DndDropProcessor(editor);
                        return processor.processData(dropItem);
                    }

                    if (model && model.name) {
                        var extension = getExtension(model.publicUrl);
                        if (supportedExtensions.indexOf(extension) !== -1) {
                            var template = `<video controls width="400"><source src="{0}" type="video/{1}" />
Download the
<a href="{0}">{1}</a>
</video>`;
                            template= template.replace("{0}", model.publicUrl);
                            template= template.replace("{0}", model.publicUrl);
                            template= template.replace("{1}", extension);
                            template= template.replace("{1}", extension);
                            _insertHtml(template);
                            return;
                        }
                    }

                    var processor = new DndDropProcessor(editor);
                    return processor.processData(dropItem);
                })
            });
        }, 1000);
    });

    return {
        getMetadata: function () {
            return {
                name: "Video files plugin",
                url: "https://www.gregwiechec.com"
            };
        }
    };
});
