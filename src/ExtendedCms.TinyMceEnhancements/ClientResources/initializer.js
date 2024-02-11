define([
    "dojo/_base/declare",

    "epi/_Module",

    "extended-cms-tinymce-enhancements/settings",
    "extended-cms-tinymce-enhancements/full-width/initializer"
], function (
    declare,

    _Module,

    settings,
    fullWidthInitializer
) {
    return declare([_Module], {
        initialize: function () {
            for (const property in this._settings.tinyMceEnhancementsOptions) {
                settings[property] = this._settings.tinyMceEnhancementsOptions[property];
            }

            if (this._settings.tinyMceEnhancementsOptions.fullWidthEnabled) {
                fullWidthInitializer();
            }
        }
    });
});
