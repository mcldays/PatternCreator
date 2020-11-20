$(document).ready(
    function () {
    $("tr:not(tr:last)>td:not(:last-child)").each(function () {
        $(this).resizable({
            containment: $(this).closest("table.tableRed"),
            handles: 's, e',
            minHeight: 10,
            minWidth: 10

        });
    });
    setAutocomplete();
let image = document.getElementsByClassName('imgModalStyle');
var imgModal;
$(document).on("change", "#submitImg", handleFiles);
$(document).on("change", "#submitStamp", handleFiles);
$(document).on("click",
    ".AddStamp",
    function() {
        var stamp = $(this).closest("div.card").find("img").prop('src');
        var stamp_id = $(this).closest("div.card").find("img").attr('stamp-id');
        $("#stampModal").modal("hide");
        var block = $(
            "<div stamp-id='" + stamp_id +"' style='height:60px; width:60px; border-thickness: 2px; border-color: #464646' class='stampWrap'><img width='100%' class='' src = '" +
            stamp +
            "'/><div class='deleteStamp'>×<div/></div >");
        $(imgModal.closest(".img-wrap")).append(block);


        $(block).draggable({
            containment: $(imgModal.closest(".img-wrap")),
            handle: block.find("img")
        });


        $(block).resizable({
            containment: $(imgModal),
            handles: 'se',
            aspectRatio: true,
            minHeight: 20,
            minWidth: 20
        });


            });
        $(document).on("click",
            ".AddPhoto",
            function () {
                imgModal = $(this).closest('.modal').find(".img-wrap");
                var block = $(
                    "<div style='height:80px; width:60px; position:absolute; top:0; border: 2px dotted #464646' class='photoWrap'><div style='height:100%; width:100%; display: flex; justify-content: center; align-items: center; font-weight: bold; cursor:grab;'>Фото 3х4</div><div class='deleteStamp'>×<div/></div >");
                $(imgModal.closest(".img-wrap")).append(block);


                $(block).draggable({
                    containment: $(imgModal),
                    handle: block.find("div")
                });


                $(block).resizable({
                    containment: $(imgModal.find('img')),
                    handles: 'se',
                    aspectRatio: true,
                    minHeight: 40,
                    minWidth: 30
                });


            });

function handleFiles() {
    $(this).submit();
}

function getColumnCells(elem) {
    return $(elem)
        .filter('th, td')
        .filter(':not([colspan])')
        .closest('table')
        .find('tr')
        .filter(':not(:has([colspan]))')
        .children(':nth-child(' + ($(elem).index() + 1) + ')');
}

function setAutocomplete() {
    $(".draggable-text.autotext").each(function() {
        var b = $(this);
        b.autocomplete({
            source: autotexts,
            select: function(event, ui) {
                b.html(ui.item.value);
                b.val(blockHtml.find(".draggable-text").html());
                b.html("");
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function(ul, item) {
            var bl = $(`<li><span></span></li>`);
            $(bl).find("span").html(item.value);
            return bl.appendTo(ul);
        };
    });
}

var after = 0;


$(document).on('change',
    ".hint-data-type",
    function() {
        if ($(this).val() === "Статичный текст из бд") {
            $(this).closest(".draggable-div").find(".draggable-text").addClass("autotext");
            setAutocomplete();
        } else {
            $(this).closest(".draggable-div").find(".draggable-text.autotext").autocomplete("destroy")
                .removeClass("autotext");
        }
    });

$(document).on('resizestart',
    "td",
    function () {

       
        var direction = $(this).data('ui-resizable').axis;
        if (direction === 's') {
            var targ = $(this).closest("tr");
            targ.children("td").height("auto");
            var maxh = 0;

            $("tr").each(function() {
                if (!$(this).is(targ) && !$(this).is($(targ).next()))
                    maxh += $(this).height();
            });
            maxh = $(this).closest("table.tableRed").height() - maxh - 12;
            $(targ).children("td:not(:last-child)").resizable("option", "maxHeight", maxh);
            $("td").each(function() {
                if (!targ.children("td").is($(this))) {
                    $(this).height($(this).height());
                }
            });
            var next = $(this).closest('tr').next('tr').children("td");
            next.height("auto");
        } else {
            var cells = getColumnCells(this);
            $(cells).each(function() {
                $(this).width("auto");
            });
            var maxw = 0;

            $(this).prevAll("td").each(function() {
                maxw += $(this).width();
            });
            $(this).nextAll("td").filter(":not(:first)").each(function() {
                maxw += $(this).width();
            });

            maxw = $(this).closest("table.tableRed").width() - maxw - 24;
            $(cells).filter(".ui-resizable").resizable("option", "maxWidth", maxw);
            $(cells).each(function() {
                $(this).prevAll("td").each(function() {
                    $(this).width($(this).width());
                });
                $(this).nextAll("td").filter(":not(:first)").each(function() {
                    $(this).width($(this).width());

                });
            });
            $(cells).each(function() {
                $(this).next("td").width("auto");
            });
        }

    });
$(document).on('resizestop',
    "td",
    function() {
        var direction = $(this).data('ui-resizable').axis;
        if (direction === 's') {
            var next = $(this).closest('tr').next('tr').children("td");
            next.height(next.height());
        } else {
            var cells = getColumnCells(this);
            $(cells).each(function() {
                $(this).next("td").width($(this).next("td").width());
            });
        }

    });


$(document).on("dragstart",
    "block-delete-svg",
    function(e) {
        e.preventDefault();
    });

$(document).on("click",
    ".block-delete-svg",
    function() {
        $(this).closest(".draggable-div").remove();
    });
$(".add-text").on("click",
    function() {
        let modelBlockOne = $(this).parent().parent();
        let block = $(`
                <div class='draggable-div block-visible'>
                    <div class='draggable-hint'>
                        <div class='hint-left'>
                            <select class='hint-data hint-data-type'>
                                <optgroup label='Данные пользователя'>
                                    <option>Ф.И.О</option>
                                    <option>Ф.И.О(Д.п.)</option>
                                    <option>Имя</option>
                                    <option>Имя(Д.п)</option>
                                    <option>Фамилия</option>
                                    <option>Фамилия(Д.п)</option>
                                    <option>Отчество</option>
                                    <option>Отчество(Д.п)</option>
                                    <option>Должность</option>
                                    <option>Образование</option>
                                    <option>Компания</option>
                                </optgroup>
                                <optgroup label='Даты'>
                                    <option>Число *конец</option>
                                    <option>Месяц *конец</option>
                                    <option>Год *конец</option>
                                    <option>(дд.мм.гггг) г. *конец</option>
                                    <option>(дд.мм.гггг) *конец</option>
                                    <option>Число *начало</option>
                                    <option>Месяц *начало</option>
                                    <option>Год *начало</option>                                    
                                    <option>(дд.мм.гггг) г. *начало</option>
                                    <option>(дд.мм.гггг) *начало</option> 
                                    <option>Число *действителено до</option>
                                    <option>Месяц *действителено до</option>
                                    <option>Год *действителено до</option>                                    
                                    <option>(дд.мм.гггг) г. *действителено до</option>
                                    <option>(дд.мм.гггг) *действителено до</option>    
                                </optgroup>
                                    <option>Программа обучения</option>
                                    <option>Квалификация</option>
                                    <option>Сфера деятельности</option>
                                    <option>Обучающая организация</option>
                                    <option>Лицензия</option>
                                    <option>Количество учебных часов</option>                                    
                                    <option>Статичный текст</option>
                                    <option>Статичный текст из бд</option>
                                    <option>Ручной ввод</option>
                                <optgroup label='Нумерация'>
                                    <option>Номер протокола</option>                                    
                                    <option>Номер корочки</option>                               
                                    <option>Номер бланка</option> 
                                </optgroup>                               
                            </select>
                            <select class='hint-data hint-data-weight'>
                                <option>Regular</option>
                                <option>Bold</option>
                                <option>Regular Underline</option>
                                <option>Bold Underline</option>
                            </select>
                            <select class='hint-data hint-data-align'>
                                <option>Слева</option>
                                <option>По центру</option>
                                <option>По ширине</option>
                                <option>Справа</option>
                            </select>
                            <input class='hint-font-size' type='number' min='8' max='72' value='16' />
                        </div>
                        <div class='hint-right'>
                           <img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/>
                           <img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/>
                            <input class='hint-data hint-rows' type='number' min='1' max='6' value='1' />
                        </div>                        
                    </div>
                    <textarea class='draggable-text' rows="1">Текст</textarea>
                </div>`);
        
        modelBlockOne.find(".img-wrap").append(block);
        $(".hint-font-size").trigger("change");
        block.draggable({
            snap: "td",
            snapTolerance: 10,
            containment: modelBlockOne.find(".img-wrap"),
            handle: "img.block-move-svg"
        });
    });

$(".imgModalStyle, .BlockTable").on("click",
    function(e) {
        let elem = $(e.target);
        if (!elem.hasClass("draggable-div")) {
            $(".draggable-div").removeClass("block-visible");
            $(".draggable-text").attr("readonly", "true");
        }
    });


$(document).on("click",
    ".deleteButton",
    function(e) {
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

$(document).on("click",
    ".greyButton",
    function() {
        let block = $(
            `<div style="display:none;" class="deleteContain"><button class="deleteButton"style="width:100%">Удалить</button></div>`);
        $(".patternRectangle").find("#link").after(block);
        $(".deleteContain").fadeIn("slow");
    });


$("body").on("click",
    function(e) {
        $(".deleteContain").hide("slow",
            function() {
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

$(".save").on("click",
    function () {
        
        let img = $(this).parent().parent().find(".imgModalStyle")[0];
        let kefX = img.naturalWidth / img.width;
        let kefY = img.naturalHeight / img.height;
        let bounds = $(this).parent().parent().find(".draggable-div").toArray().map(elem => {
            return [
                ((elem.offsetLeft)).toString(),
                ((elem.offsetTop)).toString(),
                ((elem.offsetWidth)).toString(),
                elem.parentElement.getAttribute("picture-id"),
                elem.getAttribute("position-id") || -1,
                $(elem).find(".hint-data-type").val(),
                ((elem.offsetHeight)).toString(),
                Math.round(elem.getElementsByClassName('hint-font-size')[0].value.split('pt')[0]).toString(),
                $(elem).find(".draggable-text").val(),
                $(elem).find(".hint-data-weight").val(),
                $(elem).find(".hint-data-align").val()
            ];
        });
        let stamps = $(this).parent().parent().find(".stampWrap").toArray().map(elem => {
            return {
                PosX: ((elem.offsetLeft) ).toString(),
                PosY: ((elem.offsetTop) ).toString(),
                Width: ((elem.offsetWidth) ).toString(),
                Height: ((elem.offsetHeight) ).toString(),
                StampId: elem.getAttribute("stamp-id") || -1,
                StampPositionId: elem.getAttribute("stamp-posid") || -1,
                PicId: 0
            };
            
        });
        let photos = $(this).parent().parent().find(".photoWrap").toArray().map(elem => {
            return {
                PhotoModelId: elem.getAttribute("photo-id") || -1,
                PosX: ((elem.offsetLeft)).toString(),
                PosY: ((elem.offsetTop)).toString(),
                Width: ((elem.offsetWidth)).toString(),
                Height: ((elem.offsetHeight)).toString(),
                PicId: 0
            };

        });
        $(".preloader").toggleClass("done");
        $(".modal").modal("hide");
        $.ajax({
            url: "../Pattern/SetBlocks",
            method: "POST",
            data: {
                data: JSON.stringify({
                    "bounds": bounds,
                    "stamps": stamps,
                    "photos": photos,
                    "picId": $(img).parent().attr("picture-id"),
                    "Id": $(img).parent().parent().parent().find("[name='Id']").val(),
                    "Name": $(img).parent().parent().parent().find("[name='Name']").val(),
                    "NaturalHeight": $(img).parent().parent().parent().find("[name='NaturalHeight']").val(),
                    "NaturalWidth": $(img).parent().parent().parent().find("[name='NaturalWidth']").val()

                })
            },
            success: () => {
                document.location = "../Pattern/EditorPattern";
            }
        });
    });

$(document).on("focusin",
    ".draggable-div",
    function() {
        $(this).find(".draggable-hint").fadeIn(200);
    });
$(document).on("focusout",
    ".draggable-div",
    function() {
        $(this).find(".draggable-hint").fadeOut(200);
    });
function check(element, index, array) {
    var el = $(element);
    console.log(el.picture_id);
    return el.picture_id === id;
};

$(document).on("click",
    ".patternBut",
    function() {
        var id = $(this).attr("pictureid");
        $(".img-wrap[picture-id='" + id + "']>.draggable-div").remove();
        $(".img-wrap[picture-id='" + id + "']>.stampWrap").remove();
        var target_positions = positions.find(t => t.picture_id==id);
        
        $(target_positions.positions).each(function() {
            var block = this;
            
            var textreplace = block.text.replace(/\*newline\*/g,"\n");
                
            let blockHtml = $(`
                <div class='draggable-div block-visible'>
                   <div class='draggable-hint'>
                        <div class='hint-left'>
                            <select class='hint-data hint-data-type'>
                                 <optgroup label='Данные пользователя'>
                                    <option>Ф.И.О</option>
                                    <option>Ф.И.О(Д.п.)</option>
                                    <option>Имя</option>
                                    <option>Имя(Д.п)</option>
                                    <option>Фамилия</option>
                                    <option>Фамилия(Д.п)</option>
                                    <option>Отчество</option>
                                    <option>Отчество(Д.п)</option>
                                    <option>Должность</option>
                                    <option>Образование</option>
                                    <option>Компания</option>
                                </optgroup>
                                <optgroup label='Даты'>
                                    <option>Число *конец</option>
                                    <option>Месяц *конец</option>
                                    <option>Год *конец</option>
                                    <option>(дд.мм.гггг) г. *конец</option>
                                    <option>(дд.мм.гггг) *конец</option>
                                    <option>Число *начало</option>
                                    <option>Месяц *начало</option>
                                    <option>Год *начало</option>                                    
                                    <option>(дд.мм.гггг) г. *начало</option>
                                    <option>(дд.мм.гггг) *начало</option> 
                                    <option>Число *действителено до</option>
                                    <option>Месяц *действителено до</option>
                                    <option>Год *действителено до</option>                                    
                                    <option>(дд.мм.гггг) г. *действителено до</option>
                                    <option>(дд.мм.гггг) *действителено до</option>                                
                                </optgroup>
                                    <option>Программа обучения</option>
                                    <option>Квалификация</option>
                                    <option>Сфера деятельности</option>
                                    <option>Обучающая организация</option>
                                    <option>Лицензия</option>
                                    <option>Количество учебных часов</option>                                    
                                    <option>Статичный текст</option>
                                    <option>Статичный текст из бд</option>
                                    <option>Ручной ввод</option>
                                <optgroup label='Нумерация'>
                                    <option>Номер протокола</option>                                    
                                    <option>Номер корочки</option>                              
                                    <option>Номер бланка</option> 
                                </optgroup>
                            </select>
                            <select class='hint-data hint-data-weight'>
                                <option>Regular</option>
                                <option>Bold</option>
                                <option>Regular Underline</option>
                                <option>Bold Underline</option>
                            </select>
                            <select class='hint-data hint-data-align'>
                                <option>Слева</option>
                                <option>По центру</option>
                                <option>По ширине</option>
                                <option>Справа</option>
                            </select>
                            <input class='hint-font-size' type='number' min='8' max='72' value='16' />
                        </div>
                        <div class='hint-right'>
                           <img draggable='false' class='hint-svg block-move-svg' src='../Resourses/svg/move-white.svg'/>
                           <img draggable='false' class='hint-svg block-delete-svg' src='../Resourses/svg/delete-white.svg'/>
                           <input class='hint-data hint-rows' type='number' min='1' max='6' value='1' />
                        </div>                        
                    </div>
                    <textarea class='draggable-text' rows="1">${textreplace != ""
                ? textreplace
                    : "Текст"}</textarea>
                </div>`);
            blockHtml.attr("position-id", block.position_id);
            $(".img-wrap[picture-id='" + block.picture_id + "']").append(blockHtml);
            blockHtml.draggable({
                snap: "td",
                snapTolerance: 10,
                containment: blockHtml.parent(),
                handle: "img.block-move-svg"
            });
            let img = $(".img-wrap[picture-id='" + block.picture_id + "']")
                .find('.imgModalStyle')[0];
            if (img) {
                blockHtml.css("left",
                    block.position_x.replace(/,/, '.') + "px");
                blockHtml.css("top",
                    block.position_y.replace(/,/, '.') + "px");
                blockHtml.find(".draggable-text").css("width",
                    block.position_width.replace(/,/, '.') + "px");
               
                blockHtml.find(".hint-data-type").val(block.Type);
                blockHtml.find(".hint-data-weight").val(block.FontWeight);
                blockHtml.find(".hint-data-align").val(block.Alignment);
                blockHtml.find(".hint-font-size").val(block.font_size);
                $(".hint-font-size").trigger("change");
                if (block.Type === "Статичный текст из бд") {
                    blockHtml.find(".draggable-text").addClass("autotext")
                        .autocomplete({
                            source: autotexts, select: function (event, ui) {
                                blockHtml.find(".draggable-text").html(ui.item.value);
                                blockHtml.find(".draggable-text").val(blockHtml.find(".draggable-text").html());
                                blockHtml.find(".draggable-text").html("");
                                return false;
                            }
                        }).data("ui-autocomplete")._renderItem = function (ul, item) {
                            var bl = $(`<li><span></span></li>`);
                            $(bl).find("span").html(item.value);
                            return bl.appendTo(ul);
                        };
                }
                checkrows(blockHtml.find(".draggable-text")[0]);
                $(".hint-data-weight").trigger("change");
                $(".hint-data-align").trigger("change");
            }
        });
        $(target_positions.stamps).each(function() {
            var block = this;
            var blockHtml = $(
                "<div stamp-posid='" + block.StampPositionId+"' stamp-id='" + block.stamp_id +"' style='height:60px; width:60px; border-thickness: 2px; border-color: #464646' class='stampWrap'><img width='100%' class='' src = '" +
                block.image +
                "'/><div class='deleteStamp'>×<div/></div >");
            $(".img-wrap[picture-id='" + block.picture_id + "']").append(blockHtml);
            let img = $(".img-wrap[picture-id='" + block.picture_id + "']")
                .find('.imgModalStyle')[0];
            if (img) {
                let kefX = img.naturalWidth / img.width;
                let kefY = img.naturalHeight / img.height;
                blockHtml.css("left",
                   block.position_x.replace(/,/, '.') + "px");
                blockHtml.css("top",
                    block.position_y.replace(/,/, '.') + "px");
                blockHtml.css("width",
                    block.position_width.replace(/,/, '.') + "px");
                blockHtml.css("height",
                    block.position_height.replace(/,/, '.') + "px");
            }
            $(blockHtml).draggable({
                containment: $(img),
                handle: $(blockHtml).find("img")
            });
            $(blockHtml).resizable({
                containment: $(img),
                handles: 'se',
                aspectRatio: true,
                minHeight: 20,
                minWidth: 20
            });

            });
        $(target_positions.photos).each(function () {
            var block = this;
            var blockHtml = $(
                "<div photo-id='" + block.PhotoModelId + "' style='height:80px; width:60px; position:absolute; top:0; border: 2px dotted #464646' class='photoWrap'><div style='height:100%; width:100%; display: flex; justify-content: center; align-items: center; font-weight: bold; cursor:grab;'>Фото 3х4</div><div class='deleteStamp'>×<div/></div >");
            $(".img-wrap[picture-id='" + block.picture_id + "']").append(blockHtml);
            let img = $(".img-wrap[picture-id='" + block.picture_id + "']")
                .find('.imgModalStyle')[0];
            if (img) {
                blockHtml.css("left",
                    block.position_x.replace(/,/, '.') + "px");
                blockHtml.css("top",
                    block.position_y.replace(/,/, '.') + "px");
                blockHtml.css("width",
                    block.position_width.replace(/,/, '.') + "px");
                blockHtml.css("height",
                    block.position_height.replace(/,/, '.') + "px");
            }
            $(blockHtml).draggable({
                containment: $(img),
                handle: $(blockHtml).find("div")
            });
            $(blockHtml).resizable({
                containment: $(img),
                handles: 'se',
                aspectRatio: true,
                minHeight: 40,
                minWidth: 30
            });

        });
    });
        $(document).on("change",
            ".hint-data-weight",
            function () {
                $(this).closest(".draggable-div").find(".draggable-text").css("fontWeight", $(this).val().includes("Bold") ? "bold" : "normal");
                $(this).closest(".draggable-div").find(".draggable-text").css("text-decoration", $(this).val().includes("Underline") ? "underline" : "none");
            });
        $(document).on("change",
            ".hint-data-align",
            function () {
                $(this).closest(".draggable-div").find(".draggable-text").css("text-align", $(this).val() === "Слева" ? "left" : $(this).val() === "Справа" ? "right" : "center");
            });
$(document).on("change",
    ".hint-font-size",
    function() {
        $(this).parent().parent().parent().find(".draggable-text").css("font-size", this.value + "pt");
            });
        $(document).on("change", "input[name='NaturalWidth']", function () {
            $(this).closest(".modelBlockOne").find(".mainEditor").css("width", $(this).val()+"mm");
        });
        $(document).on("change", "input[name='NaturalHeight']", function () {
            $(this).closest(".modelBlockOne").find(".mainEditor").css("height", $(this).val()+"mm");
        });
        $(document).on("change", ".hint-rows", function() {
            $(this).closest(".draggable-div").find(".draggable-text")[0].rows = $(this).val();
        });
        function checkrows(e) {
            var elem = e;
            elem.rows = Math.round(elem.scrollHeight / $(elem).css('fontSize').replace('px', ''));
            $(elem).closest(".draggable-div").find(".hint-rows").val(elem.rows);
        }
$(document).on("click",
    "button.findIcon",
    function () {
        $(".ImageName").toArray().forEach((elem) => {
            if (!elem.innerText.includes($(this).parent().find("input").val())) {
                $(elem).parent().fadeOut("slow");

            } else {
                $(elem).parent().fadeIn("slow");
            }
            if ($(this).parent().find("input").val() === "") {
                $("#patternsContain").nextAll(".glo").remove();
            } else {
                let block = `<button class="glo">
                <span>Очистить поиск</span>
            </button>`;
                if ($("#patternsContain").next(".glo").length < 1)
                    $("#patternsContain").after(block);
            }
        
    });
    });
$(document).on("click",
    "button.glo",
    function() {
        $(".ImageName").parent().fadeIn(function() {
            $("button.glo").remove();
        });
    });


    $(".GridDown").click(function (e) {
        if ($(this).closest(".modelBlockOne").find("tbody").children().length > 2) {
            $(this).closest(".modelBlockOne").find("table.tableRed>tbody>tr:last").remove();
            $(this).closest(".modelBlockOne").find("table.tableRed>tbody>tr:last").children("td:not(td:last)").resizable("destroy");
        }
    
});

$(".GridUp").click(function (e) {
    $(this).closest(".modelBlockOne").find("table.tableRed>tbody").children("tr:not(tr:last)").each(function() {
        $(this).children("td:not(:last-child)").resizable("destroy");
        $(this).children("td").height("auto");
    });
    $(this).closest(".modelBlockOne").find("table.tableRed>tbody>tr:last").clone().insertAfter($(this).closest(".modelBlockOne").find("table.tableRed>tbody>tr:last"));
    
    $(this).closest(".modelBlockOne").find("table.tableRed>tbody").children("tr:not(tr:last)").each(function () {
        $(this).children("td:not(:last-child)").each(function () {
            $(this).resizable({
                containment: $(this).closest("table.tableRed"),
                handles: 's, e',
                minHeight: 10,
                minWidth: 10
            });
        });
    });
   


});


$(".rectangleWhite.Grid").click(function(e) {
    $(this).closest(".mainEditor").find('.BlockTable').toggleClass("d-none");

});


$(".rectangleWhite.Stamp").click(function(e) {

    imgModal = $(this).parents()[1].children[1];
        });

$(document).on('click',
    ".deleteStamp",
    function() {
        $(this).parent().remove();
        });
});