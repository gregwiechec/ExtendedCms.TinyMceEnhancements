window.tinymce.PluginManager.add("advanced-image-plugin", function (editor, url) {

    require(["extended-cms-tinymce-enhancements/settings"], function (settings) {
        function replaceImgUrlQuery(uri, key, value) {
            var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
            var separator = uri.indexOf('?') !== -1 ? "&" : "?";
            if (uri.match(re)) {
                return uri.replace(re, '$1' + key + "=" + value + '$2');
            }
            else {
                return uri + separator + key + "=" + value;
            }
        }

        function updateImageUrl(imgEl) {
            if (!settings || !settings.imageAttributes && !settings.imageAttributes.imageSizeSettings) {
                return;
            }
            const setWidth = settings.imageAttributes.imageSizeSettings.setWidth;
            const setHeight = settings.imageAttributes.imageSizeSettings.setHeight;
            const staticAttributes = settings.imageAttributes.staticAttributes || [];

            if (!setWidth && !setHeight && staticAttributes.length === 0) {
                return;
            }

            const widthAttributeName = (settings.imageAttributes.imageSizeSettings || {}).widthName || "width";
            const heightAttributeName = (settings.imageAttributes.imageSizeSettings || {}).heightName || "height";

            const width = imgEl.getAttribute("width");
            const height = imgEl.getAttribute("height");

            let src = imgEl.getAttribute("src");
            if (setWidth) {
                src = replaceImgUrlQuery(src, widthAttributeName, width);
            }

            if (setHeight) {
                src = replaceImgUrlQuery(src, heightAttributeName, height);
            }

            staticAttributes.forEach(function (attribute) {
                src = replaceImgUrlQuery(src, attribute.name, attribute.value);
            });
            
            imgEl.setAttribute("src", src);
            imgEl.setAttribute("data-original", src);
            imgEl.setAttribute("data-mce-src", src);


            setTimeout(() => {
                editor.fire("change");
            }, 100);
        }

        editor.on('ObjectResized', function (e) {
            if (e.target.tagName === "IMG") {
                const img = e.target;
                updateImageUrl(img);
            }
        });

        editor.on('ExecCommand', function (e) {
            if (e.command !== "mceInsertContent") {
                return;
            }
            setTimeout(() => {
                const nodeEl = editor.selection.getNode();
                const img = nodeEl.querySelector("img");

                updateImageUrl(img);
            }, 100)
        });
    });
    
    return {
        getMetadata: function () {
            return {
                name: "Advanced image plugin",
                url: "https://www.gregwiechec.com"
            };
        }
    };
});