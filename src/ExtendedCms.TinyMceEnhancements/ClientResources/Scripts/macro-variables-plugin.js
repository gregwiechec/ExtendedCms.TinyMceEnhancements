window.tinymce.PluginManager.add("macro-variables-plugin", function (editor, url) {

    require(["extended-cms-tinymce-enhancements/settings"], function (settings) {
        if (!settings || !settings.macroValues) {
            return;
        }

        editor.ui.registry.addMenuButton("macro-variables-button", {
            text: 'My button',
            fetch: (callback) => {
                const items = settings.macroValues.map((x) => ({
                    type: "menuitem",
                    text: x.key,
                    onAction: () => editor.insertContent(x.value)  
                }));
                callback(items);
            }
        });
    });

    return {
        getMetadata: function () {
            return {
                name: "Macro varibles plugin",
                url: "https://www.gregwiechec.com"
            };
        }
    };
});