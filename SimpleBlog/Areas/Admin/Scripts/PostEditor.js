$(document).ready(function() {
    var $tagEditor = $(".post-tag-editor");
    $tagEditor.find(".tag-select").on("click","> li >a",function(e) {
        e.preventDefault();
        var $this = $(this);
        var $tagParent = $this.closest("li");
        $tagParent.toggleClass("selected");
        var selected = $tagParent.hasClass("selected");
        $tagParent.find(".selected-input").val(selected);

    });

    var $addTagButton = $tagEditor.find(".add-tag-button");
    var $newTagName = $tagEditor.find(".new-tag-name");
    function addTag(name) {
        var newTagIndex = $tagEditor.find(".tag-select > li").length-1;
        $tagEditor
            .find(".tag-select > li.template")
            .clone()
            .removeClass("template")
            .addClass("selected")
            .find(".name").text(name).end()
            .find(".name-input").val(name).attr("name", "Tags[" + newTagIndex + "].Name").end()
            .find(".selected-input").attr("name", "Tags[" + newTagIndex + "].IsChecked").val(true).end()
            .appendTo($tagEditor.find(".tag-select"));

        $newTagName.val("");
        $addTagButton.prop("disabled", true);
    }
    function checkTagName(name) {
        var isExists = true;
        $tagEditor.find(".tag-select > li > a").each(function() {
            var $this = $(this);
            var tagName = $this.text();
            if (name.toLowerCase() === tagName.toLowerCase()) {
                isExists = false;
            }
        });
        return isExists;
    }

    $addTagButton.click(function(e) {
        e.preventDefault();
        if (checkTagName($newTagName.val())) {
            addTag($newTagName.val());
        }
    });
    $newTagName.keyup(function() {
        if ($newTagName.val().trim().length > 0) {
            $addTagButton.prop("disabled", false);
        } else {
            $addTagButton.prop("disabled", true);
        }
    }).keydown(function(e) {
        if (e.which !== 13) {
            return;
        }
        e.preventDefault();
        if (checkTagName($newTagName.val())) {
            addTag($newTagName.val());
        }
    });
});