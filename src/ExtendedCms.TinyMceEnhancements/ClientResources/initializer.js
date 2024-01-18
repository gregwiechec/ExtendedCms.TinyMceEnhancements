define([
    "dojo/_base/declare",

    "epi/_Module",

    "extended-cms-tinymce-enhancements/settings"
], function (
    declare,

    _Module,

    settings
) {
    return declare([_Module], {
        initialize: function () {
            for (const property in this._settings.tinyMceEnhancementsOptions) {
                settings[property] = this._settings.tinyMceEnhancementsOptions[property];
            }
        }
    });
});
