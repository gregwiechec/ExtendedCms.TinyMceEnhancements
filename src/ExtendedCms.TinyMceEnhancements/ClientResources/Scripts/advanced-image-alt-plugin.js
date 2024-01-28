window.tinymce.PluginManager.add("advanced-image-alt-plugin", function (editor, url) {

    require(["extended-cms-tinymce-enhancements/show-alt-text-dialog"], function (showAltTextDialog) {
        editor.on('ExecCommand', function (e) {
            if (e.command !== "mceInsertContent") {
                return;
            }
            setTimeout(() => {
                const nodeEl = editor.selection.getNode();
                const img = nodeEl.querySelector("img");

                function setAltText(altText) {
                    img.setAttribute("alt", altText);

                    editor.focus();
                    setTimeout(() => {
                        editor.fire("change");
                    }, 100);
                }

                showAltTextDialog(img).then((altText) => {
                    setAltText(altText);
                }).catch(() => {
                    setAltText(altText);
                });
            }, 100);
        });
    });
    
    return {
        getMetadata: function () {
            return {
                name: "Advanced image alt plugin",
                url: "https://www.gregwiechec.com"
            };
        }
    };
});