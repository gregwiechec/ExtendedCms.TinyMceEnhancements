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

            const widthAttributeName = (settings.imageAttributes.imageSizeSettings || {}).widthName;
            const heightAttributeName = (settings.imageAttributes.imageSizeSettings || {}).heightName;

            const setWidth = !!widthAttributeName;
            const setHeight = !!heightAttributeName;
            const staticAttributes = settings.imageAttributes.staticAttributes || [];

            if (!setWidth && !setHeight && staticAttributes.length === 0) {
                return;
            }

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
            //imgEl.setAttribute("data-original", src);
            imgEl.setAttribute("data-mce-src", src);


            setTimeout(() => {
                editor.fire("change");
            }, 100);
        }

        function updateImageSize(img) {
            var maxWidth = (settings.imageRestrictions || {}).maxWidth || 0;
            var maxHeight = (settings.imageRestrictions || {}).maxHeight || 0;
            var keepRatio = (settings.imageRestrictions || {}).keepRatio || false;

            if (maxWidth <= 0 && maxHeight <= 0 && keepRatio === false) {
                return;
            }

            if (maxWidth > 0 && img.clientWidth > maxWidth) {
                img.width = maxWidth;
            }

            if (maxHeight > 0 && img.clientHeight > maxHeight) {
                img.height = maxHeight;
            }

            function updateRatio() {
                if (img.clientWidth === img.naturalWidth && img.clientHeight === img.naturalHeight) {
                    return;
                }

                var newWidth = (img.clientHeight * img.naturalWidth) / img.naturalHeight;
                var newHeight = (img.clientWidth * img.naturalHeight) / img.naturalWidth;

                var canUpdateWidth = maxWidth <= 0 || newWidth < maxWidth;
                if (canUpdateWidth && newWidth * img.clientHeight > newHeight * img.clientWidth) {
                    img.width = newWidth;
                } else {
                    img.height = newHeight;
                }
            }

            if (keepRatio) {
                updateRatio();
            }

            setTimeout(() => {
                editor.fire("change");
            }, 100);
        }

        editor.on('ObjectResized', function (e) {
            if (e.target.tagName === "IMG") {
                const img = e.target;
                updateImageSize(img);
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

                if (img) {
                    updateImageSize(img);
                    updateImageUrl(img);
                }
            }, 100);
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