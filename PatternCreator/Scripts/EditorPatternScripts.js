let image = document.getElementsByClassName('imgModalStyle');

//wheelzoom(document.getElementsByClassName('imgModalStyle'), { maxZoom: 2 });

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
$(".add-text").on("click", function() {
    let modelBlockOne = $(this).parent().parent();
    let block = $("<div class='draggable-div block-visible'><div class='draggable-hint'><div class='hint-left'><select class='hint-data-type'><option>Имя</option><option>Фамилия</option><option>Отчество</option><option>Статичный текст</option></select></div><img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/><img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/></div><textarea class='draggable-text'/></div>");
    modelBlockOne.find(".img-wrap").append(block);
    block.draggable({
        containment: modelBlockOne.find(".img-wrap"),
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

$(".save").on("click", function () {
    let bounds = $(this).parent().parent().find(".draggable-div").toArray().map(elem => {
        return [elem.offsetLeft, elem.offsetTop, elem.offsetWidth, elem.parentElement.getAttribute("picture-id"), elem.getAttribute("position-id")];
    }); 
    $.ajax({
        url: "../Pattern/SetBlocks",
        method: "POST",
        data: { data: JSON.stringify(bounds) }
    });
});

let positions = $("#positions div").toArray();
let dataPositions;
for (var i in positions) {
    //dataPositions.push({
    //    id: block.getAttribute("position-id"),
    //    posX: block.getAttribute("position-x"),
    //    posY: block.getAttribute("position-y"),
    //    width: block.getAttribute("position-width"),
    //    picId: block.getAttribute("picture-id")
    //});
    let block = positions[i];
    let blockHtml = $("<div class='draggable-div block-visible'><div class='draggable-hint'><div class='hint-left'><select class='hint-data-type'><option>Имя</option><option>Фамилия</option><option>Отчество</option><option>Статичный текст</option></select></div><img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/><img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/></div><textarea class='draggable-text'/></div>");
    
    blockHtml.find(".draggable-text").css("width", block.getAttribute("position-width"));
    blockHtml.attr("position-id", block.getAttribute("position-id"));
    $(".img-wrap[picture-id='" + block.getAttribute("picture-id") + "']").append(blockHtml);
    blockHtml.draggable({
        containment: blockHtml.parent(),
        handle: "img.block-move-svg"
    });
    blockHtml.css("left", block.getAttribute("position-x") + "px");
    blockHtml.css("top", block.getAttribute("position-y") + "px");
}

