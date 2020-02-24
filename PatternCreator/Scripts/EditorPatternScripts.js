﻿let image = document.getElementsByClassName('imgModalStyle');

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



$(document).on("click",
    ".deleteButton",
    function (e) {
        $.ajax({
            url: "/Pattern/DeletePicture",
            data: { pictureID: $($(e.target).parents()[1]).find(".patternBut").attr("pictureID") },
            method: "POST",
            success: () => {

                console.log(e.target);
                $(e.target).parent().parent().remove();
            }

        });




    });

$(document).on("click", ".greyButton", function () {

    let block = $(`<div style="display:none;" class="deleteContain"><button class="deleteButton"style="width:100%">Удалить</button></div>`);

    $(".patternRectangle").find("#link").after(block);

    $(".deleteContain").fadeIn("slow");
});





$("body").on("click",
    function (e) {

        $(".deleteContain").hide("slow", function() {
            $(".deleteContain").remove();
        });
});


$(document).on("click",
    ".draggable-text",
    function(e) {
        let elem = $(e.target);
        elem.parent().addClass("block-visible");
        $(".draggable-text").removeAttr("readonly");
    });

$(".save").on("click", function () {
    let img = $(this).parent().parent().find(".imgModalStyle")[0];
    let kefX = img.naturalWidth / img.width;
    let kefY = img.naturalHeight / img.height;
    let bounds = $(this).parent().parent().find(".draggable-div").toArray().map(elem => {
        return [((elem.offsetLeft + 2) * kefX).toString(),
            ((elem.offsetTop + 20) * kefY).toString(),
            ((elem.offsetWidth - 4) * kefX).toString(),
            elem.parentElement.getAttribute("picture-id"),
            elem.getAttribute("position-id") || "null",
            $(elem).find(".hint-data-type").val(),
            (elem.getElementsByClassName('hint-font-size')[0].value.split('px')[0]).toString()];
    }); 
    $.ajax({
        url: "../Pattern/SetBlocks",
        method: "POST",
        data: { data: JSON.stringify({"bounds": bounds, "picId":  $(img).parent().attr("picture-id")}) }
    });
});



$(document).on("click",
    ".findIcon",
    function () {
        $(".ImageName").toArray().forEach((elem) => {
            if (!elem.innerText.includes($(this).parent().find("input").val())) {
                $(elem).parent().fadeOut("slow");

            } else {
                $(elem).parent().fadeIn("slow");
            }

        });

    });


//$(".imgModalStyle").ready(() => {
//    let positions = $("#positions div").toArray();
//    let dataPositions;
//    for (var i in positions) {
//        //dataPositions.push({
//        //    id: block.getAttribute("position-id"),
//        //    posX: block.getAttribute("position-x"),
//        //    posY: block.getAttribute("position-y"),
//        //    width: block.getAttribute("position-width"),
//        //    picId: block.getAttribute("picture-id")
//        //});
//        let block = positions[i];
//        let blockHtml = $("<div class='draggable-div block-visible'><div class='draggable-hint'><div class='hint-left'><select class='hint-data-type'><option>Имя</option><option>Фамилия</option><option>Отчество</option><option>Статичный текст</option></select></div><img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/><img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/></div><textarea class='draggable-text'/></div>");

//        blockHtml.find(".draggable-text").css("width", block.getAttribute("position-width"));
//        blockHtml.attr("position-id", block.getAttribute("position-id"));
//        $(".img-wrap[picture-id='" + block.getAttribute("picture-id") + "']").append(blockHtml);
//        blockHtml.draggable({
//            containment: blockHtml.parent(),
//            handle: "img.block-move-svg"
//        });
//        let img = $(".img-wrap[picture-id='" + block.getAttribute("picture-id") + "']").find('.imgModalStyle')[0];
//        if (img) {
//            let kefX = img.naturalWidth / img.width;
//            let kefY = img.naturalHeight / img.height;
//            blockHtml.css("left", Math.round(block.getAttribute("position-x").replace(/,/, '.') / kefX - 2) + "px");
//            blockHtml.css("top", Math.round(block.getAttribute("position-y").replace(/,/, '.') / kefY - 20) + "px");
//            blockHtml.css("width", Math.round(block.getAttribute("position-width").replace(/,/, '.') / kefX + 4) + "px");
//            blockHtml.find(".hint-data-type").val(block.getAttribute("Type"));
//        }

//    }
//});

$(document).on("click", ".patternBut", function () {
    let id = $(this).attr("pictureid");

    if ($(".img-wrap[picture-id='" + id + "']").find("draggable-div").length == 0) {
        setTimeout(() => {
            let positions = $(`[position-id][picture-id='${id}']`).toArray();
            $(".draggable-div").remove();
            for (let i in positions) {
                let block = positions[i];
                let blockHtml = $(`
                <div class='draggable-div block-visible'>
                    <div class='draggable-hint'>
                        <div class='hint-left'>
                            <select class='hint-data-type'>
                                <option>Имя</option>
                                <option>Фамилия</option>
                                <option>Отчество</option>
                                <option>Статичный текст</option>
                            </select>
                            <input class='hint-font-size' type='number' min='12' max='72' value='16' />
                        </div>
                        <img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/>
                        <img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/>
                    </div>
                    <textarea class='draggable-text'/>
                </div>`);
                blockHtml.attr("position-id", block.getAttribute("position-id"));
                $(".img-wrap[picture-id='" + block.getAttribute("picture-id") + "']").append(blockHtml);
                blockHtml.draggable({
                    containment: blockHtml.parent(),
                    handle: "img.block-move-svg"
                });
                let img = $(".img-wrap[picture-id='" + block.getAttribute("picture-id") + "']").find('.imgModalStyle')[0];
                if (img) {
                    let kefX = img.naturalWidth / img.width;
                    let kefY = img.naturalHeight / img.height;
                    blockHtml.css("left", Math.round(block.getAttribute("position-x").replace(/,/, '.') / kefX - 2) + "px");
                    blockHtml.css("top", Math.round(block.getAttribute("position-y").replace(/,/, '.') / kefY - 20) + "px");
                    blockHtml.find(".draggable-text").css("width", Math.round(block.getAttribute("position-width").replace(/,/, '.') / kefX + 4) + "px");
                    blockHtml.find(".hint-data-type").val(block.getAttribute("Type"));
                    blockHtml.find(".hint-font-size").val(block.getAttribute("font-size"));
                    $(".hint-font-size").trigger("change");
                }
            }
        },1000);
    }
    
});


$(document).on("change", ".hint-font-size", function () {
    $(this).parent().parent().parent().find(".draggable-text").css("font-size", this.value + "px");
})