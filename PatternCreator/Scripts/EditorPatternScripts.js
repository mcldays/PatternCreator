let image = document.getElementsByClassName('imgModalStyle');

wheelzoom(document.getElementsByClassName('imgModalStyle'), { maxZoom: 2 });
var inputElement = $("#submitImg");
inputElement[0].addEventListener("change", handleFiles, false);
function handleFiles() {
    $(inputElement).submit();
}

$(".rectangleWhite").click(function () {
    console.log("kek.")
    $(".imgModalStyle").css("background-size", "740px 600px");
    $(".imgModalStyle").css("background-position", "0px 0px");
});

$(document).on("dragstart", "block-delete-svg", function (e) {
    e.preventDefault();
});

$(document).on("click", ".block-delete-svg", function () {
    $(this).parent().parent().remove();
});
$("#add-text").on("click", () => {
    let block = $("<div class='draggable-div block-visible'><div class='draggable-hint'><div class='hint-left'><select class='hint-data-type'><option>Имя</option><option>Фамилия</option><option>Отчество</option><option>Статичный текст</option></select></div><img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/><img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/></div><textarea class='draggable-text'/></div>");
    $("#img-wrap").append(block);
    block.draggable({
        containment: $("#img-wrap"),
        handle: "img.block-move-svg"
    });
});

$(".imgModalStyle").on("click", function(e) {
    let elem = $(e.target);
    if (!elem.hasClass("draggable-div")) {
        $(".draggable-div").removeClass("block-visible");
        $(".draggable-text").attr("readonly", "true");
    }
});

$(document).on("click", ".draggable-text", function (e) {
    let elem = $(e.target);
    elem.parent().addClass("block-visible");
    $(".draggable-text").removeAttr("readonly");
})