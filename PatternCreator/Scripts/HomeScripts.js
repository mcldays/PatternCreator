'use strict';


$(document).ready(function() {
    function findUsers(t) {
        var userId = "";
        var finded = "";
        var filter = t.val().toLowerCase().replace(/ /g, '');
        $("td>input[name='user.Name']").each(function () {
            userId = $(this).closest("td").data("userId");
            finded = $("td[data-user-id='" + userId + "']>input[name='user.Name']").val();
            finded += $("td[data-user-id='" + userId + "']>input[name='user.Surname']").val();
            finded += $("td[data-user-id='" + userId + "']>input[name='user.Patronymic']").val();
            
            if (!finded.toLowerCase().includes(filter)) {
                var tr = $("td[data-user-id='" + userId + "']:visible").closest('tr');
                tr.hide().addClass("notfounded");
                if ($("tr[data-company-id='" + tr.data("company-id") + "']:visible").length==0) {
                    $("tr[data-header-id='" + tr.data("company-id") + "']:visible").hide();
                } 

            } else {
                $("td[data-user-id='" + userId + "']:hidden").closest('tr').show().removeClass("notfounded");
                $("tr[data-header-id='" + $("td[data-user-id='" + userId + "']:visible").closest('tr').data("company-id") + "']:hidden").show();
            }

        });
        $(".closeTableBut").removeClass('rotated');
    };
    $(document).on("input", "#FindUsers", function() {
    //findUsers($(this));
    });

    $(document).on("click", ".FindButton", function() {
        //findUsers($(this).prev());
    });
});




