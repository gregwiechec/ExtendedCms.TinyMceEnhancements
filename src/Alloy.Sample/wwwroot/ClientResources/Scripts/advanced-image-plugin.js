window.tinymce.PluginManager.add("advanced-image-plugin", function (editor, url) {

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
        const width = imgEl.getAttribute("width");
        const height = imgEl.getAttribute("height");

        let src = imgEl.getAttribute("src");
        src = replaceImgUrlQuery(src,"width", width);
        src = replaceImgUrlQuery(src,"height", height);

        imgEl.setAttribute("src", src);
        imgEl.setAttribute("data-original", src);
        imgEl.setAttribute("data-mce-src", src);

        setTimeout(() => {
            editor.fire("change");
        }, 100);
    }
    
    editor.on('ObjectResized', function(e) {
        if (e.target.tagName === "IMG") {
            const img = e.target;
            updateImageUrl(img);
        }
    });
    
    editor.on('ExecCommand', function(e) {
        if (e.command !== "mceInsertContent") {
            return;
        }
        setTimeout(() => {
            const nodeEl = editor.selection.getNode();
            const img = nodeEl.querySelector("img");

            updateImageUrl(img);
        }, 100)
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