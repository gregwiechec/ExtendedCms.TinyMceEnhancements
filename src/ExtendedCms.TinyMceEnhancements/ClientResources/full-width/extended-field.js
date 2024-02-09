define([
    "dojo/_base/declare",
    "epi/shell/form/Field",
    "xstyle/css!extended-cms-tinymce-enhancements/full-width/styles.css"
], function (
    declare,
    Field
) {
    return declare([Field], {
        postCreate: function () {
            this.inherited(arguments);

            this.domNode.classList.add("extended-field");
        },

        _setLabelAttr: function (value) {
            this.inherited(arguments);
            this.labelNode.classList.add("dijitHidden");
        }
    });
});
