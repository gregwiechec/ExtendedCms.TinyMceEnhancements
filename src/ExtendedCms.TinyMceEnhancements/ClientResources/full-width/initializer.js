define([
    "dojo/_base/declare",
    "epi/shell/form/formFieldRegistry",
    "extended-cms-tinymce-enhancements/full-width/extended-field"
], function (
    declare,
    formFieldRegistry,
    ExtendedField
) {
    return function initializeFullWidth() {
        var defaultFactory = formFieldRegistry.get(formFieldRegistry.type.field);

        formFieldRegistry.remove(formFieldRegistry.type.field);

        formFieldRegistry.add({
            type: formFieldRegistry.type.field,
            hint: "",
            factory: function (widget, parent) {
                if (!widget.params.fullWithTinyMce) {
                    return defaultFactory(widget, parent);
                }

                widget.settings.width = "100%";

                if (widget.params.tinyMceWith === "Full") {
                    widget.domNode.classList.add("extended-width-widget");
                } else {
                    widget.domNode.classList.add("extended-width-widget-centered");
                }
                widget.stateNode.style.width = "100%";

                var wrapper = new ExtendedField({
                    labelTarget: widget.checkbox ? widget.checkbox.id : widget.id,
                    label: widget.label,
                    tooltip: widget.tooltip,
                    readonlyIconDisplay: widget.readOnly,
                    hasFullWidthValue: widget.useFullWidth
                });

                wrapper.own(widget.watch("readOnly", function (name, oldValue, newValue) {
                    wrapper.set("readonlyIconDisplay", newValue);
                }));

                wrapper.own(widget.on("TinyMCEInitialized",
                    function () {
                        var tabContainer =
                            document.querySelector(
                                ".epi-main-content .epi-cmsEditingForm form .dijitTabContainer");

                        var contentHeaderArea =
                            tabContainer.querySelector("[data-dojo-attach-point=topPaneContainerNode]");

                        var tabs =
                            tabContainer.querySelector(".dijitTabListContainer-top");

                        var totalHeight = tabContainer.getBoundingClientRect().height;
                        var headerHeight = contentHeaderArea.getBoundingClientRect().height;
                        var tabsHeight = tabs.getBoundingClientRect().height;

                        var offset = 100;

                        widget.editor.iframeElement.style.height = (totalHeight - headerHeight - tabsHeight - offset) + "px";
                    }));

                return wrapper;
            }
        });
    }
});
