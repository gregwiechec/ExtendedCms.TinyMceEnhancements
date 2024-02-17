define([
    "dojo/_base/declare",
    "dojo/when",
    "epi/shell/TypeDescriptorManager",
    "epi-addon-tinymce/plugins/epi-dnd-processor/dndDropProcessor"
], function (
    declare,
    when,
    TypeDescriptorManager,
    DndDropProcessor
) {


    return function initializeVideo() {
        var originalConstructor = DndDropProcessor.prototype.constructor;
        DndDropProcessor.prototype.constructor = function (editor) {
            this._editor = editor;
            return originalConstructor.apply(this, arguments);
        }
        DndDropProcessor.prototype.constructor.nom = "constructor"


        var originalProcessData = DndDropProcessor.prototype.processData;
        DndDropProcessor.prototype.processData = function (dropItem) {
            when(dropItem.data).then(function (model) {
                var typeId = model.typeIdentifier;

                var editorDropBehaviour = TypeDescriptorManager.getValue(typeId, "editorDropBehaviour");

                if (editorDropBehaviour !== 2) {
                    return originalProcessData.apply(this, arguments);
                }

                if (model && model.name) {
                    if (supportedExtensions.indexOf(getExtension(model.name)) !== -1) {
                        _insertHtml();
                        return;
                    }
                }

                return originalProcessData.apply(this, arguments);
            })
        }
        DndDropProcessor.prototype.processData.nom = "processData";
    }
});
